namespace Clickstreamer
{
    using System;
    using System.Windows;
    using Clickstreamer.Sourcing;
    using Clickstreamer.Win32.Keyboard;
    using Clickstreamer.Win32.Mouse;

    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            this.Exit += this.App_Exit;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;

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
            // propogate shutdown call to any running windows
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            KeyboardEventSourcer keyboardSourcer = new KeyboardEventSourcer(new Keyboard());
            MouseEventSourcer mouseSourcer = new MouseEventSourcer(new Mouse());

            MainWindow main = new MainWindow(mouseSourcer, keyboardSourcer);
            main.Hide();
        }
    }
}
