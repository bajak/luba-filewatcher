using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Windows;

namespace Filewatcher.GUI
{
    public partial class MainView
    {
        public MainView()
        {
            InitializeComponent();
            InitializeNotifyIcon();
            InitializeLocation();
            InitializeSize();
        }

        NotifyIcon _notifyIcon;
        WindowState _lastState;

        private void WindowStateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                _notifyIcon.Visible = true;
                ShowInTaskbar = false;
            }
            else
            {
                _lastState = WindowState;
                _notifyIcon.Visible = false;
                ShowInTaskbar = true;
            }
        }

        private void InitializeNotifyIcon()
        {
            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = Properties.Resources.MAIN_LABEL_NOTIFYICON;
            _notifyIcon.Icon = Properties.Resources.MAIN_IMAGE_NOTIFYICON;
            _notifyIcon.Click += (s, e) => { WindowState = _lastState; Activate(); };
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            if (_notifyIcon != null) { _notifyIcon.Dispose(); }
        }

        private void InitializeSize()
        {
            if (Properties.Settings.Default.MAINVIEW_WIDTH > 0)
                Width = Properties.Settings.Default.MAINVIEW_WIDTH;
            if (Properties.Settings.Default.MAINVIEW_HEIGHT > 0)
                Height = Properties.Settings.Default.MAINVIEW_HEIGHT;
        }

        private void InitializeLocation()
        {
            if (Properties.Settings.Default.MAINVIEW_LEFT > 0)
                Left = Properties.Settings.Default.MAINVIEW_LEFT;
            if (Properties.Settings.Default.MAINVIEW_TOP > 0)
                Top = Properties.Settings.Default.MAINVIEW_TOP;
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.MAINVIEW_WIDTH = Width;
            Properties.Settings.Default.MAINVIEW_HEIGHT = Height;
            Properties.Settings.Default.MAINVIEW_LEFT = Left;
            Properties.Settings.Default.MAINVIEW_TOP = Top;
            Properties.Settings.Default.Save();
        }

        private void WindowContentRendered(object sender, EventArgs e)
        {
            Topmost = false;
        }

        private void WindowInitialized(object sender, EventArgs e)
        {
            Topmost = true;
        }
    }
}
