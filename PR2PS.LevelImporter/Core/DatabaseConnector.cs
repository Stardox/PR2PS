using PR2PS.DataAccess.Entities;
using PR2PS.DataAccess.LevelsDataAccess;
using PR2PS.DataAccess.MainDataAccess;
using PR2PS.LevelImporter.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using static PR2PS.LevelImporter.Core.Enums;

namespace PR2PS.LevelImporter.Core
{
    public class DatabaseConnector : IDisposable
    {
        #region Fields.

        private Dictionary<String, List<String>> mainDbSchema;
        private Dictionary<String, List<String>> levelDbSchema;

        private MainContext mainCtx;
        private LevelsContext levelsCtx;

        #endregion

        #region Properties.

        public Boolean IsMainDbAttached { get { return this.mainCtx != null; } }
        public Boolean IsLevelsDbAttached { get { return this.levelsCtx != null; } }

        #endregion

        #region Constructor.

        public DatabaseConnector()
        {
            // .NET Reflection please.

            this.mainDbSchema = new Dictionary<String, List<String>>
            {
                { "Account", new List<String> { "Id", "Username", "PasswordHash", "Email", "Group", "RegisterDate", "RegisterIP", "LoginDate", "LoginIP", "Status", "CustomizeInfoId", "ExperienceId" } }
            };

            this.levelDbSchema = new Dictionary<String, List<String>>
            {
                { "Level", new List<String> { "Id", "AuthorId", "Title", "IsDeleted", "IsPublished" } },
                { "LevelVersion", new List<String> { "Id", "SubmittedDate", "SubmittedIP", "Note", "GameMode", "MinRank", "CowboyChance", "Gravity", "Song", "MaxTime", "Items", "Data", "PassHash", "Level_Id" } }
            };
        }

        #endregion

        #region Database connection helpers.

        private SQLiteConnection BuildConnection(String path)
        {
            return new SQLiteConnection(new SQLiteConnectionStringBuilder { DataSource = path }.ToString());
        }

        private void ValidateDbSchema(Database database, IDictionary<String, List<String>> schemaMetaData)
        {
            foreach (String table in schemaMetaData.Keys)
            {
                Boolean tableExists = TableExists(database, table);
                if (!tableExists)
                {
                    throw new DbValidationException(String.Format("Database does not contain table '{0}'.", table));
                }

                foreach (String column in schemaMetaData[table])
                {
                    Boolean columnExists = ColumnExists(database, table, column);
                    if (!columnExists)
                    {
                        throw new DbValidationException(String.Format("Table '{0}' does not contain column '{1}'.", table, column));
                    }
                }
            }
        }

        public Boolean TableExists(Database database, String tableName)
        {
            Int64 result = database.SqlQuery<Int64>(
                String.Format("SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = '{0}'", tableName)).SingleOrDefault();

            return result == 1;
        }

        private Boolean ColumnExists(Database database, String tableName, String columnName)
        {
            Boolean exists = database.SqlQuery<TableInfo>(String.Format("PRAGMA table_info('{0}')", tableName))
                .Any(ti => String.CompareOrdinal(ti.Name, columnName) == 0);

            return exists;
        }

        #endregion

        public void Attach(String path, AttachType type)
        {
            if (type == AttachType.Main)
            {
                try
                {
                    this.mainCtx = new MainContext(this.BuildConnection(path));
                    this.ValidateDbSchema(this.mainCtx.Database, this.mainDbSchema);
                }
                catch
                {
                    if (this.mainCtx != null)
                    {
                        this.mainCtx.Dispose();
                        this.mainCtx = null;
                    }

                    throw;
                }
            }
            else if (type == AttachType.Levels)
            {
                try
                {
                    this.levelsCtx = new LevelsContext(this.BuildConnection(path));
                    this.ValidateDbSchema(this.levelsCtx.Database, this.levelDbSchema);
                }
                catch
                {
                    if (this.levelsCtx != null)
                    {
                        this.levelsCtx.Dispose();
                        this.levelsCtx = null;
                    }

                    throw;
                }
            }
        }

        public IEnumerable<UserModel> FindUsers(String term, UserSearchMode mode)
        {
            if (mode == UserSearchMode.Id)
            {
                if (Int64.TryParse(term, out Int64 id))
                {
                    return this.mainCtx.Accounts.Where(a => a.Id == id).Select(a => new UserModel { Id = a.Id, Username = a.Username }).ToList();
                }
            }
            else if (mode == UserSearchMode.UserName)
            {
                return this.mainCtx.Accounts.Where(a => a.Username != null && a.Username.ToUpper().Contains(term.ToUpper()))
                    .Select(acc => new UserModel { Id = acc.Id, Username = acc.Username }).ToList();
            }

            return Enumerable.Empty<UserModel>();
        }

        public Task<Int32> ImportLevels(IEnumerable<Level> levels)
        {
            this.levelsCtx.Levels.AddRange(levels);
            return this.levelsCtx.SaveChangesAsync();
        }

        public void Dispose()
        {
            if (this.mainCtx != null)
            {
                this.mainCtx.Dispose();
                this.mainCtx = null;
            }

            if (this.levelsCtx != null)
            {
                this.levelsCtx.Dispose();
                this.levelsCtx = null;
            }
        }
    }
}
