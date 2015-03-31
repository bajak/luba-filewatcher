using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Filewatcher
{
    public interface IObservableCollection<T> : ICollection<T>, INotifyCollectionChanged, INotifyPropertyChanged  where T : class {}
}
