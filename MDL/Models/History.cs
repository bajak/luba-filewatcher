using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Filewatcher.MDL
{
    public class History : ValidationBase, IEntity
    {
        public History() 
        { 
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            OutputText = new StringBuilder();
            ErrorText = new StringBuilder();
            ExitCode = -1;
            State = HistoryState.None;
        }

        public int Id { get; set; }

        public string Name
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public DateTime StartDate
        {
            get { return GetProperty<DateTime>(); }
            set { SetProperty(value); }
        }

        public DateTime EndDate
        {
            get { return GetProperty<DateTime>(); }
            set { SetProperty(value); }
        }

        public string TargetPath
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public StringBuilder OutputText
        {
            get { return GetProperty<StringBuilder>(); }
            set { SetProperty(value); }
        }

        [NotMapped]
        public string LastOutputText
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        public StringBuilder ErrorText
        {
            get { return GetProperty<StringBuilder>(); }
            set { SetProperty(value); }
        }

        [NotMapped]
        public string LastErrorText
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        [NotMapped]
        public HistoryState State
        {
            get { return GetProperty<HistoryState>(); }
            set { SetProperty(value); }
        }

        public int ExitCode
        {
            get { return GetProperty<int>(); }
            set { SetProperty(value); }
        }

        [NotMapped]
        public Process Process
        {
            get { return GetProperty<Process>(); }
            set { SetProperty(value); }
        }

        [NotMapped]
        public WatcherChangeTypes Result
        {
            get { return GetProperty<WatcherChangeTypes>(); }
            set { SetProperty(value); }
        }

        public virtual Watch Watch 
        { 
            get { return GetProperty<Watch>(); }
            set { SetProperty(value); }
        }
    }
}
