namespace Clickstreamer.Sourcing
{
    using System;
    using System.Collections.Generic;

    public interface IEventReader<TEventArgs> where TEventArgs : EventArgs
    {
        IEnumerable<TEventArgs> Reduce();

        void Start();

        void Stop();
    }
}