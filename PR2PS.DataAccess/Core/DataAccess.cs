using PR2PS.DataAccess.Entities;
using System;

namespace PR2PS.DataAccess.Core
{
    public class DataAccess : IDataAccess
    {
        private DatabaseContext dbContext;

        public DataAccess(DatabaseContext dbContext)
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
