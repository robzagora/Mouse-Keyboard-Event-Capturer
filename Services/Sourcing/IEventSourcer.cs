namespace Clickstreamer.Sourcing
{
    using System;
    using System.Collections.Generic;
    using Clickstreamer.Processing;

    public interface IEventReader<TEventArgs> : ICanStart, ICanStop where TEventArgs : EventArgs
    {
        string Name { get; }

        IEnumerable<TEventArgs> Reduce();
    }
}