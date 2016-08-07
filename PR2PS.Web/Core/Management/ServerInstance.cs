using System;

namespace PR2PS.Web.Core.Management
{
    public class ServerInstance
    {
        public Int32 Server_id { get; set; }
        public String Server_name { get; set; }
        public String Address { get; set; }
        public String Port { get; set; }
        public Int32 Population { get; set; }
        public String Status { get; set; }
        public String Guild_id { get; set; }
        public String Tournament { get; set; }
        public String Happy_hour { get; set; }
        public String SignalRClientId { get; set; }

        private DateTime lastCheck;

        public ServerInstance()
        {
            this.lastCheck = DateTime.UtcNow;
        }

        public Boolean IsAlive()
        {
            if ((DateTime.UtcNow - this.lastCheck).TotalMilliseconds > WebConstants.SERVER_KEEPALIVE_INTERVAL_MILLIS)
            {
                return false;
            }

            return true;
        }

        public void UpdateAlive()
        {
            this.lastCheck = DateTime.UtcNow;
        }
    }
}
