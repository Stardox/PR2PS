using SQLite.CodeFirst;
using System;
using System.Data.Entity;

namespace PR2PS.DataAccess.LevelsDataAccess
{
    public class LevelsDatabaseInitializer : SqliteCreateDatabaseIfNotExists<LevelsContext>
    {
        public LevelsDatabaseInitializer(DbModelBuilder modelBuilder) : base(modelBuilder) { }

        protected override void Seed(LevelsContext context)
        {
            Console.WriteLine("Attempting to create and seed the levels database...");
        }
    }
}
