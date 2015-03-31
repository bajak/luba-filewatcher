using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.CompilerServices;
using Filewatcher.MDL;
using Filewatcher.MDL.Annotations;

namespace Filewatcher.DAL
{
    public class ObservableRepository<T> : List<T>, IObservableRepository<T> where T : class, IEntity
    {
        public ObservableRepository(DbContext dbContext)
        {
            if (dbContext == null)
                throw new ArgumentException();
            DbContext = dbContext;
            DbSet = DbContext.Set<T>();
            Initialize();
        }

        public DbContext DbContext { get; set; }
        public DbSet<T> DbSet { get; set; }

        public event NotifyCollectionChangedEventHandler CollectionChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        private void Initialize()
        {
            RegisterDbSetToList(DbSet.AsEnumerable());
            SubscribePropertiesChanged(DbSet.AsEnumerable());
        }

        private void Add(T item, bool notify)
        {
            var dbEntityEntry = DbContext.Entry(item);
            if (dbEntityEntry.State != System.Data.EntityState.Detached)
                dbEntityEntry.State = System.Data.EntityState.Added;
            else
            {
                DbSet.Add(item);
            }
            if (notify)
                OnCollectionChanged(NotifyCollectionChangedAction.Add, item);
            SubscribePropertyChanged(item);
        }

        public new void Add(T item)
        {
            base.Add(item);
            Add(item, true);
        }

        public new void Clear()
        {
            foreach (var item in DbSet)
            {
                Remove(item);
            }
        }

        public new bool Contains(T item)
        {
            return base.Contains(item);
        }

        public new void CopyTo(T[] array, int arrayIndex)
        {
            for (var i = arrayIndex; i < array.Length; i++)
            {
                Add(array[i]);
            }
            base.CopyTo(array, arrayIndex);
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        private bool Remove(T item, bool notify)
        {
            var dbEntityEntry = DbContext.Entry(item);
            if (dbEntityEntry.State != System.Data.EntityState.Deleted)
                dbEntityEntry.State = System.Data.EntityState.Deleted;
            else
            {
                DbSet.Attach(item);
                DbSet.Remove(item);
            }

            if (notify)
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, item);
            UnsubscribePropertyChanged(item);

            return true;
        }

        public new bool Remove(T item)
        {
            Remove(item, true);
            return base.Remove(item);
        }

        private void Update(T item)
        {
            var dbEntityEntry = DbContext.Entry(item);
            if (dbEntityEntry.State != System.Data.EntityState.Detached)
            {
                DbSet.Attach(item);
            }
            dbEntityEntry.State = System.Data.EntityState.Modified;
        }

        public void RegisterDbSetToList(IEnumerable items)
        {
            foreach (var item in items)
                base.Add((T)item);
        }

        private void SubscribePropertiesChanged(IEnumerable items)
        {
            if (items == null) return;
            foreach (var item in items)
            {
                SubscribePropertyChanged((T)item);
            }
        }

        private void UnsubscribePropertiesChanged(IEnumerable items)
        {
            if (items == null) return;
            foreach (var item in items)
            {
                UnsubscribePropertyChanged((T)item);
            }
        }

        private void SubscribePropertyChanged(T item)
        {
            var changed = item as INotifyPropertyChanged;
            if (changed != null)
                changed.PropertyChanged += NotifyPropertyChangedPropertyChanged;
        }

        private void UnsubscribePropertyChanged(T item)
        {
            var changed = item as INotifyPropertyChanged;
            if (changed != null)
                changed.PropertyChanged -= NotifyPropertyChangedPropertyChanged;
        }

        private void NotifyPropertyChangedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Update((T) sender);
            OnPropertyChanged(sender, e.PropertyName);
        }

        protected virtual void OnCollectionChanged(NotifyCollectionChangedAction action, T item)
        {
            var handler = CollectionChanged;
            if (handler != null) handler(this, new NotifyCollectionChangedEventArgs(action, item, IndexOf(item)));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged(object sender, [CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(sender, new PropertyChangedEventArgs(propertyName));
        }
    }
}
