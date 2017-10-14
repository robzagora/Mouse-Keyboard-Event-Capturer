namespace Clickstreamer.UI.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using Hardcodet.Wpf.TaskbarNotification;
    using DrawingIcon = System.Drawing.Icon;

    public class SystemTrayControl : ISystemTrayControl
    {
        public const string ShowState = "show",
            HideState = "hide";

        protected readonly TaskbarIcon TrayIcon;
        
        public SystemTrayControl(ContextMenu menu, DrawingIcon icon, string toolTipText)
        {
            if (menu == null)
            {
                throw new ArgumentNullException(nameof(menu), "No context menu has been provided");
            }

            if (icon == null)
            {
                throw new ArgumentNullException(nameof(icon), "No tray icon has been provided");
            }

            this.TrayIcon = new TaskbarIcon
            {
                MenuActivation = PopupActivationMode.LeftOrRightClick,
                Visibility = Visibility.Hidden,
                Icon = icon,
                ToolTipText = toolTipText,
                ContextMenu = menu // TODO: abstract out to interface to allow search for meun items
            };

            MenuItem close = this.FindMenuItem(menu, App.TrayCloseApplicationMenuItem);
            close.Header = "Terminate";
            close.Click += this.Close_Click;
        }

        public event EventHandler<TrayEventArgs> StateChanged;

        public void SetVisibility(Visibility visiblity)
        {
            this.TrayIcon.Visibility = visiblity;
        }

        public void Dispose()
        {
            this.Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.TrayIcon.Dispose();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void RequestApplicationDisplay()
        {
            this.StateChanged(this, new TrayEventArgs(SystemTrayControl.ShowState));
        }

        private MenuItem FindMenuItem(ContextMenu menu, string name)
        {
            ItemCollection collection = menu.Items;

            MenuItem item = null;

            bool found = false;
            while (collection.MoveCurrentToNext() && !found)
            {
                item = collection.CurrentItem as MenuItem;

                if (item.Name == name)
                {
                    found = true;
                    collection.MoveCurrentToFirst();
                }
            }

            return item;
        }
    }
}