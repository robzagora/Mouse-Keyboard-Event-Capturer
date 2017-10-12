namespace Capturer
{
    using System.Windows;
    using Clickstreamer.Win32;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            //Mouse.Subscribe();
            Keyboard.Subscribe();

            this.Exit += App_Exit;
        }

        private void App_Exit(object sender, ExitEventArgs e)
        {
            //Mouse.Unsubscribe();
            Keyboard.Unsubscribe();
        }
    }
}
