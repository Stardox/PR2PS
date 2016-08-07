using Microsoft.AspNet.SignalR.Client;
using PR2Hub.Core;
using System;
using System.Net;

namespace PR2PS.GameServer
{
    public static class SignalR
    {
        /// <summary>
        /// Sends basic info about this server to web server so it can be added to list of active game servers.
        /// </summary>
        /// <param name="serverName">Human readable server name.</param>
        /// <param name="address">IP address of the server.</param>
        /// <param name="port">Port to which listener is attached.</param>
        public static void RegisterServer(this IHubProxy proxy, String serverName, IPAddress address, Int32 port)
        {
            proxy.Invoke(GameConstants.RPC_REGISTER, serverName, address.ToString(), port.ToString());
        }

        /// <summary>
        /// Notifies web server that this game server is still active.
        /// </summary>
        /// <param name="serverName">Human readable server name.</param>
        public static void ServerAlive(this IHubProxy proxy, String serverName)
        {
            proxy.Invoke(GameConstants.RPC_ALIVE, serverName);
        }

        /// <summary>
        /// Sends account data to the web server which stores the progress into database.
        /// </summary>
        /// <param name="loginId">Client identifier.</param>
        /// <param name="accountData">Account data (experience, bodyparts, etc).</param>
        public static void SaveClientData(this IHubProxy proxy, AccountDataDTO accountData)
        {
            proxy.Invoke("SaveClientData", accountData);
        }

        /// <summary>
        /// Tells web server to deauthentificate client identified by loginId.
        /// </summary>
        public static void LogoutClient(this IHubProxy proxy, String server, Int32 loginId)
        {
            proxy.Invoke("LogoutClient", server, loginId);
        }

        /// <summary>
        /// Tells web server to mod (or demod) the user.
        /// Parameter action can contain 'temporary', 'trial', 'permament' or 'demod'.
        /// </summary>
        public static void ChangeModStatus(this IHubProxy proxy, String server, String issuer, String receiver, String action)
        {
            proxy.Invoke("ChangeModStatus", server, issuer, receiver, action);
        }
    }
}
