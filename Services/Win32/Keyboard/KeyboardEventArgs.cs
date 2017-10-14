namespace Clickstreamer.Win32.Keyboard
{
    using System;

    public class KeyboardEventArgs : EventArgs
    {
        private int code;

        public KeyboardEventArgs(int code)
        {
            this.code = code;
        }

        public int Code { get { return this.code; } }
    }
}