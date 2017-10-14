namespace Clickstreamer.Sourcing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Clickstreamer.Timing;

    public class EventSourcerEngine : IEventSourcerEngine
    {
        protected readonly ITimer Timer;
        protected readonly IEnumerable<IEventReader<EventArgs>> Readers;

        private readonly string name;

        public EventSourcerEngine(string name, ITimer timer, IEnumerable<IEventReader<EventArgs>> readers)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (timer == null)
            {
                throw new ArgumentNullException(nameof(timer));
            }

            if (readers == null || !readers.Any())
            {
                throw new ArgumentNullException(nameof(readers));
            }

            this.name = name;
            this.Timer = timer;
            this.Readers = readers;
        }

        public event EventHandler<EventsReducedArgs<EventArgs>> DataReduced;

        public string Name
        {
            get { return this.name; }
        }

        private Action<IEventReader<EventArgs>> OnReduce => source => this.DataReduced(this, new EventsReducedArgs<EventArgs>(source.Name, source.Reduce(), DateTime.UtcNow));

        public virtual void Start()
        {
            foreach (var reader in this.Readers)
            {
                reader.Start();
            }

            this.Timer.Elapsed += this.Timer_Elapsed;
            this.Timer.Start();
        }
        
        public virtual void Stop()
        {
            this.Timer.Stop();
            this.Timer.Elapsed -= this.Timer_Elapsed;

            foreach (var reader in this.Readers)
            {
                reader.Stop();

                this.OnReduce(reader);
            }
        }

        public void Dispose()
        {
            this.Disposing(true);

            GC.SuppressFinalize(this);
        }

        protected void Disposing(bool disposing)
        {
            if (disposing)
            {
                this.Timer.Dispose();
            }
        }

        private void Timer_Elapsed(object sender, TimerEventArgs e)
        {
            foreach (var reader in this.Readers)
            {
                this.OnReduce(reader);
            }
        }
    }
}