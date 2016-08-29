using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PR2PS.DataAccess.Entities;

namespace PR2PS.DataAccess.Engine
{
    public class QueryEngine : IQueryEngine
    {
        private DatabaseContext dbContext;

        public QueryEngine(DatabaseContext dbContext)
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
