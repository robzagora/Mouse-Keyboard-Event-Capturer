namespace Clickstreamer.Win32.Mouse
{
    using System;

    public class MouseEventArgs : EventArgs
    {
        private int x, y;

        private uint flags, mouseData, time;

        public MouseEventArgs(int x, int y, uint flags, uint mouseData, uint time)
        {
            this.x = x;
            this.y = y;
            this.time = time;
            this.flags = flags;
            this.mouseData = mouseData;
        }

        public int X
        {
            get { return this.x; }
        }

        public int Y
        {
            get { return this.y; }
        }

        public uint Flags
        {
            get { return this.flags; }
        }

        public uint MouseData
        {
            get { return this.mouseData; }
        }

        public uint Time
        {
            get { return this.time; }
        }
    }
}