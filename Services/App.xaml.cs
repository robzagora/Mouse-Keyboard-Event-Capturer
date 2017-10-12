namespace Clickstreamer
{
    using System;
    using System.Windows;
    using Clickstreamer.Win32;

    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            //Mouse.Subscribe();
            Keyboard.Subscribe();

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
            //Mouse.Unsubscribe();
            Keyboard.Unsubscribe();
        }
    }
}
