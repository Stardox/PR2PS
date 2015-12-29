using PR2PS.Web.DataAccess.Entities;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PR2PS.Web.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Ban> Bans { get; set; }

        public DatabaseContext() : base("PR2Context") { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            Database.SetInitializer<DatabaseContext>(new DatabaseInitializer(modelBuilder));
        }
    }
}
