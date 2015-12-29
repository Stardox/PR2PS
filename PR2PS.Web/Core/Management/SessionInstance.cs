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
        public Int32 LoginId { get; private set; }
        public AccountDataDTO AccounData { get; private set; }
        public String Server { get; private set; }
        public String IP { get; private set; }
        public Int32? Port { get; private set; }

        public SessionInstance(Guid token, Int32 loginId, AccountDataDTO accountData, String server, String ip, Int32? port)
        {
            this.Token = token;
            this.AccounData = accountData;
            this.LoginId = loginId;
            this.Server = server;
            this.IP = ip;
            this.Port = port;
        }
    }
}
