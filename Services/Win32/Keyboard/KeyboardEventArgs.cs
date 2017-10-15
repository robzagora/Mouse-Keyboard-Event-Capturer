namespace Clickstreamer.Win32.Keyboard
{
    using System;

    public class KeyboardEventArgs : EventArgs
    {
        private uint keyCode;
        private uint scanCode;
        private uint flags;
        private uint time;

        private string eventType;

        public KeyboardEventArgs(uint keyCode, uint scanCode, uint flags, uint time, string eventType)
        {
            this.keyCode = keyCode;
            this.scanCode = scanCode;
            this.flags = flags;
            this.time = time;
            this.eventType = eventType;
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

        public string KeyType
        {
            get { return this.eventType; }
        }
    }
}