namespace Clickstreamer.Sourcing
{
    using System;
    using System.Collections.Generic;
    using Clickstreamer.Events;

    public abstract class EventReaderBase<TEventArgs> : IEventReader<TEventArgs> where TEventArgs : EventArgs
    {
        private readonly string name;
        private readonly IList<TEventArgs> events;

        private readonly IDataObserver<TEventArgs> eventObserver;

        private readonly object @lock = new object();

        protected EventReaderBase(string name, IDataObserver<TEventArgs> eventObserver)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.name = name;
            this.events = new List<TEventArgs>();
            this.eventObserver = eventObserver;
        }

        public string Name
        {
            get { return this.name; }
        }

        public IEnumerable<TEventArgs> Reduce()
        {
            IEnumerable<TEventArgs> items;

            lock (this.@lock)
            {
                items = new List<TEventArgs>(this.events);

                this.events.Clear();
            }

            return items;
        }

        public void Start()
        {
            this.eventObserver.Event += this.EventRaiser_OnEvent;
            this.eventObserver.Subscribe();
        }

        public void Stop()
        {
            this.eventObserver.Unsubscribe();
            this.eventObserver.Event -= this.EventRaiser_OnEvent;
        }

        private void EventRaiser_OnEvent(object sender, TEventArgs e)
        {
            lock (this.@lock)
            {
                this.events.Add(e);
            }
        }
    }
}