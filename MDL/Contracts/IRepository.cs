using System.Data.Entity;
using System.Linq;

namespace Filewatcher.MDL
{
    public interface IRepository<T> where T : class, IEntity
    {
        DbContext DbContext { get; set; }
        DbSet<T> DbSet { get; set; }
        IQueryable<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(int id);
    }
}
