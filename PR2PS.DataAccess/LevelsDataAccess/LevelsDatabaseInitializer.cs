using PR2PS.DataAccess.Entities;
using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;

namespace PR2PS.DataAccess.LevelsDataAccess
{
    public class LevelsDatabaseInitializer : SqliteCreateDatabaseIfNotExists<LevelsContext>
    {
        public LevelsDatabaseInitializer(DbModelBuilder modelBuilder) : base(modelBuilder) { }

        protected override void Seed(LevelsContext context)
        {
            Console.WriteLine("Attempting to create and seed the levels database...");

            try
            {
                this.ImportMaps(context);
            }
            catch (Exception ex)
            {
                Console.WriteLine(String.Concat("Importing of levels failed with following error:\n", ex.ToString()));
            }
        }

        private void ImportMaps(LevelsContext context)
        {
            String path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Files", "Import");
            if (!Directory.Exists(path) || File.Exists(path))
            {
                Console.WriteLine("Import directory not found, skipping...");
                return;
            }

            Console.WriteLine("Scanning Import directory for files...");

            IEnumerable<String> files = Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories);
            if (!files.Any())
            {
                Console.WriteLine("No files found, skipping...");
                return;
            }

            Int32 successCount = 0;
            Int32 failCount = 0;

            foreach (String file in files)
            {
                Console.WriteLine(String.Concat("Importing ", file));

                try
                {
                    String rawData = File.ReadAllText(file);
                    Level level = Level.FromString(rawData, 1);

                    if (level == null)
                    {
                        failCount++;
                        Console.WriteLine(String.Concat("Failed to parse level ", file));
                        continue;
                    }

                    context.Levels.Add(level);

                    successCount++;
                    Console.WriteLine(String.Concat("Level ", file, " prepared."));
                }
                catch (Exception ex)
                {
                    failCount++;
                    Console.WriteLine(String.Concat("Import of ", file, " failed with following error:\n", ex.Message));
                }
            }

            Console.WriteLine("Attempting to save imported levels to database...");

            context.SaveChanges();

            Console.WriteLine("Import completed. Successfully imported {0} levels. Failed to import {1} levels.", successCount, failCount);
        }
    }
}
