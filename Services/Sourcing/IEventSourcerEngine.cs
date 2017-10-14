namespace Clickstreamer.Sourcing
{
    using System;
    using Clickstreamer.Processing;

    public interface IEventSourcerEngine : IProcess, IDisposable
    {
        event EventHandler<EventsReducedArgs<EventArgs>> DataReduced;
    }
}