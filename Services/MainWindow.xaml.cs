namespace Clickstreamer
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using Clickstreamer.Sourcing;
    using Clickstreamer.UI.Controls;
    using Newtonsoft.Json;

    public partial class MainWindow : Window, IDisposable
    {
        private readonly IEventSourcerEngine eventSourcer;
        private readonly ISystemTrayControl tray;

        private readonly string dataStorePath;

        public MainWindow(IEventSourcerEngine eventSourcer, ISystemTrayControl tray, string dataStorePath)
        {
            this.InitializeComponent();

            this.eventSourcer = eventSourcer ?? throw new ArgumentNullException(nameof(eventSourcer));
            this.tray = tray ?? throw new ArgumentNullException(nameof(tray));

            if (string.IsNullOrWhiteSpace(dataStorePath))
            {
                throw new ArgumentNullException(nameof(dataStorePath));
            }

            this.dataStorePath = dataStorePath;

            this.tray.SetVisibility(Visibility.Visible);

            this.eventSourcer.DataReduced += this.EventSourcer_DataReduced;
            this.eventSourcer.Start();
        }

        public void Finalise()
        {
            this.eventSourcer.Stop();
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
                this.eventSourcer.Dispose();
                this.tray.Dispose();
            }
        }

        private void EventSourcer_DataReduced(object sender, EventsReducedArgs<EventArgs> e)
        {
            Console.WriteLine("Event Sourcer Engine performed a reduce job at: " + e.Time + ", total reduced amount: " + e.Data.Count());

            this.SaveDataAsync(e.ReducerName, e.Data);
        }

        private Task SaveDataAsync<TData>(string resourceName, IEnumerable<TData> data)
        {
            return Task.Run(() =>
            {
                if (data.Any())
                {
                    string savePath = Path.Combine(
                        this.dataStorePath,
                        resourceName,
                        DateTime.UtcNow.ToString("dd-M-yyyy--HH-mm-ss-fffffff"));

                    // TODO: abstract out to interface
                    File.WriteAllText(savePath, JsonConvert.SerializeObject(data));
                }
            });
        }
    }
}