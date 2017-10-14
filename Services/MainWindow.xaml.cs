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
    using Clickstreamer.Timing;
    using Clickstreamer.UI.Controls;
    using Clickstreamer.Win32.Keyboard;
    using Clickstreamer.Win32.Mouse;
    using Newtonsoft.Json;

    public partial class MainWindow : Window, IDisposable
    {
        private readonly ISystemTrayControl tray;
        private readonly ITimer timer;
        private readonly IEventReader<MouseEventArgs> mouseEventReader;
        private readonly IEventReader<KeyboardEventArgs> keyboardEventReader;

        public MainWindow(
            ISystemTrayControl tray, 
            ITimer timer, 
            IEventReader<MouseEventArgs> mouseEventReader,
            IEventReader<KeyboardEventArgs> keyboardEventReader)
        {
            this.InitializeComponent();

            this.tray = tray ?? throw new ArgumentNullException(nameof(tray));
            this.mouseEventReader = mouseEventReader ?? throw new ArgumentNullException(nameof(mouseEventReader));
            this.keyboardEventReader = keyboardEventReader ?? throw new ArgumentNullException(nameof(keyboardEventReader));
            this.timer = timer ?? throw new ArgumentNullException(nameof(timer));

            this.tray.SetVisibility(Visibility.Visible);

            this.mouseEventReader.Start();
            this.keyboardEventReader.Start();
            this.timer.Start(60000);
        }

        private Func<Task> SaveMouseData => () => this.SaveDataAsync<MouseEventArgs>("mouse", this.mouseEventReader.Reduce());

        private Func<Task> SaveKeyboardData => () => this.SaveDataAsync<KeyboardEventArgs>("keyboard", this.keyboardEventReader.Reduce());

        public void Finalise()
        {
            this.timer.Stop();
            this.mouseEventReader.Stop();
            this.keyboardEventReader.Stop();

            Task.WaitAll(this.SaveMouseData(), this.SaveKeyboardData());
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
            this.SaveMouseData();
            this.SaveKeyboardData();
        }

        private Task SaveDataAsync<TData>(string resourceName, IEnumerable<TData> data)
        {
            return Task.Run(() =>
            {
                if (data.Any())
                {
                    // TODO: abstract out to interface
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