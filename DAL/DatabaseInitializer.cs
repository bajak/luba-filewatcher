using System.Data.Entity;

namespace Filewatcher.DAL
{
    internal class DatabaseInitializer : DropCreateDatabaseIfModelChanges<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        {
            context.SaveChanges();
        }
    }
}
