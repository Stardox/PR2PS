using PR2PS.DataAccess.Entities;
using System;

namespace PR2PS.DataAccess.Core
{
    public class DataAccessEngine : IDataAccessEngine
    {
        private DatabaseContext dbContext;

        public DataAccessEngine(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Account FindAccountById(Int64 accountId)
        {
            throw new NotImplementedException();
        }

        public Account FindAccountByUsername(String username)
        {
            throw new NotImplementedException();
        }

        public Ban FindBanByAccountIdAndIP(Int64 accountId, String ipAddress)
        {
            throw new NotImplementedException();
        }
    }
}
