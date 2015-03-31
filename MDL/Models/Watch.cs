using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;

namespace Filewatcher.MDL
{
    public class Watch : ValidationBase, IEntity
    {
        public Watch()
        {
            Histories = new ObservableCollection<History>();
        }

        public int Id { get; set; }

        public string Name 
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string WatchFolder
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public IntPtr WatchFolderHandle { get; set; }

        public string Process
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public bool IsProcessHidden
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public bool LogOutput
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public string WorkingFolder
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string OutputPath
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string IncludeExtension
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string ExcludeExtension
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public string Parameter
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public WatchState State
        {
            get { return GetProperty<WatchState>(); }
            set { SetProperty(value); }
        }

        public bool UseShellExecute
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public bool IncludeSubfolders
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        public NotifyFilters Filter {
            get { return GetProperty<NotifyFilters>(); }
            set { SetProperty(value); }
        }

        [NotMapped]
        public FileSystemWatcher FileWatcher
        {
            get 
            { 
                var fileWatcher = GetProperty<FileSystemWatcher>();
                if (fileWatcher == null)
                {
                    fileWatcher = new FileSystemWatcher();
                    SetProperty(fileWatcher);
                }
                return fileWatcher;
            }
            set { SetProperty(value); }
        }

        public ObservableCollection<History> Histories 
        { 
            get { return GetProperty<ObservableCollection<History>>(); }
            set { SetProperty(value); }
        }
    }
}
