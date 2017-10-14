namespace Clickstreamer.Sourcing
{
    using Clickstreamer.Events;
    using Clickstreamer.Win32.Mouse;

    public class MouseEventSourcer : EventReaderBase<MouseEventArgs>
    {
        public MouseEventSourcer(IDataObserver<MouseEventArgs> observer) : base(observer)
        {
        }
    }
}