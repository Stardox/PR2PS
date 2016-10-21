using PR2PS.Common.Constants;
using PR2PS.Web.Core.JsonModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PR2PS.Web.Core.Management
{
    public sealed class ServerManager
    {
        #region Singleton

        private static readonly ServerManager instance = new ServerManager();

        public static ServerManager Instance
        {
            get { return instance; }
        }

        static ServerManager() { }

        private ServerManager()
        {
            this.servers = new Dictionary<String, ServerInstance>();
            this.serversLock = new Object();
            this.currentId = 0;
        }

        #endregion

        private Dictionary<String, ServerInstance> servers;
        private readonly Object serversLock;
        private Int32 currentId;

        public Int32 ServerCount
        {
            get
            { 
                lock (this.serversLock)
                {
                    return this.servers.Count;
                }
            }
        }

        public Boolean AddServer(String name, String address, String port, String signalRClientId)
        {
            lock (this.serversLock)
            {
                if (this.servers.ContainsKey(name))
                {
                    return false;
                }

                this.servers.Add(name, new ServerInstance()
                {
                    Server_id = this.currentId++,
                    Server_name = name,
                    Address = address,
                    Port = port,
                    Population = 0,
                    Status = StatusMessages.SERVER_OPEN,
                    Guild_id = "0", // TODO.
                    Happy_hour = "0", // TODO.
                    Tournament = "0", // TODO.
                    SignalRClientId = signalRClientId
                });

                return true;
            }
        }

        public Boolean RemoveServer(String name)
        {
            lock (this.serversLock)
            {
                if (!this.servers.ContainsKey(name))
                {
                    return false;
                }

                this.servers.Remove(name);

                return true;
            }
        }

        public ServerInstance GetServer(String name)
        {
            lock (this.serversLock)
            {
                ServerInstance foundServer = null;
                this.servers.TryGetValue(name, out foundServer);
                return foundServer;
            }
        }

        public List<ServerJson> GetServers()
        {
            lock (this.serversLock)
            {
                return this.servers.Values
                    .Select(serv =>
                        new ServerJson()
                        {
                            Address = serv.Address,
                            GuildId = serv.Guild_id,
                            HappyHour = serv.Happy_hour,
                            Population = serv.Population,
                            Port = serv.Port,
                            ServerId = serv.Server_id,
                            Server_name = serv.Server_name,
                            Status = serv.Status,
                            Tournament = serv.Tournament
                        })
                    .ToList<ServerJson>();
            }
        }

        public List<String> RemoveDeadServers()
        {
            lock (this.serversLock)
            {
                // TODO - LINQ.
                List<String> toRemove = new List<String>();
                foreach (KeyValuePair<String, ServerInstance> pair in this.servers)
                {
                    if (!pair.Value.IsAlive())
                    {
                        toRemove.Add(pair.Key);
                    }
                }

                foreach (String key in toRemove)
                {
                    this.servers.Remove(key);
                }

                return toRemove;
            }
        }

        public void UpdateKeepAliveTime(String name)
        {
            lock (this.serversLock)
            {
                ServerInstance server = null;
                if (this.servers.TryGetValue(name, out server))
                {
                    server.UpdateAlive();
                }
            }
        }
    }
}
