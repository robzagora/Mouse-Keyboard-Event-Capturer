namespace Capturer
{
    using System.Windows;
    using Clickstreamer.Win32;

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

           //  Mouse.MouseAction += Mouse_MouseAction;
            Keyboard.KeyboardAction += Keyboard_KeyboardAction1;

            this.Hide();
        }

        private void Mouse_MouseAction(object sender, System.EventArgs e)
        {
            System.Console.WriteLine(e);
        }

        private void Keyboard_KeyboardAction1(object sender, Keyboard.KeyboardEventArgs e)
        {
            System.Console.WriteLine(e.Code);
        }
    }
}