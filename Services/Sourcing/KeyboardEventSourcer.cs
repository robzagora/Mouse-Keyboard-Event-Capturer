namespace Clickstreamer.Sourcing
{
    using Clickstreamer.Events;
    using Clickstreamer.Win32.Keyboard;

    public class KeyboardEventSourcer : EventReaderBase<KeyboardEventArgs>
    {
        public KeyboardEventSourcer(IDataObserver<KeyboardEventArgs> observer) : base(observer)
        {
        }
    }
}