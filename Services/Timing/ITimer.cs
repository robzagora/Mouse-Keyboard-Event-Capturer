namespace Clickstreamer.Timing
{
    using System;

    public interface ITimer : IDisposable
    {
        event EventHandler<TimerEventArgs> Elapsed;

        void Start();

        void Start(double intervalInMillis);

        void Stop();
    }
}