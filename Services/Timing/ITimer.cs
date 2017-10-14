namespace Clickstreamer.Timing
{
    using System;
    using Clickstreamer.Processing;

    public interface ITimer : ICanStart, ICanStop, IDisposable
    {
        event EventHandler<TimerEventArgs> Elapsed;

        void Start(double intervalInMillis);
    }
}