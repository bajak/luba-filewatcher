using System;
using System.Collections.Generic;
using System.IO;
using Filewatcher.MDL;

namespace Filewatcher.BLL
{
    public class WatchBinding
    {
        private readonly Watch _watch;
        private readonly string _bindingKey;

        public WatchBinding(Watch watch)
        {
            _watch = watch;
            _bindingKey = GetType().Name;
            InitializeState();
        }

        public void InitializeBinding()
        {
            SetWatchFolderBinding();
            SetExtensionBinding();
            SetStateBinding();
            SetSubfoldersBinding();
            SetFilterBinding();
        }

        private void SetWatchFolderBinding()
        {
            var propertyName = PropertyHelper<Watch>.GetProperty(x => x.WatchFolder).Name;
            if (_watch.ContainsPropertyAction(_bindingKey, propertyName))
                return;
            _watch.AddPropertyAction(_bindingKey, value => 
            {
                if (_watch[propertyName] != null)
                    return;
                //if (_watch.WatchFolderHandle != IntPtr.Zero)
                //    NativeMethods.CloseHandle(_watch.WatchFolderHandle);

                //_watch.WatchFolderHandle = NativeMethods.OpenFolder(value.ToString());
                _watch.FileWatcher.Path = value.ToString();
            }, propertyName);
        }

        private void SetExtensionBinding()
        {
            var propertyName = PropertyHelper<Watch>.GetProperty(x => x.IncludeExtension).Name;
            if (_watch.ContainsPropertyAction(_bindingKey, propertyName))
                return;
            _watch.AddPropertyAction(_bindingKey, value =>
            {
                if (_watch[propertyName] != null)
                    return;
                _watch.FileWatcher.Filter = value.ToString();
            }, propertyName);
        }

        private void SetStateBinding()
        {
            var propertyName = PropertyHelper<Watch>.GetProperty(x => x.State).Name;
            if (_watch.ContainsPropertyAction(_bindingKey, propertyName))
                return;
            _watch.AddPropertyAction(_bindingKey, value =>
            {
                if (_watch.Errors.Count > 0)
                    return;
                _watch.FileWatcher.EnableRaisingEvents = 
                    ((WatchState)value) == WatchState.Enabled;
            }, propertyName);
        }


        private void SetSubfoldersBinding()
        {
            var propertyName = PropertyHelper<Watch>.GetProperty(x => x.IncludeSubfolders).Name;
            if (_watch.ContainsPropertyAction(_bindingKey, propertyName))
                return;
            _watch.AddPropertyAction(_bindingKey, value =>
            {
                if (_watch[propertyName] != null)
                    return;
                _watch.FileWatcher.IncludeSubdirectories = (bool)value;
            }, propertyName);
        }

        private void SetFilterBinding()
        {
            var propertyName = PropertyHelper<Watch>.GetProperty(x => x.Filter).Name;
            if (_watch.ContainsPropertyAction(_bindingKey, propertyName))
                return;
            _watch.AddPropertyAction(_bindingKey, value =>
            {
                if (_watch[propertyName] != null)
                    return;
                _watch.FileWatcher.NotifyFilter = (NotifyFilters)value;
            }, propertyName);
        }

        private void InitializeState()
        {
            if (_watch.State == WatchState.Error)
                _watch.State = WatchState.Disabled;
            _watch.Errors.CollectionChanged += (sender, e) =>
            {
                _watch.State = _watch.Errors.Count > 0 ?
                    WatchState.Error : WatchState.Disabled;
            };
        }

        public static void InitializeWatches(IEnumerable<Watch> watches)
        {
            foreach (var watch in watches)
            {
                var watchBinding = new WatchBinding(watch);
                watchBinding.InitializeBinding();
            }
        }
    }
}
