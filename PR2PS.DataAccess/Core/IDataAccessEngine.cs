using PR2PS.DataAccess.Entities;
using System;

namespace PR2PS.DataAccess.Core
{
    public interface IDataAccessEngine
    {
        /// <summary>
        /// Checks if account data is valid and if so then creates new user Account.
        /// </summary>
        /// <param name="username">Account username.</param>
        /// <param name="password">Account password.</param>
        /// <param name="email">Account email address.</param>
        /// <param name="ipAddress">IPv4 address from which the request originates.</param>
        void RegisterUser(String username, String password, String email, String ipAddress);

        Account FindAccountById(Int64 accountId);
        Account FindAccountByUsername(String username);
        Ban FindBanByAccountIdAndIP(Int64 accountId, String ipAddress);
    }
}
