namespace Clickstreamer.Timing
{
    using System;

    public class TimerEventArgs : EventArgs
    {
        private DateTime time;

        public TimerEventArgs(DateTime time)
        {
            this.time = time;
        }

        public DateTime Time
        {
            get { return this.time; }
        }
    }
}