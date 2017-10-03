using PR2PS.DataAccess.Entities;
using System;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PR2PS.DataAccess.LevelsDataAccess
{
    public class LevelsContext : DbContext
    {
        public DbSet<Level> Levels { get; set; }
        public DbSet<LevelVersion> LevelVersions { get; set; }

        public LevelsContext(String connectionString) : base(connectionString) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            Database.SetInitializer(new LevelsDatabaseInitializer(modelBuilder));
        }
    }
}
