using PR2PS.DataAccess.Entities;
using System;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace PR2PS.DataAccess.LevelsDataAccess
{
    public class LevelsContext : DbContext
    {
        public DbSet<Level> Levels { get; set; }
        public DbSet<LevelVersion> LevelVersions { get; set; }
        public DbSet<LevelPlay> LevelPlays { get; set; }
        public DbSet<LevelVote> LevelVotes { get; set; }

        public LevelsContext(String connectionString) : base(connectionString) { }

        public LevelsContext(DbConnection connection) : base(connection, true) { }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            Database.SetInitializer(new LevelsDatabaseInitializer(modelBuilder));
        }
    }
}
