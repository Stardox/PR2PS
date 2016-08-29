using PR2PS.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR2PS.DataAccess.Engine
{
    public interface IQueryEngine
    {
        Account FindAccountById(Int64 accountId);
        Account FindAccountByUsername(String username);
        Ban FindBanByAccountIdAndIP(Int64 accountId, String ipAddress);
    }
}
