using PR2PS.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR2PS.Web.Core.Management
{
    /// <summary>
    /// Class representing unique session.
    /// </summary>
    public class SessionInstance
    {
        public Guid Token { get; private set; }
        public Int32 LoginId { get; set; }
        public AccountDataDTO AccounData { get; set; }
        public String Server { get; set; }
        public String IP { get; set; }
        public Int32? Port { get; set; }
        public DateTime ValidUntil { get; private set; }

        public Boolean IsExpired { get { return DateTime.Compare(DateTime.UtcNow, ValidUntil) > 0; } }

        public SessionInstance(Guid token, Int32 loginId, AccountDataDTO accountData, String server, String ip, Int32? port)
        {
            this.Token = token;
            this.AccounData = accountData;
            this.LoginId = loginId;
            this.Server = server;
            this.IP = ip;
            this.Port = port;
            this.ValidUntil = DateTime.UtcNow.AddMinutes(WebConstants.SESSION_EXPIRY_MINUTES);
        }

        /// <summary>
        /// Extends the session for constant interval.
        /// </summary>
        public void Extend()
        {
            this.ValidUntil = DateTime.UtcNow.AddMinutes(WebConstants.SESSION_EXPIRY_MINUTES);
        }
    }
}
