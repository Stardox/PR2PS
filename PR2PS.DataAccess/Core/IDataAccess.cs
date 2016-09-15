using PR2PS.DataAccess.Entities;
using System;

namespace PR2PS.DataAccess.Core
{
    public interface IDataAccess
    {
        Account FindAccountById(Int64 accountId);
        Account FindAccountByUsername(String username);
        Ban FindBanByAccountIdAndIP(Int64 accountId, String ipAddress);
    }
}
