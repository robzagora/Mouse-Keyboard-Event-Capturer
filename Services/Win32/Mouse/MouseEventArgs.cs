namespace Clickstreamer.Win32.Mouse
{
    using System;

    public class MouseEventArgs : EventArgs
    {
        private int x, y;

        private uint time;

        public MouseEventArgs(int x, int y, uint time)
        {
            this.x = x;
            this.y = y;
            this.time = time;
        }

        public int X { get { return this.x; } }

        public int Y { get { return this.y; } }

        public uint Time { get { return this.time; } }
    }
}