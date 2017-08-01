using PR2PS.Common.Constants;
using PR2PS.Common.Exceptions;
using PR2PS.Common.Extensions;
using PR2PS.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Helpers;

namespace PR2PS.DataAccess.Core
{
    public class DataAccessEngine : IDataAccessEngine
    {
        private DatabaseContext dbContext;

        public DataAccessEngine(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Dispose()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
                this.dbContext = null;
            }
        }

        public Account GetAccountById(Int64 id)
        {
            return this.dbContext.Accounts.FirstOrDefault(a => a.Id == id);
        }

        public Account GetAccountByUsername(String username)
        {
            username = username ?? String.Empty;

            return this.dbContext.Accounts.FirstOrDefault(a => a.Username.ToUpper() == username.ToUpper());
        }

        public void RegisterUser(String username, String password, String email, String ipAddress)
        {
            username = username ?? String.Empty;
            password = password ?? String.Empty;
            email = email ?? String.Empty;

            if (username.Length > ValidationConstraints.USERNAME_LENGTH)
            {
                throw new PR2Exception(ErrorMessages.ERR_USERNAME_TOO_LONG);
            }
            else if (password.Length > ValidationConstraints.PASSWORD_LENGTH)
            {
                throw new PR2Exception(ErrorMessages.ERR_PASSWORD_TOO_LONG);
            }
            else if (email.Length > ValidationConstraints.EMAIL_LENGTH)
            {
                throw new PR2Exception(ErrorMessages.ERR_EMAIL_TOO_LONG);
            }
            else if (!Regex.IsMatch(username, ValidationConstraints.USERNAME_PATTERN))
            {
                throw new PR2Exception(ErrorMessages.ERR_USERNAME_INVALID);
            }
            else if (!Regex.IsMatch(password, ValidationConstraints.PASSWORD_PATTERN))
            {
                throw new PR2Exception(ErrorMessages.ERR_PASSWORD_INVALID);
            }
            else if (!Regex.IsMatch(email, ValidationConstraints.EMAIL_PATTERN))
            {
                throw new PR2Exception(ErrorMessages.ERR_EMAIL_INVALID);
            }
            else if (this.GetAccountByUsername(username) != null)
            {
                throw new PR2Exception(ErrorMessages.ERR_USER_EXISTS);
            }

            Account newAcc = new Account()
            {
                Username = username,
                PasswordHash = Crypto.HashPassword(password),
                Email = email,
                RegisterIP = ipAddress,
                CustomizeInfo = new CustomizeInfo(),
                Experience = new Experience()
            };

            this.dbContext.Accounts.Add(newAcc);
            this.dbContext.SaveChanges();
        }

        public Account AuthenticateUser(String username, String password, String ipAddress)
        {
            username = username ?? String.Empty;
            password = password ?? String.Empty;

            Account acc = this.GetAccountByUsername(username);
            if (acc == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_USER_WITH_SUCH_NAME);
            }
            else if (!Crypto.VerifyHashedPassword(acc.PasswordHash, password))
            {
                throw new PR2Exception(ErrorMessages.ERR_WRONG_PASS);
            }

            DateTime utcDateTime = DateTime.UtcNow; // To prevent usage of SQL functions.
            Ban ban = this.dbContext
                          .Bans
                          .Where(b => (b.Receiver.Id == acc.Id || (b.IsIPBan && b.IPAddress == ipAddress))
                                       && DateTime.Compare(b.ExpirationDate, utcDateTime) > 0)
                          .OrderByDescending(b => b.ExpirationDate)
                          .FirstOrDefault();
            if (ban != null)
            {
                throw new PR2Exception(String.Format(
                    ErrorMessages.ERR_BANNED,
                    ban.Issuer.Username,
                    String.IsNullOrWhiteSpace(ban.Reason) ? StatusMessages.STR_NO_REASON : ban.Reason,
                    ban.Id,
                    ban.ExpirationDate.ToUniversalTime().GetPrettyBanExpirationString()));
            }

            return acc;
        }

        public void UpdateAccountStatus(Int64 userId, String status, String ipAddress)
        {
            this.UpdateAccountStatus(this.GetAccountById(userId), status, ipAddress);
        }

        public void UpdateAccountStatus(Account account, String status, String ipAddress)
        {
            if (account == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_SUCH_USER);
            }

            DateTime utcDateTime = DateTime.UtcNow; // To prevent usage of SQL functions.
            account.LoginDate = utcDateTime;
            account.LoginIP = ipAddress;
            account.Status = status;

            this.dbContext.SaveChanges();
        }

        public void ChangePassword(Int64 userId, String oldPassword, String newPassword)
        {
            Account acc = this.GetAccountById(userId);
            if (acc == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_SUCH_USER);
            }

            oldPassword = oldPassword ?? String.Empty;
            newPassword = newPassword ?? String.Empty;

            if (newPassword.Length > ValidationConstraints.PASSWORD_LENGTH)
            {
                throw new PR2Exception(ErrorMessages.ERR_PASSWORD_TOO_LONG);
            }
            else if (!Regex.IsMatch(newPassword, ValidationConstraints.PASSWORD_PATTERN))
            {
                throw new PR2Exception(ErrorMessages.ERR_PASSWORD_INVALID);
            }
            else if (!Crypto.VerifyHashedPassword(acc.PasswordHash, oldPassword))
            {
                throw new PR2Exception(ErrorMessages.ERR_WRONG_PASS);
            }

            acc.PasswordHash = Crypto.HashPassword(newPassword);

            this.dbContext.SaveChanges();
        }

        public IEnumerable<Message> GetMessages(Int64 userId, Int32? start, Int32? count)
        {
            Account acc = this.GetAccountById(userId);
            if (acc == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_SUCH_USER);
            }

            start = start ?? 0;
            count = count ?? 10;

            return acc.Messages
                      .Where(m => !m.IsDeleted)
                      .OrderByDescending(m => m.DateSent)
                      .Skip(start.Value)
                      .Take(count.Value);
        }

        public void SendMessage(Int64 senderId, String recipientUsername, String message, String ipAddress)
        {
            Account sender = this.GetAccountById(senderId);
            if (sender == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_SUCH_USER);
            }

            Account recipient = this.GetAccountByUsername(recipientUsername);
            if (recipient == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_USER_WITH_SUCH_NAME);
            }

            recipient.Messages.Add(new Message
            {
                Sender = sender,
                Recipient = recipient,
                Content = message ?? String.Empty,
                DateSent = DateTime.UtcNow.GetSecondsSinceUnixTime(),
                IPAddress = ipAddress
            });

            this.dbContext.SaveChanges();
        }

        public void DeleteMessage(Int64 userId, Int64? messageId)
        {
            Account acc = this.GetAccountById(userId);
            if (acc == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_SUCH_USER);
            }

            messageId = messageId ?? -1;

            Message message = acc.Messages.FirstOrDefault(m => m.Id == messageId);
            if (message == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_MESSAGE_NOT_FOUND);
            }

            message.IsDeleted = true;

            this.dbContext.SaveChanges();
        }

        public void DeleteAllMessages(Int64 userId)
        {
            Account acc = this.GetAccountById(userId);
            if (acc == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_SUCH_USER);
            }

            foreach (Message msg in acc.Messages)
            {
                msg.IsDeleted = true;
            }

            this.dbContext.SaveChanges();
        }

        public Account Ban(Int64 issuerId, String receiverUsername, Int32 duration, String reason, String chatLog, Boolean isIPBan)
        {
            Account issuer = this.GetAccountById(issuerId);
            if (issuer == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_SUCH_USER);
            }
            else if (issuer.Group < UserGroup.MODERATOR)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_RIGHTS);
            }

            Account receiver = this.GetAccountByUsername(receiverUsername);
            if (receiver == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_USER_WITH_SUCH_NAME);
            }
            else if (receiver.Group == UserGroup.ADMINISTRATOR)
            {
                throw new PR2Exception(ErrorMessages.ERR_ADMINS_ARE_ABSOLUTE);
            }

            receiver.Bans.Add(new Ban
            {
                Issuer = issuer,
                IPAddress = receiver.LoginIP,
                IsIPBan = isIPBan,
                StartDate = DateTime.UtcNow,
                ExpirationDate = DateTime.UtcNow.AddSeconds(duration),
                Reason = reason,
                Extra = chatLog
            });

            this.dbContext.SaveChanges();

            return receiver;
        }

        
    }
}
