using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Filewatcher.DAL;
using Filewatcher.MDL;

namespace Filewatcher
{
    internal class DatabaseContext : DbContext
    {
        static DatabaseContext()
        {
            Database.SetInitializer(new DatabaseInitializer());
        }

        public DatabaseContext() : base("DataConnection") { }
        public DbSet<Watch> Watches { get; set; }
        public DbSet<History> Histories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}
