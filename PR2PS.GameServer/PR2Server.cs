using PR2PS.Common.Constants;
using PR2PS.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

namespace PR2PS.GameServer
{
    /// <summary>
    /// Represents the game server.
    /// TODO - Deatiled description.
    /// </summary>
    public class PR2Server
    {
        /// <summary>
        /// Seconds elapsed since 1.1.1970 UTC.
        /// </summary>
        public Int32 SecondsSinceUnixTime { get; private set; }
        /// <summary>
        /// Gets available login id.
        /// </summary>
        public Int32 LoginId { get { return ++loginId; } }

        public String ServerName { get; private set; }

        /// <summary>
        /// Dictionary mapping login id to connected client.
        /// Clients in this dictionary are successfuly connected, but has not been authentificated yet.
        /// </summary>
        private Dictionary<Int32, ConnectedClient> clientsNotAuth;
        /// <summary>
        /// Dictionary mapping login id to authentificated client.
        /// Clients in this dictionary has been already authentificated.
        /// </summary>
        private Dictionary<Int32, ConnectedClient> clientsAuth;
        /// <summary>
        /// Dictionary mapping chatroom names to the existing chatrooms.
        /// </summary>
        private Dictionary<String, ChatRoom> chatRooms;
        /// <summary>
        /// Dictionary mapping rightroom names to the existing rightrooms.
        /// </summary>
        private Dictionary<String, RightRoom> rightRooms;

        private Object clientsNotAuthSyncLock;
        private Object clientsAuthSyncLock;
        private Object chatRoomsSyncLock;
        private Object rightRoomsSyncLock;

        private Int32 loginId = 0;
        private Timer oneSecTimer;
        private DateTime unixTime;

        public PR2Server(String name)
        {
            this.ServerName = name;
            this.clientsNotAuth = new Dictionary<Int32, ConnectedClient>();
            this.clientsAuth = new Dictionary<Int32, ConnectedClient>();
            this.chatRooms = new Dictionary<String, ChatRoom>();
            this.rightRooms = new Dictionary<String, RightRoom>();
            this.clientsNotAuthSyncLock = new Object();
            this.clientsAuthSyncLock = new Object();
            this.chatRoomsSyncLock = new Object();
            this.rightRoomsSyncLock = new Object();

            this.unixTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
            this.oneSecTimer = new Timer(1000);
            this.oneSecTimer.Elapsed += oneSecTimer_Elapsed;
            this.oneSecTimer.Start();

            this.chatRooms.Add(GameRooms.ROOM_MAIN, new ChatRoom(GameRooms.ROOM_MAIN, true, this));
            this.rightRooms.Add(GameRooms.ROOM_CAMPAIGN, new RightRoom(GameRooms.ROOM_CAMPAIGN, this));
            this.rightRooms.Add(GameRooms.ROOM_ALLTIMEBEST, new RightRoom(GameRooms.ROOM_ALLTIMEBEST, this));
            this.rightRooms.Add(GameRooms.ROOM_TODAYSBEST, new RightRoom(GameRooms.ROOM_TODAYSBEST, this));
            this.rightRooms.Add(GameRooms.ROOM_SEARCH, new RightRoom(GameRooms.ROOM_SEARCH, this));
        }

        /// <summary>
        /// Event handler for 1s timer.
        /// Updates the property indicating seconds elapsed since 1.1.1970 UTC.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void oneSecTimer_Elapsed(Object sender, ElapsedEventArgs e)
        {
            TimeSpan result = DateTime.UtcNow.Subtract(this.unixTime);
            this.SecondsSinceUnixTime = Convert.ToInt32(result.TotalSeconds);
        }

        /// <summary>
        /// Adds successfuly connected client into dictionary of connected not-authentificated clients.
        /// This method is called as soon as client requests his unique login id.
        /// </summary>
        /// <param name="loginID">Login id of the client.</param>
        /// <param name="client">Connected client itself.</param>
        public void AddClient(Int32 loginID, ConnectedClient client)
        {
            lock (this.clientsNotAuthSyncLock)
            {
                this.clientsNotAuth.Add(loginID, client);
            }
        }

        /// <summary>
        /// Handler, when client closes connection.
        /// Deauth client if necessary and also remove it from all lists/dictionaries.
        /// </summary>
        /// <param name="client">Disconnected client.</param>
        public void RemoveClient(ConnectedClient client)
        {
            if (client == null) return;

            Boolean removeSuccessful = false;

            if (client.IsAuthentificated)
            {
                lock (this.clientsAuthSyncLock)
                {
                    removeSuccessful = this.clientsAuth.Remove(client.LoginID);
                }

                if (removeSuccessful)
                {
                    // Client has been successfuly removed from the dictionary.

                    // Remove from chat room if in one.
                    this.MoveClientToChatRoom(GameRooms.ROOM_NONE, client);
                    // Remove from right room if in one.
                    this.MoveClientToRightRoom(GameRooms.ROOM_NONE, client);
                    // Forfeit and remove from game if in one.
                    this.MoveClientToGameRoom(GameRooms.ROOM_NONE, client);

                    // Upload character information back to the web server and logout client.
                    client.PushUpdateRPC();
                    client.LogoutRPC();
                }
                else
                {
                    // Client has either not even been authentificated or it was removed already.

                    Console.WriteLine("Client {0} cannot be deauthentificated, because it was either not authentificated or it was removed already.", loginId);
                }
            }
            else
            {
                lock (this.clientsNotAuthSyncLock)
                {
                    removeSuccessful = this.clientsNotAuth.Remove(client.LoginID);
                }

                if (!removeSuccessful)
                {
                    // No such client exists.

                    Console.WriteLine("Client {0} cannot be removed, because it was not even found.", loginId);
                }
            }
        }

        /// <summary>
        /// RPC handler when client is successfuly authentificated on the web server.
        /// Sends login successful packet which will display lobby on the client.
        /// </summary>
        /// <param name="loginId">Login id sent by web server.</param>
        /// <param name="accModel">Accoutn model sent by web server.</param>
        public void LoginSuccessful(Int32 loginId, AccountDataDTO accData)
        {
            ConnectedClient cli = null;

            lock (this.clientsNotAuthSyncLock)
            {
                // Try to find connected client with specific login id.
                
                this.clientsNotAuth.TryGetValue(loginId, out cli);
            }

            if(cli == null)
            {
                // Client not found.

                Console.WriteLine("Error occured while attempting to authentificate client {0}.", loginId);
            }
            else
            {
                // Client found. Do the authentification procedure.

                lock (this.clientsNotAuthSyncLock)
                {
                    this.clientsNotAuth.Remove(loginId);
                }

                lock (this.clientsAuthSyncLock)
                {
                    this.clientsAuth.Add(loginId, cli);
                }

                cli.IsAuthentificated = true;
                cli.AccData = accData;

                cli.SendMessage("loginSuccessful`1");
                cli.SendMessage(String.Format("setRank`{0}", cli.AccData.Rank));
                if (cli.AccData.Group > UserGroup.MEMBER) cli.SendMessage(String.Format("setGroup`{0}", cli.AccData.Group));
                //cli.SendMessage("message`Welcome to PR2 private server. :)");
            }
        }

        /// <summary>
        /// RPC handler when it is necessary to kick client from game.
        /// </summary>
        /// <param name="userId">Account id of user.</param>
        /// <param name="ip">IP address. If specified, all clients with this IP will be logged out.</param>
        public void ForceLogout(Int64 userId, String ip)
        {
            List<ConnectedClient> clients;

            lock (this.clientsAuthSyncLock)
            {
                if (String.IsNullOrEmpty(ip))
                {
                    clients = this.clientsAuth.Values.Where(cli =>
                        cli.AccData.UserId == userId).ToList();
                }
                else
                {
                    clients = this.clientsAuth.Values.Where(cli =>
                        cli.AccData.UserId == userId
                        || cli.IPAddress == ip).ToList();
                }
            }

            foreach (ConnectedClient cli in clients)
            {
                this.RemoveClient(cli);
                cli.Disconnect();
            }
        }

        /// <summary>
        /// RPC handler. Adds system message to chat room main.
        /// </summary>
        /// <param name="message">Message content.</param>
        /// <param name="user">If specified, only this user will see the system message.</param>
        public void SystemMessage(String message, String user)
        {
            if (String.IsNullOrEmpty(message))
            {
                return;
            }

            if (user == null)
            {
                if (this.chatRooms[GameRooms.ROOM_MAIN] != null)
                {
                    this.chatRooms[GameRooms.ROOM_MAIN].PushSystemMessage(message);
                }
            }
            else
            {
                ConnectedClient foundClient = this.FindClient(user);
                if (foundClient != null)
                {
                    foundClient.SendMessage(String.Concat("systemChat`", message));
                }
            }
        }

        /// <summary>
        /// Sends list of players online to the client.
        /// </summary>
        /// <param name="client">Receiver of the list.</param>
        public void SendOnlineList(ConnectedClient destClient)
        {
            // TODO - Implement better solution rather than iteration on every request.

            lock (this.clientsAuthSyncLock)
            {
                foreach (ConnectedClient cli in this.clientsAuth.Values)
                {
                    destClient.SendMessage(String.Format("addUser`{0}`{1}`{2}`{3}", cli.AccData.Username, cli.AccData.Group, cli.AccData.Rank, cli.AccData.HatSeq.Split(Separators.COMMA_SEPARATOR, StringSplitOptions.RemoveEmptyEntries).Length - 1));
                }
            }
        }

        /// <summary>
        /// Moves client from his previous chatroom to specified one or simply adds to new one if no previous chatroom present.
        /// </summary>
        /// <param name="chatRoom">Chatroom name where to move client. If the argument is 'none' the client gets removed.</param>
        /// <param name="client">Client.</param>
        public void MoveClientToChatRoom(String chatRoomName, ConnectedClient client)
        {
            if (chatRoomName == GameRooms.ROOM_NONE)
            {
                // Firstly check if client even is in chatroom.
                if (client.ChatRoom != null)
                {
                    // It is, lets remove it.
                    client.ChatRoom.RemoveClientFromChatRoom(client);
                    if (!client.ChatRoom.Permament && client.ChatRoom.MembersCount == 0)
                    {
                        // If the chatroom is not permament and membercount is 0, remove the chatroom completely.
                        lock (this.chatRoomsSyncLock)
                        {
                            this.chatRooms.Remove(client.ChatRoom.Name);
                        }
                    }
                    client.ChatRoom = null;
                }
            }
            else
            {
                if (client.ChatRoom == null)
                {
                    // Client is not in any chatroom atm, no need to move it.

                    ChatRoom chatRoom = null;

                    lock (this.chatRoomsSyncLock)
                    {
                        this.chatRooms.TryGetValue(chatRoomName, out chatRoom);
                    }

                    if (chatRoom == null)
                    {
                        // No such chatroom exists, we need to create it firstly.
                        chatRoom = new ChatRoom(chatRoomName, false, this);
                        chatRoom.AddClientToChatRoom(client);
                        chatRoom.SendMeMessages(client);
                        client.ChatRoom = chatRoom;

                        lock (this.chatRoomsSyncLock)
                        {
                            this.chatRooms.Add(chatRoomName, chatRoom);
                        }
                    }
                    else
                    { 
                        // Chatroom exists, lets simply add the client into it.
                        chatRoom.AddClientToChatRoom(client);
                        chatRoom.SendMeMessages(client);
                        client.ChatRoom = chatRoom;
                    }
                }
                else
                {
                    // We need to remove client from his old chatroom before adding to new one.

                    client.ChatRoom.RemoveClientFromChatRoom(client);
                    if (!client.ChatRoom.Permament && client.ChatRoom.MembersCount == 0)
                    {
                        // If the chatroom is not permament and membercount is 0, remove the chatroom completely.
                        lock (this.chatRoomsSyncLock)
                        {
                            this.chatRooms.Remove(client.ChatRoom.Name);
                        }
                    }

                    // Client removed, now we need to check whether the specified chatroom exists.

                    ChatRoom chatRoom = null;

                    lock (this.chatRoomsSyncLock)
                    {
                        this.chatRooms.TryGetValue(chatRoomName, out chatRoom);
                    }

                    if (chatRoom == null)
                    {
                        // No such chatroom exists, we need to create it firstly.
                        chatRoom = new ChatRoom(chatRoomName, false, this);
                        chatRoom.AddClientToChatRoom(client);
                        chatRoom.SendMeMessages(client);
                        client.ChatRoom = chatRoom;

                        lock (this.chatRoomsSyncLock)
                        {
                            this.chatRooms.Add(chatRoomName, chatRoom);
                        }
                    }
                    else
                    {
                        // Chatroom exists, lets simply add the client into it.
                        chatRoom.AddClientToChatRoom(client);
                        chatRoom.SendMeMessages(client);
                        client.ChatRoom = chatRoom;
                    }
                }
            }
        }

        /// <summary>
        /// Sends list of chat rooms to the specific client.
        /// </summary>
        /// <param name="client">Receiver of the chat rooms list.</param>
        public void SendMeChatRoomList(ConnectedClient client)
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append("setChatRoomList");

            lock (this.chatRoomsSyncLock)
            {
                foreach (ChatRoom room in this.chatRooms.Values)
                {
                    strBuilder.Append(Separators.ARG_SEPARATOR);
                    strBuilder.Append(room.Name);
                    strBuilder.Append(" - ");
                    strBuilder.Append(room.MembersCount);
                }
            }

            client.SendMessage(strBuilder.ToString());
        }

        /// <summary>
        /// Moves client from his previous rightroom to specified one.
        /// </summary>
        /// <param name="chatRoom">Rightroom name where to move client. If the argument is 'none', the client gets removed.</param>
        /// <param name="client">Client.</param>
        public void MoveClientToRightRoom(String rightRoomName, ConnectedClient client)
        {
            switch (rightRoomName)
            { 
                case GameRooms.ROOM_NONE:
                    // Firstly check if client even is in rightroom.
                    if (client.RightRoom != null)
                    {
                        // It is, lets remove it.
                        client.RightRoom.ClearSlot(client, true);
                        client.RightRoom.RemoveClientFromRightRoom(client);
                    }
                    break;

                case GameRooms.ROOM_CAMPAIGN:
                case GameRooms.ROOM_ALLTIMEBEST:
                case GameRooms.ROOM_TODAYSBEST:
                case GameRooms.ROOM_SEARCH:
                    if (client.RightRoom == null)
                    {
                        // Client is not in any rightroom atm, no need to move it.

                        RightRoom rightRoom = null;
                        lock (this.rightRoomsSyncLock)
                        {
                            this.rightRooms.TryGetValue(rightRoomName, out rightRoom);
                        }

                        if (rightRoom == null)
                        {
                            // No such rightroom exists, however it should exist!
                            Console.WriteLine("Error, client {0} attempled to join rightroom {1} and even tho it should exist, it does not!", client.LoginID, rightRoomName);
                        }
                        else
                        {
                            // Rightroom exists, add the client into it.
                            rightRoom.AddClientToRightRoom(client);
                            client.RightRoom = rightRoom;
                        }
                    }
                    else
                    {
                        // We need to remove client from his old rightroom before adding to specified one.

                        client.RightRoom.ClearSlot(client, true);
                        client.RightRoom.RemoveClientFromRightRoom(client);

                        RightRoom rightRoom = null;
                        lock (this.rightRoomsSyncLock)
                        {
                            this.rightRooms.TryGetValue(rightRoomName, out rightRoom);
                        }

                        if (rightRoom == null)
                        {
                            // No such rightroom exists, however it should exist!
                            Console.WriteLine("Error, client {0} attempled to join rightroom {1} and even tho it should exist, it does not!", client.LoginID, rightRoomName);
                        }
                        else
                        {
                            // Rightroom exists, add the client into it.
                            rightRoom.AddClientToRightRoom(client);
                            client.RightRoom = rightRoom;
                        }
                    }
                    break;

                default:
                    // Illegal rightroom join attempt, print message for now.
                    // TODO - Better handling.
                    Console.WriteLine("Error, client {0} tried to join illegal rightroom {1}.", client.LoginID, rightRoomName);
                    break;
            }
        }

        /// <summary>
        /// Moves client from his previous gameroom to specified one.
        /// Argument is always 'none', therefore this method will make player forfeit and removes him from game.
        /// </summary>
        /// <param name="gameRoomName">Gameroom name where to move client. If the argument is 'none', the client gets removed.</param>
        /// <param name="client">Client.</param>
        public void MoveClientToGameRoom(String gameRoomName, ConnectedClient client)
        {
            switch (gameRoomName)
            {
                case GameRooms.ROOM_NONE:
                    // Firstly check if client even is ingame.
                    if (client.Game != null)
                    {
                        // It is, lets make him forfeit + remove it.
                        client.Game.RemoveClient(client);
                    }
                    break;

                default:
                    // Illegal gameroom join attempt, print message for now.
                    // TODO - Better handling.
                    Console.WriteLine("Error, client {0} tried to join illegal gameroom {1}.", client.LoginID, gameRoomName);
                    break;
            }
        }

        public ConnectedClient FindClient(String userName)
        {
            lock (this.clientsAuthSyncLock)
            {
                return this.clientsAuth.Values.FirstOrDefault(cli =>
                    cli.AccData != null &&
                    String.Compare(cli.AccData.Username, userName, true) == 0);
            }
        }
    }
}
