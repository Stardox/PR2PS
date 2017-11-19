using PR2PS.DataAccess.LevelsDataAccess;
using PR2PS.DataAccess.MainDataAccess;
using PR2PS.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PR2PS.DataAccess.Entities;
using PR2PS.LevelImporter.Models;

namespace PR2PS.LevelImporter.Core
{
    public enum AttachType
    {
        Main,
        Levels
    }

    public enum UserSearchMode
    {
        UserName,
        Id
    }

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

        private void ValidateDbSchema(IDbConnection connection, IDictionary<String, List<String>> schemaMetaData)
        {
            connection.Open();

            foreach (String table in schemaMetaData.Keys)
            {
                Boolean tableExists = TableExists(connection, table);
                if (!tableExists)
                {
                    throw new DbValidationException(String.Format("Database does not contain table '{0}'.", table));
                }

                foreach (String column in schemaMetaData[table])
                {
                    Boolean columnExists = ColumnExists(connection, table, column);
                    if (!columnExists)
                    {
                        throw new DbValidationException(String.Format("Table '{0}' does not contain column '{1}'.", table, column));
                    }
                }
            }

            connection.Close();
        }

        public Boolean TableExists(IDbConnection connection, String tableName)
        {
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = String.Format("SELECT COUNT(*) FROM sqlite_master WHERE type = 'table' AND name = '{0}'", tableName);
                Int64 result = (Int64)cmd.ExecuteScalar();

                return result == 1;
            }
        }

        private Boolean ColumnExists(IDbConnection connection, String tableName, String columnName)
        {
            using (IDbCommand cmd = connection.CreateCommand())
            {
                cmd.CommandText = string.Format("PRAGMA table_info({0})", tableName);
                IDataReader reader = cmd.ExecuteReader();
                Int32 nameIndex = reader.GetOrdinal("Name");

                while (reader.Read())
                {
                    if (String.CompareOrdinal(reader.GetString(nameIndex), columnName) == 0)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        #endregion

        public void Attach(String path, AttachType type)
        {
            if (type == AttachType.Main)
            {
                try
                {
                    this.mainCtx = new MainContext(this.BuildConnection(path));
                    this.ValidateDbSchema(this.mainCtx.Database.Connection, this.mainDbSchema);
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
                    this.ValidateDbSchema(this.levelsCtx.Database.Connection, this.levelDbSchema);
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

    public class DbValidationException : Exception
    {
        public DbValidationException(String message) : base(message) { }
    }
}
