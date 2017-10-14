namespace Clickstreamer.UI.Controls
{
    using System;
    using System.Windows;

    public interface ISystemTrayControl : IDisposable
    {
        event EventHandler<TrayEventArgs> StateChanged;

        void SetVisibility(Visibility visiblity);
    }
}