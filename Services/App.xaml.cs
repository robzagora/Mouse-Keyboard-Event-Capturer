namespace Clickstreamer
{
    using System;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using Clickstreamer.Sourcing;
    using Clickstreamer.Timing;
    using Clickstreamer.UI.Controls;
    using Clickstreamer.Win32.Keyboard;
    using Clickstreamer.Win32.Mouse;
    using UiResources = Clickstreamer.Resources;

    public partial class App : Application
    {
        public const string Name = "Clickstreamer",
            TrayContextMenuControlName = "TrayContextMenu",
            TrayCloseApplicationMenuItem = "CloseApplicationMenuItem";

        private const string MutexName = "mouseAndKeyboardDataCapturer";

        private static Mutex mutex;

        private MainWindow mainWindow;

        public App()
        {
            if (Mutex.TryOpenExisting(App.MutexName, out Mutex result))
            {
                Application.Current.Shutdown();
            }
            else
            {
                App.mutex = new Mutex(true, App.MutexName);

                AppDomain.CurrentDomain.UnhandledException += this.CurrentDomain_UnhandledException;

                this.Exit += this.App_Exit;
            }
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;

            // TODO: perform logging to an external location(s)
            if (ex == null)
            {
                Console.WriteLine("Unhandled exception caught without the exception object!");
            }
            else
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            if (App.mutex != null)
            {
                this.mainWindow.Finalise();
                this.mainWindow.Dispose();

                App.mutex.ReleaseMutex();
                App.mutex.Dispose();
            }
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            ThreadedTimer timer = new ThreadedTimer();

            // TODO: create DI bindings container 
            // TODO: create IFactory 
            KeyboardEventSourcer keyboardSourcer = new KeyboardEventSourcer(new Keyboard());
            MouseEventSourcer mouseSourcer = new MouseEventSourcer(new Mouse());

            ContextMenu menu = App.Current.TryFindResource(App.TrayContextMenuControlName) as ContextMenu;
            SystemTrayControl tray = new SystemTrayControl(menu, UiResources.App, App.Name);

            this.mainWindow = new MainWindow(tray, timer, mouseSourcer, keyboardSourcer);
            this.mainWindow.Hide();
        }
    }
}