using System;
using System.Windows;
using Filewatcher.MDL;

namespace Filewatcher.GUI
{
    internal class WatchViewModel : PropertyBase
    {
        public WatchViewModel()
        {
            InitializeClass(
                ViewHelper.CreateDefaultWatch());
        }

        public WatchViewModel(Watch watch)
        {
            InitializeClass(watch);
        }

        private void InitializeClass(Watch watch)
        {
            Watch = watch;
            InitializeCommands();
            InitializeProperties();
            WatchValidation.ValidateWatches(new[] { watch });
        }

        public EventHandler<ExitRequestEventArgs> ExitRequest;

        public Watch Watch { get; set; }

        public RelayCommand SaveCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        public RelayCommand SelectProcessFileCommand { get; private set; }
        public RelayCommand SelectWatchFolderCommand { get; private set; }
        public RelayCommand SelectWorkingFolderCommand { get; private set; }
        public RelayCommand InsertMacroCommand1 { get; private set; }
        public RelayCommand InsertMacroCommand2 { get; private set; }

        public bool ShowMessageShellExecute
        {
            get { return Properties.Settings.Default.SHOWMESSAGE_SHELLEXECUTE; }
            set
            {
                Properties.Settings.Default.SHOWMESSAGE_SHELLEXECUTE = value;
                Properties.Settings.Default.Save();
            }
        }

        public bool ShowMessageOutputPath
        {
            get { return Properties.Settings.Default.SHOWMESSAGE_OUTPUTPATH; }
            set
            {
                Properties.Settings.Default.SHOWMESSAGE_OUTPUTPATH = value;
                Properties.Settings.Default.Save();
            }
        }

        private void InitializeCommands()
        {
            SaveCommand = new RelayCommand(
                () => OnExitRequest(true), 
                () => Watch.Errors.Count <= 0);

            CancelCommand = new RelayCommand(
                () => OnExitRequest(false), 
                () => true);
            
            SelectProcessFileCommand = new RelayCommand(() => {
                var filePath = ViewHelper.GetOpenFilePath(
                    ViewConfig.WATCH_GENERIC_OPENFILE_DEF_EXTENSION,
                    ViewConfig.WATCH_GENERIC_OPENFILE_ALLFILES, false);
                Watch.Process = 
                    String.IsNullOrEmpty(filePath) ? Watch.Process : filePath;
            }, () => true);

            SelectWatchFolderCommand = new RelayCommand(() => {
                var folderPath = ViewHelper.GetFolderPath(
                    ViewConfig.WATCH_GENERIC_OPENFOLDER_ROOT);
                Watch.WatchFolder = 
                    String.IsNullOrEmpty(folderPath) ? Watch.WatchFolder : folderPath;
            }, () => true);

            SelectWorkingFolderCommand = new RelayCommand(() => {
                var folderPath = ViewHelper.GetFolderPath(
                    ViewConfig.WATCH_GENERIC_OPENFOLDER_ROOT);
                Watch.WorkingFolder = 
                    String.IsNullOrEmpty(folderPath) ? Watch.WorkingFolder : folderPath;
            }, () => true);

            InsertMacroCommand1 = new RelayCommand(e => {
                var window = (Window)e;
                window.ShowInTaskbar = false;
                var macroView = new MacroView { Topmost=true, Owner=window };
                var dialogResult = macroView.ShowDialog();
                window.ShowInTaskbar = true;
                if (dialogResult != true)
                    return;
                Watch.Parameter += 
                    (!Watch.Parameter.EndsWith(" ") ? " " : "") + 
                    macroView.Value;
            }, e => true);

            InsertMacroCommand2 = new RelayCommand(e =>
            {
                var window = (Window)e;
                window.ShowInTaskbar = false;
                var macroView = new MacroView { Topmost = true, Owner = window };
                var dialogResult = macroView.ShowDialog();
                window.ShowInTaskbar = true;
                if (dialogResult != true)
                    return;
                Watch.OutputPath +=
                    (!Watch.OutputPath.EndsWith(" ") ? " " : "") +
                    macroView.Value;
            }, e => true);
        }

        private void InitializeProperties() 
        {
            Watch.PropertyChanged += (o, e) => {
                if (e.PropertyName != PropertyHelper<Watch>.GetProperty(x => x.UseShellExecute).Name
                &&  Watch.UseShellExecute && ShowMessageShellExecute)
                {
                    MessageBox.Show(Properties.Resources.WARNINGMESSAGE_TEXT_SHELLEXECUTE, 
                                    Properties.Resources.WARNINGMESSAGE_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                    ShowMessageShellExecute = false;
                }

                if (e.PropertyName != PropertyHelper<Watch>.GetProperty(x => x.OutputPath).Name
                && Watch.LogOutput && String.IsNullOrEmpty(Watch.OutputPath))
                {
                    Watch.LogOutput = false;
                }

                if (e.PropertyName != PropertyHelper<Watch>.GetProperty(x => x.OutputPath).Name
                && Watch.LogOutput && !String.IsNullOrEmpty(Watch.OutputPath) && ShowMessageOutputPath)
                {
                    MessageBox.Show(Properties.Resources.WARNINGMESSAGE_TEXT_OUTPUTPATH,
                                    Properties.Resources.WARNINGMESSAGE_TITLE, MessageBoxButton.OK, MessageBoxImage.Warning);
                    ShowMessageOutputPath = false;   
                }
            };
        }

        protected void OnExitRequest(bool dialogResult)
        {
            var handler = ExitRequest;
            if (handler == null) return;
            handler(this, new ExitRequestEventArgs(
                dialogResult, Watch));
        }
    }
}
