namespace Clickstreamer.Timing
{
    using System;
    using System.Timers;

    public class ThreadedTimer : ITimer
    {
        protected readonly Timer Timer;

        public ThreadedTimer()
        {
            this.Timer = new Timer();
        }

        public ThreadedTimer(double intervalInMillis)
        {
            this.Timer = new Timer(intervalInMillis);
        }

        public event EventHandler<TimerEventArgs> Elapsed;

        public void Start()
        {
            this.Timer.Elapsed += this.Timer_Elapsed;
            this.Timer.Start();
        }

        public void Start(double intervalInMillis)
        {
            this.Timer.Interval = intervalInMillis;
            this.Timer.Enabled = true;
            this.Timer.Elapsed += this.Timer_Elapsed;

            this.Timer.Start();
        }

        public void Stop()
        {
            this.Timer.Stop();

            this.Timer.Elapsed -= this.Timer_Elapsed;
        }

        public void Dispose()
        {
            this.Disposing(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Disposing(bool disposing)
        {
            if (disposing)
            {
                this.Timer.Dispose();
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Elapsed(this, new TimerEventArgs(e.SignalTime));
        }
    }
}