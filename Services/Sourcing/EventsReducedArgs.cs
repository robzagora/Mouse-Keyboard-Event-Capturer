namespace Clickstreamer.Sourcing
{
    using System;
    using System.Collections.Generic;

    public class EventsReducedArgs<TData> : EventArgs
    {
        private readonly string reducerName;
        private readonly IEnumerable<TData> data;
        private readonly DateTime time;

        public EventsReducedArgs(string reducerName, IEnumerable<TData> data, DateTime time)
        {
            this.reducerName = reducerName;
            this.data = data ?? new TData[0];
            this.time = time;
        }

        public string ReducerName
        {
            get { return this.reducerName; }
        }

        public IEnumerable<TData> Data
        {
            get { return this.data; }
        }

        public DateTime Time
        {
            get { return this.time; }
        }
    }
}