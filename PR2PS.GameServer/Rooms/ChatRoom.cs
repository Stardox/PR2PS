using PR2PS.Common.Constants;
using System;
using System.Collections.Generic;

namespace PR2PS.GameServer
{
    public class ChatRoom
    {
        public String Name { get; private set; }
        public Boolean Permament { get; private set; }
        /// <summary>
        /// Gets the number of members in this chatroom.
        /// </summary>
        public Int32 MembersCount
        { 
            get 
            {
                lock (this.membersSyncLock) return this.members.Count;
            }
        }

        private Dictionary<Int32, ConnectedClient> members;
        private Object membersSyncLock;
        private Queue<String> messages;
        private Object messagesSyncLock;
        private PR2Server gameServer;

        public ChatRoom(String name, Boolean permament, PR2Server gameServer)
        {
            this.Name = name;
            this.Permament = permament;
            this.gameServer = gameServer;
            this.members = new Dictionary<Int32, ConnectedClient>();
            this.membersSyncLock = new Object();
            this.messages = new Queue<String>();
            this.messagesSyncLock = new Object();
            this.messages.Enqueue(String.Format("systemChat`Welcome to the chatroom {0}!", name));
        }

        /// <summary>
        /// Removes client from chatroom memberlist.
        /// </summary>
        /// <param name="client">Client to remove.</param>
        public void RemoveClientFromChatRoom(ConnectedClient client)
        {
            lock (this.membersSyncLock)
            {
                this.members.Remove(client.LoginID);
            }
        }

        /// <summary>
        /// Adds client to chatroom memberlist.
        /// </summary>
        /// <param name="client">Client to add.</param>
        public void AddClientToChatRoom(ConnectedClient client)
        {
            lock (this.membersSyncLock)
            {
                if (this.members.ContainsKey(client.LoginID))
                {
                    Console.WriteLine("Error! Client {0} is already present in chatroom {1}", client.LoginID, this.Name);
                }
                else
                {
                    this.members.Add(client.LoginID, client);
                }
            }
        }

        /// <summary>
        /// Adds message to the queue of chatroom messages.
        /// Also sends the message to chatroom membes.
        /// </summary>
        /// <param name="sender">Client, who sent the message.</param>
        /// <param name="message">Text message.</param>
        public void PushMessage(ConnectedClient sender, String message)
        {
            // Checks if message is actual game command.
            if (message.StartsWith("/giveall "))
            {
                if(sender.AccData.Group == 3)
                {
                    String[] commandParts = message.Split(new Char[] { ' ' }, 2);
                    if(commandParts.Length > 1)
                    {
                        ConnectedClient foundCli = this.gameServer.FindClient(commandParts[1]);
                        if (foundCli == null)
                        {
                            sender.SendMessage(String.Format("systemChat`User '{0}' is not currently online.", commandParts[1]));
                        }
                        else
                        {
                            foundCli.AccData.HatSeq = BodyParts.PARTS_ALL_HATS;
                            foundCli.AccData.HeadSeq = BodyParts.PARTS_ALL_HEADS;
                            foundCli.AccData.BodySeq = BodyParts.PARTS_ALL_BODIES;
                            foundCli.AccData.FeetSeq = BodyParts.PARTS_ALL_FEET;
                            foundCli.AccData.HatSeqEpic = BodyParts.PARTS_ALL_HATS;
                            foundCli.AccData.HeadSeqEpic = BodyParts.PARTS_ALL_HEADS;
                            foundCli.AccData.BodySeqEpic = BodyParts.PARTS_ALL_BODIES;
                            foundCli.AccData.FeetSeqEpic = BodyParts.PARTS_ALL_FEET;
                            foundCli.AccData.ObtainedRankTokens = 150;

                            sender.SendMessage(String.Format("systemChat`Success! User '{0}' has unlocked everything.", commandParts[1]));
                        }

                        return;
                    }
                }
            }

            String niceMessage = String.Format("chat`{0}`{1}`{2}", sender.AccData.Username, sender.AccData.Group, message);

            lock (this.messagesSyncLock)
            {
                this.messages.Enqueue(niceMessage);
                if (this.messages.Count > 20)
                {
                    this.messages.Dequeue();
                }
            }

            lock (this.membersSyncLock)
            {
                foreach (ConnectedClient cli in this.members.Values)
                {
                    cli.SendMessage(niceMessage);
                }
            }
        }

        /// <summary>
        /// Adds system message to the queue of chatroom messages.
        /// Also sends the message to chatroom membes.
        /// </summary>
        /// <param name="message">Text message.</param>
        public void PushSystemMessage(String message)
        {
            String niceMessage = String.Concat("systemChat`", message);

            lock (this.messagesSyncLock)
            {
                this.messages.Enqueue(niceMessage);
                if (this.messages.Count > 20)
                {
                    this.messages.Dequeue();
                }
            }

            lock (this.membersSyncLock)
            {
                foreach (ConnectedClient cli in this.members.Values)
                {
                    cli.SendMessage(niceMessage);
                }
            }
        }

        /// <summary>
        /// Sends all messages of the currrent chatroom to specified client. Useful, when client connects to chatroom.
        /// </summary>
        /// <param name="receiver">Client, who will receive the messages.</param>
        public void SendMeMessages(ConnectedClient receiver)
        {
            lock (this.messagesSyncLock)
            {
                foreach (String msg in this.messages)
                {
                    receiver.SendMessage(msg);
                }
            }
        }
    }
}
