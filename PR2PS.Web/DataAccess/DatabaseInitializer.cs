using PR2PS.Web.Core;
using PR2PS.Web.DataAccess.Entities;
using SQLite.CodeFirst;
using System;
using System.Data.Entity;
using System.Web.Helpers;

namespace PR2PS.Web.DataAccess
{
    public class DatabaseInitializer : SqliteCreateDatabaseIfNotExists<DatabaseContext>
    {
        public DatabaseInitializer(DbModelBuilder modelBuilder) : base(modelBuilder) { }

        protected override void Seed(DatabaseContext context)
        {
            Console.WriteLine("Creating and seeding the database...");

            Account adminAcc = new Account()
            {
                Username = "Admin",
                PasswordHash = Crypto.HashPassword(""),
                Group = 3,
                CustomizeInfo = new CustomizeInfo()
                {
                    HatSeq = Constants.PARTS_ALL_HATS,
                    HeadSeq = Constants.PARTS_ALL_HEADS,
                    BodySeq = Constants.PARTS_ALL_BODIES,
                    FeetSeq = Constants.PARTS_ALL_FEET,
                    HatSeqEpic = Constants.PARTS_ALL_HATS,
                    HeadSeqEpic = Constants.PARTS_ALL_HEADS,
                    BodySeqEpic = Constants.PARTS_ALL_BODIES,
                    FeetSeqEpic = Constants.PARTS_ALL_FEET,
                    ObtainedRankTokens = 150
                },
                Experience = new Experience()
            };

            context.Accounts.Add(adminAcc);
        }
    }
}
