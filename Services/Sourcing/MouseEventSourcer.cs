namespace Clickstreamer.Sourcing
{
    using System;
    using System.Collections.Generic;
    using Clickstreamer.Events;
    using Clickstreamer.Win32.Mouse;

    public class MouseEventSourcer : EventReaderBase<MouseEventArgs>, IEventReader<EventArgs>
    {
        private const string ReaderName = "Mouse Reader";

        public MouseEventSourcer(IDataObserver<MouseEventArgs> observer) 
            : base(MouseEventSourcer.ReaderName, observer)
        {
        }

        IEnumerable<EventArgs> IEventReader<EventArgs>.Reduce()
        {
            return base.Reduce();
        }
    }
}