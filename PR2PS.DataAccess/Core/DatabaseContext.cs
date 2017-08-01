using PR2PS.DataAccess.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PR2PS.DataAccess.Core
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<CustomizeInfo> CustomizeInfos { get; set; }
        public DbSet<Experience> Experience { get; set; }
        public DbSet<Ban> Bans { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DatabaseContext(String connectionString) : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            Database.SetInitializer<DatabaseContext>(new DatabaseInitializer(modelBuilder));
        }
    }
}
