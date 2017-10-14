namespace Clickstreamer.Sourcing
{
    using System;
    using System.Collections.Generic;
    using Clickstreamer.Events;
    using Clickstreamer.Win32.Keyboard;

    public class KeyboardEventSourcer : EventReaderBase<KeyboardEventArgs>, IEventReader<EventArgs>
    {
        private const string ReaderName = "Keyboard Reader";

        public KeyboardEventSourcer(IDataObserver<KeyboardEventArgs> observer) 
            : base(KeyboardEventSourcer.ReaderName, observer)
        {
        }

        IEnumerable<EventArgs> IEventReader<EventArgs>.Reduce()
        {
            return base.Reduce();
        }
    }
}