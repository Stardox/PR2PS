using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR2PS.Web.Core.Management
{
    public sealed class SessionManager
    {
        #region Singleton

        private static readonly SessionManager instance = new SessionManager();

        public static SessionManager Instance
        {
            get { return instance; }
        }

        static SessionManager() { }

        private SessionManager()
        {
            this.sessions = new List<SessionInstance>();
            this.sessionsLock = new Object();
        }

        #endregion

        private List<SessionInstance> sessions;
        private readonly Object sessionsLock;

        public SessionInstance GetSessionByUsername(String username)
        {
            lock (this.sessionsLock)
            {
                return this.sessions.FirstOrDefault(s => s.AccounData != null && s.AccounData.Username.ToUpper() == username.ToUpper());
            }
        }

        public SessionInstance GetSessionByToken(String token)
        {
            Guid parsedToken;
            if (!Guid.TryParse(token, out parsedToken)) return null;

            lock (this.sessionsLock)
            {
                return this.sessions.FirstOrDefault(s => s.Token == parsedToken);
            }
        }

        public SessionInstance GetSessionByLoginId(String serverName, Int32 loginId)
        {
            lock (this.sessionsLock)
            {
                return this.sessions.FirstOrDefault(s => s.Server == serverName && s.LoginId == loginId);
            }
        }

        public void StoreSession(SessionInstance session)
        {
            lock (this.sessionsLock)
            {
                this.sessions.Add(session);
            }
        }

        public void RemoveSession(SessionInstance session)
        {
            lock (this.sessionsLock)
            {
                this.sessions.Remove(session);
            }
        }

        public Int32 RemoveExpiredSessions()
        {
            lock (this.sessionsLock)
            {
                return this.sessions.RemoveAll(s => s.IsExpired);
            }
        }
    }
}
