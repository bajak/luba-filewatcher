using System;
using System.Collections.Generic;
using Filewatcher.MDL;
using Filewatcher.BLL;
using System.Linq;

namespace Filewatcher.GUI
{
    internal class MacroViewModel : PropertyBase 
    {
        public MacroViewModel()
        {
            InitializeCommands();
        }

        public List<ParamMacro> Macros 
        { get { return Parameter.Macros.Values.ToList(); } } 

        public ParamMacro SelectedMacro {
            get { return GetProperty<ParamMacro>(); }
            set { SetProperty(value); }
        }

        public RelayCommand OKCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public EventHandler<ExitRequestEventArgs> ExitRequest;

        protected void OnExitRequest(ExitRequestEventArgs e)
        {
            var handler = ExitRequest;
            if (handler == null) return;
            handler(this, e);
        }

        private void InitializeCommands()
        {
            OKCommand = new RelayCommand(
                ()=> OnExitRequest(new ExitRequestEventArgs(true, SelectedMacro.Definition)),
                () => SelectedMacro != null);

            CancelCommand = new RelayCommand(
                () => OnExitRequest(new ExitRequestEventArgs(false, null)),
                () => true);
        }
    }
}
