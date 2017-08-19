using PR2PS.DataAccess.Entities;
using System;
using System.Collections.Generic;

namespace PR2PS.DataAccess.MainDataAccess
{
    public interface IMainDataAccessEngine : IDisposable
    {
        /// <summary>
        /// Get user's account by its unique id.
        /// </summary>
        /// <param name="id">Account id.</param>
        /// <returns>Found account or null.</returns>
        Account GetAccountById(Int64 id);

        /// <summary>
        /// Get user's account by its username.
        /// </summary>
        /// <param name="username">Account's username.</param>
        /// <returns>Found account or null.</returns>
        Account GetAccountByUsername(String username);

        /// <summary>
        /// Checks if account data is valid and if so then creates new user Account.
        /// </summary>
        /// <param name="username">Account username.</param>
        /// <param name="password">Account password.</param>
        /// <param name="email">Account email address.</param>
        /// <param name="ipAddress">IPv4 address from which the request originates.</param>
        void RegisterUser(String username, String password, String email, String ipAddress);

        /// <summary>
        /// Authenticate user according to username and performs check, whether user is banned.
        /// Applicable when user already has a valid session.
        /// </summary>
        /// <param name="username">Account username.</param>
        /// <param name="ipAddress">IPv4 address from which the request originates.</param>
        /// <returns>Account instance on success, exception otherwise.</returns>
        Account AuthenticateUser(String username, String ipAddress);

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

        /// <summary>
        /// Sends private message to specified user.
        /// </summary>
        /// <param name="senderId">Unique id of user who sent the message.</param>
        /// <param name="recipientUsername">Username of user who shall receive the message.</param>
        /// <param name="message">Message content.</param>
        /// <param name="ipAddress">IPv4 address from which the request originates.</param>
        void SendMessage(Int64 senderId, String recipientUsername, String message, String ipAddress);

        /// <summary>
        /// Deletes message with specified id.
        /// </summary>
        /// <param name="userId">User's unique id.</param>
        /// <param name="messageId">Unique id of message to delete.</param>
        void DeleteMessage(Int64 userId, Int64? messageId);

        /// <summary>
        /// Deletes all messages received by user.
        /// </summary>
        /// <param name="userId">User's unique id.</param>
        void DeleteAllMessages(Int64 userId);

        /// <summary>
        /// Issues a ban for specified user.
        /// </summary>
        /// <param name="issuerId">Id of user who is issuing ban.</param>
        /// <param name="receiverUsername">Username of user who is receiving ban.</param>
        /// <param name="duration">Duration of ban in seconds.</param>
        /// <param name="reason">Reasoning of issued ban.</param>
        /// <param name="chatLog">Copy of chat log at the moment of ban.</param>
        /// <param name="isIPBan">Indicates if this ban is an IP ban.</param>
        /// <returns>Profile of banned user on success, otherwise exception gets thrown.</returns>
        Account Ban(Int64 issuerId, String receiverUsername, Int32 duration, String reason, String chatLog, Boolean isIPBan);

        /// <summary>
        /// Checks if user is banned.
        /// </summary>
        /// <param name="receiverId">Id of user for which the check will be performed.</param>
        /// <param name="ipAddress">IPv4 address from which the request originates.</param>
        /// <returns>Instance of Ban which will be expiring last or null if user is not banned.</returns>
        Ban IsUserBanned(Int64 receiverId, String ipAddress);
    }
}
