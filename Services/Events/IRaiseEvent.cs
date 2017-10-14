namespace Clickstreamer.Events
{
    using System;

    public interface IRaiseEvent<TEvent> where TEvent : EventArgs
    {
        event EventHandler<TEvent> Event;
    }
}