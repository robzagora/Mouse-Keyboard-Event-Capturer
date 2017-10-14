namespace Clickstreamer.Events
{
    using System;

    public interface IDataObserver<TEvent> : IRaiseEvent<TEvent> where TEvent : EventArgs
    {
        void Subscribe();

        void Unsubscribe();
    }
}