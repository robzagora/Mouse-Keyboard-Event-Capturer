namespace Clickstreamer.Win32.Keyboard
{
    using System;

    public class KeyboardEventArgs : EventArgs
    {
        private uint keyCode;
        private uint scanCode;
        private uint flags;
        private uint time;

        public KeyboardEventArgs(uint keyCode, uint scanCode, uint flags, uint time)
        {
            this.keyCode = keyCode;
        }

        public uint KeyCode
        {
            get { return this.keyCode; }
        }

        public uint ScanCode
        {
            get { return this.scanCode; }
        }

        public uint Flags
        {
            get { return this.flags; }
        }

        public uint Time
        {
            get { return this.time; }
        }
    }
}