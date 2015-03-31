using System.Data.Entity;

namespace Filewatcher.MDL
{
    public interface IObservableRepository<T> : IObservableCollection<T> where T : class, IEntity {
        DbContext DbContext { get; set; }
        DbSet<T> DbSet { get; set; }
    }
}
