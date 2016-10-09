using PR2PS.Common.Constants;
using PR2PS.Common.Exceptions;
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
