using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using Filewatcher.BLL;
using Filewatcher.MDL;

namespace Filewatcher.GUI
{
    internal class MainViewModel : PropertyBase
    {
        public MainViewModel(IUow uow, IDispatcherContext dispatcher)
        {
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-us");
            if (!SaveHistories)
                uow.Histories.Clear();
            Uow = uow;
            _dispatcher = dispatcher;
            Watches = uow.Watches;
            Histories = uow.Histories;
            Options = ViewHelper.CreateDefaultOptions();
            InitializeOutputCommands();
            InitializeHistoryCommands();
            InitializeWatchCommands();
            InitializeOptionCommands();
            InitializeInfoCommands();
            InitializeOutputText();
            InitializeBalloons();
            InitializeLogic();
        }

        private readonly IDispatcherContext _dispatcher;

        public IObservableRepository<Watch> Watches { get; private set; }
        public IObservableRepository<History> Histories { get; private set; }
        public Options Options { get; private set; }
        public IUow Uow { get; private set; }

        public RelayCommand RemoveSelectedHistories { get; private set; }
        public RelayCommand ClearHistory { get; private set; }

        public RelayCommand EnableSelectedWatches {get; private set;}
        public RelayCommand DisableSelectedWatches {get; private set;}
        public RelayCommand AddWatch {get; private set;}
        public RelayCommand EditWatch {get; private set;}
        public RelayCommand RemoveWatches {get; private set;}

        public RelayCommand ClearOutputText { get; private set; }
        public RelayCommand SaveOutputText { get; private set; }

        public RelayCommand SelectOutputPath {get; private set;}

        public RelayCommand VisitWebsite {get; private set;}

        public bool SaveHistories
        {
            get { return Properties.Settings.Default.SAVEHISTORIES; }
            set
            {
                Properties.Settings.Default.SAVEHISTORIES = value;
                Properties.Settings.Default.Save();
            }
        }

        public bool SaveOutput
        {
            get { return Properties.Settings.Default.SAVEOUTPUT; }
            set
            {
                Properties.Settings.Default.SAVEOUTPUT = value;
                Properties.Settings.Default.Save();
            }
        }

        public bool PopupsVisible
        {
            get { return Properties.Settings.Default.SHOWPOPUPS; }
            set
            {
                Properties.Settings.Default.SHOWPOPUPS = value;
                Properties.Settings.Default.Save();
            }
        }

        public bool Message
        {
            get { return Properties.Settings.Default.SHOWPOPUPS; }
            set
            {
                Properties.Settings.Default.SHOWPOPUPS = value;
                Properties.Settings.Default.Save();
            }
        }

        public string OutputText 
        { 
            get { return GetProperty<string>(); } 
            set { SetProperty(value); } 
        }

        private void InitializeOutputCommands()
        {
            ClearOutputText = new RelayCommand(() =>
            {
                OutputText = string.Empty;
            }, () => !String.IsNullOrEmpty(OutputText));
        }

        private void InitializeHistoryCommands()
        {
            RemoveSelectedHistories = new RelayCommand(selectedItemsObj => {
                var selectedItems = (IList)selectedItemsObj;
                RemoveItems(selectedItems, Uow.Histories);
                Uow.Commit();
            }, selectedItems => 
               selectedItems != null && ((IList)selectedItems).Count > 0);

            ClearHistory = new RelayCommand(selectedItemsObj => {
                ((IList)selectedItemsObj).Clear();
                Uow.Histories.Clear();
                Uow.Commit();
            }, selectedItemsObj => Uow.Histories.Count > 0);
        }

        private void InitializeWatchCommands()
        {
            EnableSelectedWatches = new RelayCommand(selectedItemsObj => {
                var selectedItems = (IList)selectedItemsObj;
                ChangeWatchEnabledState(selectedItems, WatchState.Enabled);
                Uow.Commit();
            }, selectedItems => 
                selectedItems != null && ((IList)selectedItems).Cast<Watch>().Any(watch => watch.State == WatchState.Disabled));

            DisableSelectedWatches = new RelayCommand(selectedItemsObj => {
                var selectedItems = (IList)selectedItemsObj;
                ChangeWatchEnabledState(selectedItems, WatchState.Disabled);
                Uow.Commit();
            }, selectedItems => 
                selectedItems != null && ((IList)selectedItems).Cast<Watch>().Any(watch => watch.State == WatchState.Enabled));

            AddWatch = new RelayCommand(e => {
                var window = (MainView)e;
                var watchView = new WatchView { Owner=window, Topmost=true };
                window.ShowInTaskbar = false;
                var dialogResult = watchView.ShowDialog();
                window.ShowInTaskbar = true;
                if (dialogResult != true) 
                    return;
                var watchBinding = new WatchBinding(watchView.Value);
                watchBinding.InitializeBinding();
                var statePropertyName = PropertyHelper<Watch>.GetProperty(x => x.State).Name;
                watchView.Value.InvokeAllPropertyFunctions(str => str == statePropertyName, true);
                Watches.Add(watchView.Value);
                Uow.Commit();
            }, e => true);

            EditWatch = new RelayCommand(e => {
                var window = (MainView)e;
                var selectedItems = window.WatchesDataGrid.SelectedItems;
                var selectedItem = (Watch)selectedItems[0];
                var backupKey = Guid.NewGuid().ToString();
                selectedItem.BackupProperties(backupKey);
                var watchView = new WatchView(selectedItem);
                window.ShowInTaskbar = false;
                var dialogResult = watchView.ShowDialog();
                window.ShowInTaskbar = true;
                if (dialogResult == true)
                {
                    selectedItem.State = WatchState.Enabled;
                    Uow.Commit();
                }
                else
                {
                    selectedItem.RestoreProperties(backupKey);
                    selectedItem.InvokeAllPropertiesChanged();
                }
                
                selectedItem.RemoveBackup(backupKey);
            }, e => ((MainView)e).WatchesDataGrid.SelectedItems != null 
                 && ((MainView)e).WatchesDataGrid.SelectedItems.Count == 1);

            RemoveWatches = new RelayCommand(selectedItemsObj => {
                var selectedItems = (IList)selectedItemsObj;
                foreach (Watch selectedItem in selectedItems)
                {
                    selectedItem.State = WatchState.Disabled;
                    selectedItem.FileWatcher.Dispose();
                }
                RemoveItems(selectedItems, Uow.Watches);
                Uow.Commit();
            },  selectedItems => selectedItems != null && ((IList)selectedItems).Count > 0);
        }

        private static void ChangeWatchEnabledState(IList selectedItems, WatchState state)
        {
                var selectedItemsTmp = new Watch[selectedItems.Count];
                selectedItems.CopyTo(selectedItemsTmp, 0);
                while (selectedItems.Count > 0)
                {
                    var currItem =(Watch)selectedItems[0];
                    selectedItems.Remove(currItem);
                    currItem.State = state;
                }
                foreach (var watch in selectedItemsTmp)
                    selectedItems.Add(watch);
        }

        private static void RemoveItems<T>(IList selectedItems, ICollection<T> data) where T : class
        {
            while (selectedItems.Count > 0) 
            {
                var currItem =(T)selectedItems[0];
                selectedItems.Remove(currItem);
                data.Remove(currItem);
            }
        }

        private void InitializeInfoCommands()
        {
            VisitWebsite = new RelayCommand(() => 
                ViewHelper.OpenBrowser("http://www.bajak.net"), () => true);
        }

        private void InitializeOptionCommands()
        {
            SelectOutputPath = new RelayCommand(() =>
            {
                var folderPath = ViewHelper.GetSaveFilePath(
                    ViewConfig.OPTIONS_DEFAULT_OUTPUTPATH_EXTENSION,
                    ViewConfig.OPTIONS_DEFAULT_OUTPUTPATH_FILTER,
                    ViewConfig.OPTIONS_DEFAULT_OUTPUTPATH_INITIALPATH);
                Options.OutputPath =
                    String.IsNullOrEmpty(folderPath) ? Options.OutputPath : folderPath;
            }, () => true);
        }

        private void InitializeOutputText()
        {
            Options.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == PropertyHelper<Options>.GetProperty(x => x.OutputPath).Name
                &&  Options.Errors.Count <= 0)
                {
                    Properties.Settings.Default.SAVEOUTPUTLOCATION = Options.OutputPath;
                    Properties.Settings.Default.Save();
                }
            };

            Histories.PropertyChanged += (s, e) => {
                if (e.PropertyName ==  PropertyHelper<History>.GetProperty(x => x.LastOutputText).Name)
                {
                    Options.InvokeAllPropertyActions();
                    var history = (History)s;
                    var currText = history.EndDate.ToShortTimeString() + " " + history.Name + ": " + history.LastOutputText + "\r\n";
                    OutputText += currText;
                    if (Options.Errors.Count <= 0 && SaveOutput)
                        ViewHelper.WriteToFile(Options.OutputPath, currText);
                }
                if (e.PropertyName == PropertyHelper<History>.GetProperty(x => x.LastErrorText).Name)
                {
                    Options.InvokeAllPropertyActions();
                    var history = (History)s;
                    var currText = history.EndDate.ToShortTimeString() + " " + history.Name + ": " + history.LastErrorText + "\r\n";
                    OutputText += currText;
                    if (Options.Errors.Count <= 0 && SaveOutput)
                        ViewHelper.WriteToFile(Options.OutputPath, currText);
                }
            };
        }

        private void InitializeBalloons()
        {
            Histories.CollectionChanged += (s, e) => {
                if (!PopupsVisible)
                    return;
                if (e.Action != NotifyCollectionChangedAction.Add)
                    return;
                var newItem = (History)e.NewItems[0];
                var target = Path.GetFileName(newItem.TargetPath);
                var dir = new DirectoryInfo(Path.GetDirectoryName(newItem.TargetPath)).Name;
                BalloonView.ShowBalloon(newItem.Name, dir + @"\" + target, 4000);
            };
        }

        private void InitializeLogic()
        {
            OptionsValidation.ValidateOptions(Options);
            WatchValidation.ValidateWatches(Watches);
            WatchBinding.InitializeWatches(Watches);
            var stateMemberName = PropertyHelper<Watch>.GetProperty(x => x.State).Name;
            foreach (var watch in Watches)
                watch.InvokeAllPropertyFunctions(str => str == stateMemberName, true);
            var historyManager = new HistoryBinding(_dispatcher);
            historyManager.CommitRequest += (sender, e) => Uow.Commit();
            historyManager.Initialize(Watches, Histories);
        }
    }
}
