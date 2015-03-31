using System.ComponentModel;
using Filewatcher.MDL;

namespace Filewatcher.GUI
{
    public partial class WatchView
    {
        public Watch Value { get; private set; }

        public WatchView()
        {
            Initialize(null);
        }

        public WatchView(Watch watch)
        {
            Initialize(watch);
        }

        private void Initialize(Watch watch)
        {
            InitializeComponent();
            DataContext = watch == null ? 
                new WatchViewModel() : new WatchViewModel(watch);
            InitalizeExitRequest();
            InitializeLocation();
        }

        private void InitalizeExitRequest()
        {
            var currDataContext = DataContext as WatchViewModel;
            if (currDataContext == null) return;
            currDataContext.ExitRequest += (s, eArgs) => 
            { 
                DialogResult = eArgs.DialogResult; 
                Value = (Watch)eArgs.Value; 
                Close(); 
            };
        }

        private void InitializeLocation()
        {
            if (Properties.Settings.Default.MACROVIEW_LEFT > 0)
                Left = Properties.Settings.Default.MACROVIEW_LEFT;
            if (Properties.Settings.Default.MACROVIEW_TOP > 0)
                Top = Properties.Settings.Default.MACROVIEW_TOP;
        }

        private void WindowClosing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.MACROVIEW_LEFT = Left;
            Properties.Settings.Default.MACROVIEW_TOP = Top;
            Properties.Settings.Default.Save();
        }
    }
}
