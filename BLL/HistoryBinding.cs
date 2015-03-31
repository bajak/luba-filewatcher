using System;
using System.Collections.Specialized;
using System.IO;
using System.Text.RegularExpressions;
using Filewatcher.MDL;

namespace Filewatcher.BLL
{
    public class HistoryBinding
    {
        public HistoryBinding(IDispatcherContext dispatcher)
        {
            _dispatcher = dispatcher;
        }

        private readonly IDispatcherContext _dispatcher;
        private IObservableCollection<History> _histories;

        public event EventHandler CommitRequest;

        public void Initialize(IObservableCollection<Watch> watches,
                                IObservableCollection<History> histories)
        {
            _histories = histories;
            foreach (var watch in watches)
                BindCreateHistoryOnNotify(watch, histories);
            BindProcessOnAdd(watches, histories);
        }

        private void BindProcessOnAdd(INotifyCollectionChanged watches, 
            IObservableCollection<History> histories)
        {
            watches.CollectionChanged += (sender, e) => {
                if (e.Action != NotifyCollectionChangedAction.Add)
                    return;
                var watch = (Watch)(e.NewItems[0]);
                BindCreateHistoryOnNotify(watch, histories);
            };
        }

        public void BindCreateHistoryOnNotify(Watch watch, 
            IObservableCollection<History> histories)
        {
            FileSystemEventHandler notifyAction = (sender, e) => _dispatcher.Invoke(() => {
                watch.FileWatcher.EnableRaisingEvents = false;
                if (!String.IsNullOrEmpty(watch.ExcludeExtension))
                    if (new Regex(watch.ExcludeExtension).IsMatch(e.Name)) 
                    {
                        watch.FileWatcher.EnableRaisingEvents = true;
                        return;
                    }
                var history = new History();
                history.Name = watch.Name;
                history.TargetPath = e.FullPath;
                history.Watch = watch;
                _histories.Add(history);
                InvokeCommitRequest();
                var processManager = new ProcessInitializer(_dispatcher);
                processManager.CreateProcess(watch, history);
                watch.FileWatcher.EnableRaisingEvents = true;
            });

            watch.FileWatcher.Changed += notifyAction;
            watch.FileWatcher.Created += notifyAction;
            watch.FileWatcher.Deleted += notifyAction;
            watch.FileWatcher.Renamed += (s, e) =>
                notifyAction(s, new FileSystemEventArgs(
                    WatcherChangeTypes.Renamed, e.FullPath, e.Name));
        }

        protected virtual void InvokeCommitRequest()
        {
            var handler = CommitRequest;
            if (handler != null) handler(this, EventArgs.Empty);
        }
    }
}
