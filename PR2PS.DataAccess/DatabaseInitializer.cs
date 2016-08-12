using PR2PS.Common.Constants;
using PR2PS.DataAccess.Entities;
using SQLite.CodeFirst;
using System;
using System.Data.Entity;
using System.Web.Helpers;

namespace PR2PS.DataAccess
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
                PasswordHash = Crypto.HashPassword(""), // TODO - Find an alternative to this to get rid of the dependency on System.Web.
                Group = 3,
                CustomizeInfo = new CustomizeInfo()
                {
                    HatSeq = BodyParts.PARTS_ALL_HATS,
                    HeadSeq = BodyParts.PARTS_ALL_HEADS,
                    BodySeq = BodyParts.PARTS_ALL_BODIES,
                    FeetSeq = BodyParts.PARTS_ALL_FEET,
                    HatSeqEpic = BodyParts.PARTS_ALL_HATS,
                    HeadSeqEpic = BodyParts.PARTS_ALL_HEADS,
                    BodySeqEpic = BodyParts.PARTS_ALL_BODIES,
                    FeetSeqEpic = BodyParts.PARTS_ALL_FEET,
                    ObtainedRankTokens = 150
                },
                Experience = new Experience()
            };

            context.Accounts.Add(adminAcc);
        }
    }
}
