using PR2PS.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace PR2PS.DataAccess.Core
{
    public interface IDataAccessEngine
    {
        /// <summary>
        /// Get user's account by its unique id.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <returns>Found account or null.</returns>
        Account GetAccountById(Int64 id);

        /// <summary>
        /// Checks if account data is valid and if so then creates new user Account.
        /// </summary>
        /// <param name="username">Account username.</param>
        /// <param name="password">Account password.</param>
        /// <param name="email">Account email address.</param>
        /// <param name="ipAddress">IPv4 address from which the request originates.</param>
        void RegisterUser(String username, String password, String email, String ipAddress);

        /// <summary>
        /// Authenticate user according to credentials and performs check, whether user is banned.
        /// </summary>
        /// <param name="username">Account username.</param>
        /// <param name="password">Account password.</param>
        /// <param name="ipAddress">IPv4 address from which the request originates.</param>
        /// <returns>Account instance on success, exception otherwise.</returns>
        Account AuthenticateUser(String username, String password, String ipAddress);

        /// <summary>
        /// Updates user's login status, login date and login ip address.
        /// </summary>
        /// <param name="userId">Id of user Account which should be updated.</param>
        /// <param name="status">Account status text. Example: Playing on Derron.</param>
        /// <param name="ipAddress">IPv4 address from which the request originates.</param>
        void UpdateAccountStatus(Int64 userId, String status, String ipAddress);

        /// <summary>
        /// Updates user's login status, login date and login ip address.
        /// </summary>
        /// <param name="account">In memory persited user Account record which should be updated.</param>
        /// <param name="status">Account status text. Example: Playing on Derron.</param>
        /// <param name="ipAddress">IPv4 address from which the request originates.</param>
        void UpdateAccountStatus(Account account, String status, String ipAddress);

        /// <summary>
        /// Changes user's password.
        /// </summary>
        /// <param name="userId">User's unique id.</param>
        /// <param name="oldPassword">Old password for verification.</param>
        /// <param name="newPassword">New password.</param>
        void ChangePassword(Int64 userId, String oldPassword, String newPassword);

        /// <summary>
        /// Gets all private messages associated with user.
        /// </summary>
        /// <param name="userId">User's unique id.</param>
        /// <param name="start">Number of messages (relative to first) which should be skipped.</param>
        /// <param name="count">Number of messages to get.</param>
        /// <returns></returns>
        IEnumerable<Message> GetMessages(Int64 userId, Int32? start, Int32? count);
    }
}
