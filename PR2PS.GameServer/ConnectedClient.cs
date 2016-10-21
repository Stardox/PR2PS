using Microsoft.AspNet.SignalR.Client;
using PR2PS.Common.Constants;
using PR2PS.Common.DTO;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;

namespace PR2PS.GameServer
{
    public class ConnectedClient
    {
        /// <summary>
        /// Number of sent messages.
        /// </summary>
        private Int32 sendCount = 0;
        /// <summary>
        /// Used for TCP communication with the connected client.
        /// </summary>
        private NetworkStream connection;
        /// <summary>
        /// Address of connected client.
        /// </summary>
        private EndPoint endPoint;
        /// <summary>
        /// Game server.
        /// </summary>
        private PR2Server gameServer;
        /// <summary>
        /// Used for generating checksum and also for message verification.
        /// </summary>
        private PacketCrypto hasher;
        private IHubProxy remoteServerHubProxy;

        /// <summary>
        /// Assigned login id.
        /// </summary>
        public Int32 LoginID { get; private set; }
        /// <summary>
        /// Determines, whether the client has alrady been authentificated by web server
        /// and received customize info data.
        /// </summary>
        public Boolean IsAuthentificated { get; set; }
        /// <summary>
        /// Connected client data (username, bodyparts, etc).
        /// </summary>
        public AccountDataDTO AccData { get; set; }
        /// <summary>
        /// Information about chatroom to which client is connected or null if none.
        /// </summary>
        public ChatRoom ChatRoom { get; set; }
        /// <summary>
        /// Information about rightroom to which client is connected or null if none.
        /// </summary>
        public RightRoom RightRoom { get; set; }
        /// <summary>
        /// Information about slot, which client is occupying or null if none.
        /// </summary>
        public OccupancyInfo OccupancyInfo { get; set; }
        /// <summary>
        /// Information about game, which client is in or null if none.
        /// </summary>
        public Race Game { get; set; }
        /// <summary>
        /// Gets IP address of the connected client.
        /// </summary>
        public String IPAddress
        { 
            get
            {
                IPEndPoint ipEndPt = this.endPoint as IPEndPoint;
                if (ipEndPt == null) return String.Empty;
                return ipEndPt.Address.ToString();
            }
        }

        public ConnectedClient(NetworkStream connection, EndPoint endPoint, PR2Server gameServer, IHubProxy remoteServerHubProxy)
        {
            this.connection = connection;
            this.endPoint = endPoint;
            this.gameServer = gameServer;
            this.remoteServerHubProxy = remoteServerHubProxy;
            this.hasher = new PacketCrypto();
        }

        /// <summary>
        /// Uploads account data to the web server.
        /// </summary>
        public void PushUpdateRPC()
        {
            this.remoteServerHubProxy.SaveClientData(this.AccData);
        }

        /// <summary>
        /// Removes this client session from remote web server.
        /// </summary>
        public void LogoutRPC()
        {
            this.remoteServerHubProxy.LogoutClient(this.gameServer.ServerName, this.LoginID);
        }

        /// <summary>
        /// Tells web server that the status of this user has changed.
        /// Parameter action can contain 'temporary', 'trial', 'permament' or 'demod'.
        /// </summary>
        public void ChangeModStatusRPC(String receiver, String action)
        {
            this.remoteServerHubProxy.ChangeModStatus(this.gameServer.ServerName, this.AccData.Username, receiver, action);
        }

        /// <summary>
        /// Handles the message sent by connected client.
        /// Message format example:
        /// 138`1`request_login_id` + end of transmission character.
        /// </summary>
        /// <param name="recvMsg">Received message in its raw format.</param>
        public void HandleReceivedMessage(String recvMsg)
        {
            if (recvMsg.Equals(GameConstants.POLICY_FILE_XML_REQUEST))
            {
                // Client is requesting policy file, send it.
                Byte[] sendData = sendData = ASCIIEncoding.ASCII.GetBytes(GameConstants.POLICY_FILE_XML_RESPONSE);
                connection.Write(sendData, 0, sendData.Length);
            }
            else if(Regex.Match(recvMsg, GameConstants.PACKET_REGEX).Success)
            {
                // Successfuly verified received message as PR2 message.

                // Split message according to EOT incase that client sent multiple messages concatenated.
                String[] packets = recvMsg.Split(Separators.EOT_SEPARATOR, StringSplitOptions.RemoveEmptyEntries);

                // Iterate through every single message and process it.
                foreach (String packet in packets)
                {
                    // First off verify, whether 3 bytes long hash matches.
                    if (hasher.VerifyPacket(packet))
                    {
                        String[] msgSegments = packet.Split(Separators.ARG_SEPARATOR);

                        #region Handle these requests even if client has not been authentificated yet.

                        switch (msgSegments[2])
                        {
                            case "request_login_id":
                                this.LoginID = this.gameServer.LoginId;
                                this.gameServer.AddClient(this.LoginID, this);
                                this.SendMessage(String.Format("setLoginID`{0}", this.LoginID));
                                break;

                            case "ping":
                                this.SendMessage(String.Format("ping`{0}", this.gameServer.SecondsSinceUnixTime));
                                break;
                        }

                        #endregion

                        #region Handle these requests only when client has been authentificated already.

                        if (IsAuthentificated)
                        {
                            // Handle these request only when connected client has been authentificated already.

                            switch (msgSegments[2])
                            {
                                case "get_customize_info":
                                    this.SendMessage(String.Format("setCustomizeInfo`{0}", this.AccData.ToString()));
                                    this.SendMessage(String.Format("setRank`{0}", this.AccData.Rank));
                                    if (this.AccData.Group > 1) this.SendMessage(String.Format("setGroup`{0}", this.AccData.Group));
                                    break;

                                case "get_online_list":
                                    this.gameServer.SendOnlineList(this);
                                    break;

                                case "set_customize_info":
                                    if (msgSegments.Length >= 18)
                                    {
                                        this.AccData.HatColor = Convert.ToInt32(msgSegments[3]);
                                        this.AccData.HeadColor = Convert.ToInt32(msgSegments[4]);
                                        this.AccData.BodyColor = Convert.ToInt32(msgSegments[5]);
                                        this.AccData.FeetColor = Convert.ToInt32(msgSegments[6]);
                                        this.AccData.HatColor2 = Convert.ToInt32(msgSegments[7]);
                                        this.AccData.HeadColor2 = Convert.ToInt32(msgSegments[8]);
                                        this.AccData.BodyColor2 = Convert.ToInt32(msgSegments[9]);
                                        this.AccData.FeetColor2 = Convert.ToInt32(msgSegments[10]);
                                        this.AccData.Hat = Convert.ToInt32(msgSegments[11]);
                                        this.AccData.Head = Convert.ToInt32(msgSegments[12]);
                                        this.AccData.Body = Convert.ToInt32(msgSegments[13]);
                                        this.AccData.Feet = Convert.ToInt32(msgSegments[14]);
                                        this.AccData.Speed = Convert.ToInt32(msgSegments[15]);
                                        this.AccData.Accel = Convert.ToInt32(msgSegments[16]);
                                        this.AccData.Jump = Convert.ToInt32(msgSegments[17]);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Client {0} error - too few arguments in customize info data.", this.LoginID);
                                    }
                                    break;

                                case "use_rank_token":
                                    if (this.AccData.ObtainedRankTokens > 0 && this.AccData.UsedRankTokens < this.AccData.ObtainedRankTokens)
                                    {
                                        this.AccData.Rank++;
                                        this.AccData.UsedRankTokens++;
                                    }
                                    break;

                                case "unuse_rank_token":
                                    if (this.AccData.ObtainedRankTokens > 0 && this.AccData.UsedRankTokens > 0)
                                    {
                                        if (this.AccData.GetRemainingStats() > 0)
                                        { 
                                            // There are still available points left,
                                            // let the client do the calculations.
                                        }
                                        else if (this.AccData.Speed > 0)
                                        {
                                            this.AccData.Speed--;
                                        }
                                        else if (this.AccData.Accel > 0)
                                        {
                                            this.AccData.Accel--;
                                        }
                                        else if (this.AccData.Jump > 0)
                                        {
                                            this.AccData.Jump--;
                                        }

                                        this.AccData.Rank--;
                                        this.AccData.UsedRankTokens--;
                                    }
                                    break;

                                case "set_chat_room":
                                    if (msgSegments.Length < 4)
                                    {
                                        // If for some reason client has not specified room name, set it to none.
                                        this.gameServer.MoveClientToChatRoom(GameRooms.ROOM_NONE, this);
                                    }
                                    else
                                    {
                                        this.gameServer.MoveClientToChatRoom(msgSegments[3], this);
                                    }
                                    break;

                                case "get_chat_rooms":
                                    this.gameServer.SendMeChatRoomList(this);
                                    break;

                                case "chat":
                                    if (this.ChatRoom != null)
                                    {
                                        this.ChatRoom.PushMessage(this, msgSegments[3]);
                                    }
                                    else if (this.Game != null)
                                    {
                                        this.Game.BroadCast(String.Format("chat`{0}`{1}`{2}", this.AccData.Username, this.AccData.Group, msgSegments[3]));
                                    }
                                    break;

                                case "set_right_room":
                                    if (msgSegments.Length < 4)
                                    {
                                        // If for some reason client has not specified room name, set it to none.
                                        this.gameServer.MoveClientToRightRoom(GameRooms.ROOM_NONE, this);
                                    }
                                    else
                                    {
                                        this.gameServer.MoveClientToRightRoom(msgSegments[3], this);
                                    }
                                    break;

                                case "set_game_room":
                                    if (msgSegments.Length < 4)
                                    {
                                        // If for some reason client has not specified room name, set it to none.
                                        this.gameServer.MoveClientToGameRoom(GameRooms.ROOM_NONE, this);
                                    }
                                    else
                                    {
                                        this.gameServer.MoveClientToGameRoom(msgSegments[3], this);
                                    }
                                    break;

                                case "fill_slot":
                                    if (msgSegments.Length > 4)
                                    {
                                        if (this.RightRoom != null)
                                        {
                                            Int32 slot;
                                            if (Int32.TryParse(msgSegments[4], out slot))
                                            {
                                                this.RightRoom.FillSlot(this, msgSegments[3], slot);
                                            }
                                        }
                                    }
                                    break;

                                case "clear_slot":
                                    if (this.RightRoom != null)
                                    {
                                        this.RightRoom.ClearSlot(this, true);
                                    }
                                    break;

                                case "confirm_slot":
                                    if (this.RightRoom != null)
                                    {
                                        this.RightRoom.ConfirmSlot(this);
                                    }

                                    break;

                                case "finish_drawing":
                                    if (this.Game != null)
                                    {
                                        if (msgSegments.Length == 8)
                                        {
                                            this.Game.HandleFinishDrawing(this, msgSegments);
                                        }
                                    }
                                    break;

                                case "finish_race":
                                    if (this.Game != null)
                                    {
                                        if (msgSegments.Length == 6)
                                        {
                                            this.Game.Finish(this, msgSegments);
                                        }
                                    }
                                    break;

                                case "quit_race":
                                    if (this.Game != null)
                                    {
                                        this.Game.Forfeit(this);
                                    }
                                    break;

                                case "p":
                                    if (this.Game != null)
                                    {
                                        if (msgSegments.Length == 5)
                                        {
                                            this.Game.BroadCastPosition(this, msgSegments);
                                        }
                                    }
                                    break;

                                case "exact_pos":
                                    if (this.Game != null)
                                    {
                                        if (msgSegments.Length == 5)
                                        {
                                            this.Game.BroadCastExactPosition(this, msgSegments);
                                        }
                                    }
                                    break;

                                case "set_var":
                                    if (this.Game != null)
                                    {
                                        if (msgSegments.Length > 3)
                                        {
                                            this.Game.BroadCastVar(this, msgSegments);
                                        }
                                    }
                                    break;

                                case "add_effect":
                                    if (this.Game != null)
                                    {
                                        if (msgSegments.Length > 3)
                                        {
                                            this.Game.BroadCastEffect(this, msgSegments);
                                        }
                                    }
                                    break;

                                case "activate":
                                    if (this.Game != null)
                                    {
                                        if (msgSegments.Length > 3)
                                        {
                                            this.Game.BroadCastActivate(this, msgSegments);
                                        }
                                    }
                                    break;

                                case "zap":
                                    if (this.Game != null)
                                    {
                                        this.Game.BroadCastZap(this);
                                    }
                                    break;

                                case "loose_hat":
                                    if (this.Game != null)
                                    {
                                        if (msgSegments.Length == 6)
                                        {
                                            this.Game.LooseHat(this, msgSegments);
                                        }
                                    }
                                    break;

                                case "get_hat":
                                    if (this.Game != null)
                                    {
                                        if (msgSegments.Length == 4)
                                        {
                                            this.Game.GetHat(this, msgSegments[3]);
                                        }
                                    }
                                    break;

                                case "promote_to_moderator":
                                    if (msgSegments.Length == 5)
                                    {
                                        if (this.AccData.Group == 3)
                                        {
                                            ConnectedClient target = this.gameServer.FindClient(msgSegments[3]);
                                            if (target != null)
                                            {
                                                if (target.AccData.Group == 1)
                                                {
                                                    target.AccData.Group = 2;
                                                }
                                            }

                                            this.ChangeModStatusRPC(msgSegments[3], msgSegments[4]);
                                        }
                                    }
                                    break;

                                case "demote_moderator":
                                    if (msgSegments.Length == 4)
                                    {
                                        if (this.AccData.Group == 3)
                                        {
                                            ConnectedClient target = this.gameServer.FindClient(msgSegments[3]);
                                            if (target != null)
                                            {
                                                if (target.AccData.Group == 2)
                                                {
                                                    target.AccData.Group = 1;
                                                }
                                            }

                                            this.ChangeModStatusRPC(msgSegments[3], "demod");
                                        }
                                    }
                                    break;

                              

                                //default:
                                //    Console.WriteLine(String.Format("{0}: Unrecognized packet header: {1}", this.endPoint, msgSegments[2]));
                                //    break;
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        Console.WriteLine(String.Format("{0}: Failed to verify packet: {1}", this.endPoint, packet));
                        this.connection.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Sends message to connected client.
        /// Message has following format: packetIdentifier`arguments
        /// Example: setLoginID`1
        /// </summary>
        /// <param name="message">Message to send.</param>
        public void SendMessage(String message)
        {
            String packetWithoutHash = String.Format("{0}`{1}", this.sendCount++, message);
            String finalPacket = String.Concat(hasher.GetPacketWithHash(packetWithoutHash), Separators.EOT_CHAR);
            
            String logMsg = String.Format("To {0}: {1}", this.endPoint, finalPacket);
            //Console.WriteLine(logMsg);

            Byte[] sendData = ASCIIEncoding.ASCII.GetBytes(finalPacket);

            try
            {
                this.connection.Write(sendData, 0, sendData.Length);
                this.connection.Flush();
            }
            catch (IOException)
            { 
                // Client most likely disconnected/X-ed out, remove him.
                this.gameServer.RemoveClient(this);
                Console.WriteLine("Error occured while sending message to {0}.", this.ToString());
            }
        }

        public override String ToString()
        {
            if (this.AccData != null)
            {
                return String.Format("IP: {0}, login id: {1}, username: {2}", this.endPoint, this.LoginID, this.AccData.Username);
            }
            else
            {
                return base.ToString();
            }
        }

        public void Disconnect()
        {
            this.connection.Close();
        }
    }
}
