namespace Clickstreamer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Timers;
    using System.Windows;
    using Clickstreamer.Sourcing;
    using Clickstreamer.Win32.Keyboard;
    using Clickstreamer.Win32.Mouse;
    using Newtonsoft.Json;

    public partial class MainWindow : Window
    {
        private readonly Timer saveTimer;

        private readonly IEventReader<MouseEventArgs> mouseEventReader;
        private readonly IEventReader<KeyboardEventArgs> keyboardEventReader;

        public MainWindow(IEventReader<MouseEventArgs> mouseEventReader, IEventReader<KeyboardEventArgs> keyboardEventReader)
        {
            this.InitializeComponent();

            this.mouseEventReader = mouseEventReader ?? throw new ArgumentNullException(nameof(mouseEventReader));
            this.keyboardEventReader = keyboardEventReader ?? throw new ArgumentNullException(nameof(keyboardEventReader));

            this.saveTimer = new Timer();

            this.saveTimer.Elapsed += this.SaveTimer_Elapsed;
            this.saveTimer.AutoReset = true;
            this.saveTimer.Interval = 60000;
            this.saveTimer.Enabled = true;

            this.mouseEventReader.Start();
            this.keyboardEventReader.Start();

            this.saveTimer.Start();
        }
        
        private void SaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.SaveData<MouseEventArgs>("mouse", this.mouseEventReader.Reduce());
            this.SaveData<KeyboardEventArgs>("keyboard", this.keyboardEventReader.Reduce());
        }

        private void SaveData<TData>(string resourceName, IEnumerable<TData> data)
        {
            Task.Run(() =>
            {
                if (data.Any())
                {
                    File.WriteAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), resourceName + "-" + DateTime.UtcNow.ToString("dd-M-yyyy--HH-mm-ss")), JsonConvert.SerializeObject(data));
                }
            });
        }
    }
}