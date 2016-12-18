using Microsoft.AspNet.SignalR;
using PR2PS.Common.DTO;
using System;

namespace PR2PS.Web.Core.SignalR
{
    /// <summary>
    /// Class providing set of methods that are used to call remote SignalR methods on connected clients.
    /// Used for communication with remote game servers.
    /// </summary>
    public class HubCtxProvider
    {
        #region Singleton

        private static readonly Lazy<HubCtxProvider> instance = new Lazy<HubCtxProvider>(
            () => new HubCtxProvider(GlobalHost.ConnectionManager.GetHubContext<SignalRHub>()));

        /// <summary>
        /// Gets instance of this class.
        /// </summary>
        public static HubCtxProvider Instance { get { return instance.Value; } }

        private HubCtxProvider(IHubContext hubContext)
        {
            this.hubContext = hubContext;
        } 

        #endregion

        /// <summary>
        /// SignalR hub context reference.
        /// </summary>
        private IHubContext hubContext;

        /// <summary>
        /// Notifies remote game server that user has been successfully authenticated, authorized and has now a valid session.
        /// </summary>
        /// <param name="serverId">SignalR connection id uniquely representing remote game server.</param>
        /// <param name="loginId">Login id of user who successfully logged in.</param>
        /// <param name="userData">User profile details.</param>
        public void LoginSuccessful(String serverId, Int32 loginId, AccountDataDTO userData)
        {
            this.hubContext.Clients.Client(serverId).LoginSuccessful(loginId, userData);
        }

        /// <summary>
        /// Instructs remote game server to invalidate session of specific user.
        /// </summary>
        /// <param name="serverId">SignalR connection id uniquely representing remote game server.</param>
        /// <param name="userId">User's unique id.</param>
        /// <param name="ipAddress">
        /// User's login IP address.
        /// If specified, all users logged in from this IP will have their sessions invalidated.
        /// </param>
        public void ForceLogout(String serverId, Int64 userId, String ipAddress)
        {
            this.hubContext.Clients.Client(serverId).ForceLogout(userId, ipAddress);
        }

        /// <summary>
        /// Instructs all game servers to invalidate session of specific user.
        /// </summary>
        /// <param name="userId">User's unique id.</param>
        /// <param name="ipAddress">
        /// User's login IP address.
        /// If specified, all users logged in from this IP will have their sessions invalidated.
        /// </param>
        public void ForceLogout(Int64 userId, String ipAddress)
        {
            this.hubContext.Clients.All.ForceLogout(userId, ipAddress);
        }
    }
}
