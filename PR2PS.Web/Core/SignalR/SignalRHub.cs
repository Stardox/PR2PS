using Microsoft.AspNet.SignalR;
using PR2PS.Common.Constants;
using PR2PS.Common.DTO;
using PR2PS.DataAccess.Core;
using PR2PS.DataAccess.Entities;
using PR2PS.Web.Core.Management;
using System;
using System.Linq;

namespace PR2PS.Web.Core.SignalR
{
    /// <summary>
    /// Class containing remote procedure called by TCP game server using SignalR.
    /// </summary>
    public class SignalRHub : Hub
    {
        /// <summary>
        /// Adds server to dictionary of recognized game servers.
        /// </summary>
        /// <param name="serverName">Server name (Derron, etc.).</param>
        /// <param name="address">Server host address.</param>
        /// <param name="port">Number of port.</param>
        public void RegisterServer(String serverName, String address, String port)
        {
            if (ServerManager.Instance.AddServer(serverName, address, port, this.Context.ConnectionId))
            {
                Console.WriteLine("Registered server '{0}' {1}:{2}.", serverName, address, port);
            }
            else
            {
                Console.WriteLine("Failed to register server '{0}'. It probably has been registered already.", serverName);
            }
        }

        /// <summary>
        /// Updates last check time for specified server so that it is not recycled.
        /// </summary>
        /// <param name="serverName">Server name (Derron, etc.).</param>
        public void ServerAlive(String serverName)
        {
            ServerManager.Instance.UpdateKeepAliveTime(serverName);
        }

        /// <summary>
        /// Saves user data into database. Usually called when user is logging out.
        /// </summary>
        /// <param name="accData">Account data.</param>
        public void SaveClientData(AccountDataDTO accData)
        {
            if (accData == null) return;

            using (DatabaseContext db = new DatabaseContext("PR2Context"))
            {
                Account accFromDb = db.Accounts.FirstOrDefault(acc => acc.Id == accData.UserId);

                if (accFromDb != null)
                {
                    accFromDb.CustomizeInfo.Hat = accData.Hat;
                    accFromDb.CustomizeInfo.Head = accData.Head;
                    accFromDb.CustomizeInfo.Body = accData.Body;
                    accFromDb.CustomizeInfo.Feet = accData.Feet;
                    accFromDb.CustomizeInfo.HatColor = accData.HatColor;
                    accFromDb.CustomizeInfo.HeadColor = accData.HeadColor;
                    accFromDb.CustomizeInfo.BodyColor = accData.BodyColor;
                    accFromDb.CustomizeInfo.FeetColor = accData.FeetColor;
                    accFromDb.CustomizeInfo.HatColor2 = accData.HatColor2;
                    accFromDb.CustomizeInfo.HeadColor2 = accData.HeadColor2;
                    accFromDb.CustomizeInfo.BodyColor2 = accData.BodyColor2;
                    accFromDb.CustomizeInfo.FeetColor2 = accData.FeetColor2;
                    accFromDb.CustomizeInfo.HatSeq = accData.HatSeq;
                    accFromDb.CustomizeInfo.HeadSeq = accData.HeadSeq;
                    accFromDb.CustomizeInfo.BodySeq = accData.BodySeq;
                    accFromDb.CustomizeInfo.FeetSeq = accData.FeetSeq;
                    accFromDb.CustomizeInfo.HatSeqEpic = accData.HatSeqEpic;
                    accFromDb.CustomizeInfo.HeadSeqEpic = accData.HeadSeqEpic;
                    accFromDb.CustomizeInfo.BodySeqEpic = accData.BodySeqEpic;
                    accFromDb.CustomizeInfo.FeetSeqEpic = accData.FeetSeqEpic;
                    accFromDb.CustomizeInfo.Speed = accData.Speed;
                    accFromDb.CustomizeInfo.Accel = accData.Accel;
                    accFromDb.CustomizeInfo.Jump = accData.Jump;
                    accFromDb.CustomizeInfo.Rank = accData.Rank;
                    accFromDb.CustomizeInfo.UsedRankTokens = accData.UsedRankTokens;
                    accFromDb.CustomizeInfo.ObtainedRankTokens = accData.ObtainedRankTokens;

                    db.SaveChanges();
                }

                // TODO - Log error if acc not found.
            }
        }

        /// <summary>
        /// Removes user session identified by his login id.
        /// </summary>
        /// <param name="serverName">Server name (Derron, etc.).</param>
        /// <param name="loginId">Login id identifying the session.</param>
        public void LogoutClient(String serverName, Int32 loginId)
        {
            SessionInstance session = SessionManager.Instance.GetSessionByLoginId(serverName, loginId);

            if (session != null)
            {
                SessionManager.Instance.RemoveSession(session);
                using (DatabaseContext db = new DatabaseContext("PR2Context"))
                {
                    Account accFromDb = db.Accounts.FirstOrDefault(acc => session.AccounData.UserId == acc.Id);

                    if (accFromDb != null)
                    {
                        accFromDb.Status = StatusMessages.STR_OFFLINE;
                        accFromDb.CustomizeInfo = accFromDb.CustomizeInfo;
                        accFromDb.Experience = accFromDb.Experience;
                        db.SaveChanges();
                    }
                }
            }
        }

        public void ChangeModStatus(String serverName, String issuer, String receiver, String action)
        {
            // TODO - Clean this up wtf...

            if (receiver == null)
            {
                return;
            }

            // TODO - Just update the DB and refresh the session...
            SessionInstance session = SessionManager.Instance.GetSessionByUsername(receiver);
            if (session != null)
            { 
                switch(action)
                {
                    case "permanent":
                        if(session.AccounData.Group == UserGroup.MEMBER)
                        {
                            session.AccounData.Group = UserGroup.MODERATOR;
                        }
                        break;
                    case "demod":
                        if(session.AccounData.Group == UserGroup.MODERATOR)
                        {
                            session.AccounData.Group = UserGroup.MEMBER;
                        }
                        break;
                    case "temporary":
                    case "trial":
                    default:
                        // :-(
                        break;
                }

                using (DatabaseContext db = new DatabaseContext("PR2Context"))
                {
                    Account accFromDb = db.Accounts.FirstOrDefault(acc => acc.Username.ToUpper() == receiver.ToUpper());

                    if (accFromDb == null)
                    {
                        this.sendSystemMessage(serverName, "No such user found.", issuer);
                        return;
                    }

                    switch (action)
                    {
                        case "permanent":
                            if (accFromDb.Group == UserGroup.MEMBER)
                            {
                                accFromDb.Group = UserGroup.MODERATOR;
                                this.sendSystemMessage(
                                    serverName,
                                    String.Format("User '{0}' has been promoted to moderator.", receiver),
                                    null);
                            }
                            break;
                        case "demod":
                            if (accFromDb.Group == UserGroup.MODERATOR)
                            {
                                accFromDb.Group = UserGroup.MEMBER;
                                this.sendSystemMessage(
                                    serverName,
                                    String.Format("User '{0}' has been demodded.", receiver),
                                    null);
                            }
                            break;
                        case "temporary":
                        case "trial":
                        default:
                            this.sendSystemMessage(serverName, "Sorry, not implemented yet.", issuer);
                            break;
                    }

                    db.SaveChanges();
                } 
            }
        }

        /// <summary>
        /// Helper method which sends system message to main chat room of specified game server.
        /// </summary>
        /// <param name="serverName">Name of server which will receive the message.</param>
        /// <param name="message">The message content.</param>
        /// <param name="username">If specified, then only this user will see this message.</param>
        private void sendSystemMessage(String serverName, String message, String username)
        {
            ServerInstance server = ServerManager.Instance.GetServer(serverName);
            if(server != null)
            {
                this.Clients.Client(server.SignalRClientId).SystemMessage(message, username);
            }
        }
    }
}
