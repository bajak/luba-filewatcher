using System.ComponentModel;
using System.Windows;

namespace Filewatcher.GUI
{
    public partial class MacroView
    {

        public string Value { get; set; }

        public MacroView()
        {
            InitializeComponent();
            InitalizeExitRequest();
            InitializeLocation();
        }

        private void InitalizeExitRequest()
        {
            var currDataContext = DataContext as MacroViewModel;
            if (currDataContext == null) return;
            currDataContext.ExitRequest += (s, eArgs) => 
            {
                if (eArgs.DialogResult)
                    Value = eArgs.Value.ToString();
                
                DialogResult = eArgs.DialogResult;
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
