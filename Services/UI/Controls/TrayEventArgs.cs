namespace Clickstreamer.UI.Controls
{
    using System;

    public class TrayEventArgs : EventArgs
    {
        private string state;

        public TrayEventArgs(string state)
        {
            this.state = state ?? throw new ArgumentNullException(nameof(state));
        }

        public string State
        {
            get { return this.state; }
        }
    }
}