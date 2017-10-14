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
    using Clickstreamer.UI.Controls;
    using Clickstreamer.Win32.Keyboard;
    using Clickstreamer.Win32.Mouse;
    using Newtonsoft.Json;

    public partial class MainWindow : Window, IDisposable
    {
        private readonly ISystemTrayControl tray;
        private readonly IEventReader<MouseEventArgs> mouseEventReader;
        private readonly IEventReader<KeyboardEventArgs> keyboardEventReader;

        private readonly Timer timer;

        public MainWindow(ISystemTrayControl tray, IEventReader<MouseEventArgs> mouseEventReader, IEventReader<KeyboardEventArgs> keyboardEventReader)
        {
            this.InitializeComponent();

            this.tray = tray ?? throw new ArgumentNullException(nameof(tray));
            this.mouseEventReader = mouseEventReader ?? throw new ArgumentNullException(nameof(mouseEventReader));
            this.keyboardEventReader = keyboardEventReader ?? throw new ArgumentNullException(nameof(keyboardEventReader));

            this.timer = new Timer(60000);

            this.timer.Elapsed += this.SaveTimer_Elapsed;
            this.timer.AutoReset = true;
            this.timer.Enabled = true;

            this.tray.SetVisibility(Visibility.Visible);

            this.mouseEventReader.Start();
            this.keyboardEventReader.Start();
            this.timer.Start();
        }

        public void Finalise()
        {
            this.timer.Stop();
            this.mouseEventReader.Stop();
            this.keyboardEventReader.Stop();

            Task.WaitAll(
                this.SaveDataAsync<MouseEventArgs>("mouse", this.mouseEventReader.Reduce()),
                this.SaveDataAsync<KeyboardEventArgs>("keyboard", this.keyboardEventReader.Reduce()));
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.timer.Dispose();
                this.tray.Dispose();
            }
        }

        private void SaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.SaveDataAsync<MouseEventArgs>("mouse", this.mouseEventReader.Reduce());
            this.SaveDataAsync<KeyboardEventArgs>("keyboard", this.keyboardEventReader.Reduce());
        }

        private Task SaveDataAsync<TData>(string resourceName, IEnumerable<TData> data)
        {
            return Task.Run(() =>
            {
                if (data.Any())
                {
                    File.WriteAllText(
                        Path.Combine(
                            Environment.GetFolderPath(Environment.SpecialFolder.Desktop), 
                            resourceName + "-" + DateTime.UtcNow.ToString("dd-M-yyyy--HH-mm-ss")), 
                        JsonConvert.SerializeObject(data));
                }
            });
        }
    }
}