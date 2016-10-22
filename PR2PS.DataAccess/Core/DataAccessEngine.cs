using PR2PS.Common.Constants;
using PR2PS.Common.Exceptions;
using PR2PS.Common.Extensions;
using PR2PS.DataAccess.Entities;
using System;
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

        public Account GetAccountById(Int64 id)
        {
            return this.dbContext.Accounts.FirstOrDefault(a => a.Id == id);
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
            else if (this.dbContext.Accounts.Any(a => a.Username.ToUpper() == username.ToUpper()))
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

            Account acc = this.dbContext.Accounts.FirstOrDefault(a => a.Username.ToUpper() == username.ToUpper());
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

        public void UpdateAccountStatus(Int64 id, String status, String ipAddress)
        {
            this.UpdateAccountStatus(this.GetAccountById(id), status, ipAddress);
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

        public void ChangePassword(Int64 id, String oldPassword, String newPassword)
        {
            Account acc = this.GetAccountById(id);
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
    }
}
