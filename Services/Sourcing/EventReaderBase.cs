namespace Clickstreamer.Sourcing
{
    using System;
    using System.Collections.Generic;
    using Clickstreamer.Events;

    public abstract class EventReaderBase<TEventArgs> : IEventReader<TEventArgs> where TEventArgs : EventArgs
    {
        private readonly IList<TEventArgs> events;

        private readonly IDataObserver<TEventArgs> eventObserver;

        private readonly object @lock = new object();

        protected EventReaderBase(IDataObserver<TEventArgs> eventObserver)
        {
            this.events = new List<TEventArgs>();
            this.eventObserver = eventObserver;
        }

        public IEnumerable<TEventArgs> Reduce()
        {
            IEnumerable<TEventArgs> items;

            lock (this.@lock)
            {
                items = new List<TEventArgs>(this.events);

                events.Clear();
            }

            return items;
        }

        public void Start()
        {
            this.eventObserver.OnEvent += this.EventRaiser_OnEvent;
            this.eventObserver.Subscribe();
        }

        public void Stop()
        {
            this.eventObserver.Unsubscribe();
            this.eventObserver.OnEvent -= this.EventRaiser_OnEvent;
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