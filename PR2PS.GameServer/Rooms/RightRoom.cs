using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR2PS.GameServer
{
    public class RightRoom
    {
        public String Name { get; private set; }
        /// <summary>
        /// Gets the number of members in this room.
        /// </summary>
        public Int32 MembersCount
        { 
            get 
            {
                lock (this.membersSyncLock) return this.members.Count;
            }
        }

        private Dictionary<Int32, ConnectedClient> members;
        private Dictionary<String, OccupancyInfo> occupiedSlots;
        private Object membersSyncLock;
        private Object occupiedSlotsSyncLock;
        private PR2Server gameServer;

        public RightRoom(String name, PR2Server gameServer)
        {
            this.Name = name;
            this.gameServer = gameServer;
            this.members = new Dictionary<Int32, ConnectedClient>();
            this.occupiedSlots = new Dictionary<String, OccupancyInfo>();
            this.membersSyncLock = new Object();
            this.occupiedSlotsSyncLock = new Object();
        }

        /// <summary>
        /// Removes client from this right room.
        /// This method wont clear the slot that client might be occupying.
        /// </summary>
        /// <param name="client">Client to remove.</param>
        public void RemoveClientFromRightRoom(ConnectedClient client)
        {
            lock (this.membersSyncLock)
            {
                this.members.Remove(client.LoginID);
            }

            client.RightRoom = null;
        }

        /// <summary>
        /// Adds client to this right room memberlist and sends him occupancy info.
        /// </summary>
        /// <param name="client">Client to add.</param>
        public void AddClientToRightRoom(ConnectedClient client)
        {
            lock (this.membersSyncLock)
            {
                if (this.members.ContainsKey(client.LoginID))
                {
                    Console.WriteLine("Error! Client {0} is already present in rightroom {1}", client.LoginID, this.Name);
                }
                else
                {
                    this.members.Add(client.LoginID, client);
                }
            }

            this.sendMeSlotsStatus(client);
        }

        /// <summary>
        /// Handler, when client requests to be put on specific level slot.
        /// </summary>
        /// <param name="sender">Client who sent request.</param>
        /// <param name="identifier">Level identifier usually in format levelID_version.</param>
        /// <param name="slot">Slot number from range 0-3.</param>
        public void FillSlot(ConnectedClient sender, String identifier, Int32 slot)
        {
            // First off, check whether occupancy info already exists for such a level identifier.
            OccupancyInfo occupancyInfo = null;
            lock (this.occupiedSlotsSyncLock)
            {
                this.occupiedSlots.TryGetValue(identifier, out occupancyInfo);
            }

            if (occupancyInfo == null)
            {
                // It does not, which means that noone is currently occupying any slot of the specified level,
                // therefore, we need to create new OccupancyInfo for it.
                occupancyInfo = new OccupancyInfo(identifier);
                lock (this.occupiedSlotsSyncLock)
                {
                    this.occupiedSlots.Add(identifier, occupancyInfo);
                }

                // No need to check whether slot is free, because we just created new occupancy info, however we still
                // need to check if client is occupying different slot which we need to clean firstly.
                OccupancyInfo oldOccInfo = sender.OccupancyInfo;
                if (oldOccInfo != null)
                {
                    // Client is already occupying a slot, lets clear it firstly.
                    this.ClearSlot(sender, true);
                }
                
                // All done, lets assign occupancy info to client, fill slot and notify others.
                sender.OccupancyInfo = occupancyInfo;
                sender.OccupancyInfo.FillSlot(sender, slot);
                this.broadcastFillSlot(sender, identifier, slot);
            }
            else
            { 
                // OccupancyInfo already exists which means that either someone else has already filled one of slots
                // for the specified level or it was filled in the past but it was not removed from the list.

                // First of all, check whether client is not occupying any other slots already.
                OccupancyInfo oldOccInfo = sender.OccupancyInfo;
                if (oldOccInfo != null)
                {
                    // Client is already occupying a slot, lets clear it firstly.
                    this.ClearSlot(sender, true);
                }

                // Next up check, whether specified slot is free.
                if (occupancyInfo.IsSlotFree(slot))
                { 
                    // It is, we can finally assign occupancy info, fill slot and notify others.
                    sender.OccupancyInfo = occupancyInfo;
                    sender.OccupancyInfo.FillSlot(sender, slot);
                    this.broadcastFillSlot(sender, identifier, slot);
                }
            }
        }

        /// <summary>
        /// Handler, when client confirms his slot.
        /// </summary>
        /// <param name="sender">Client who confirmed slot.</param>
        public void ConfirmSlot(ConnectedClient sender)
        {
            // Check, whether is client is actually occupying slot.
            if (sender.OccupancyInfo != null)
            { 
                // He is, lets confirm his slot.
                Int32? slotId = sender.OccupancyInfo.ConfirmSlot(sender);
                if (slotId != null)
                {
                    // Slot confirmed, notify other members.
                    this.broadcastConfirmSlot(sender.OccupancyInfo.LevelIdentifier, slotId ?? -1);
                    this.checkStartGame(sender.OccupancyInfo);
                }
            }
        }

        /// <summary>
        /// Handler, when client requests to be removed from slot which he is occupying.
        /// This method firstly removes client from his slot if it manages to find any and
        /// clear slot notification will be sent to this right room members afterwards.
        /// </summary>
        /// <param name="sender">Client who requested to clear his slot.</param>
        /// <param name="checkStartGame">Flag indicating whether start game check should be performed once client will be removed from his slot.</param>
        public void ClearSlot(ConnectedClient sender, Boolean checkStartGame)
        {
            // Check whether client is already occupying slot.
            if (sender.OccupancyInfo != null)
            { 
                // He is, lets try to remove him from his slot.
                Int32? removedSlotId = sender.OccupancyInfo.ClearSlot(sender);
                if (removedSlotId != null)
                { 
                    // Success, lets notify everyone in this room about slot clearance.
                    this.broadcastClearSlot(sender.OccupancyInfo.LevelIdentifier, removedSlotId ?? -1);
                    // Lets check whether it is possible to start game or not.
                    if (checkStartGame) this.checkStartGame(sender.OccupancyInfo);
                    sender.OccupancyInfo = null;
                }
            }
        }

        /// <summary>
        /// Checks whether it is possible to start game of specified occupancy info.
        /// If it is, initialize and start new race.
        /// </summary>
        /// <param name="occupancyInfo">Occupancy info to check.</param>
        private void checkStartGame(OccupancyInfo occupancyInfo)
        {
            if (occupancyInfo.CanStartGame())
            {
                // We can start a new game, lets obtain list of clients.
                List<ConnectedClient> clients = occupancyInfo.GetClients();
                // Clear appropriate slots and remove clients from this right room.
                foreach (ConnectedClient cli in clients)
                {
                    this.ClearSlot(cli, false);
                    this.RemoveClientFromRightRoom(cli);
                }

                Race newRace = new Race(occupancyInfo.LevelIdentifier, clients);
                newRace.Initialize();
                newRace.StartRace();
            }
        }

        /// <summary>
        /// [USELESS] Checks whether specified client is occupying any slots in this room.
        /// </summary>
        /// <param name="sender">Client to check.</param>
        /// <returns>OccupancyInfo reference which client is occupying or null if it is not occupying any.</returns>
        private OccupancyInfo getOccupancyInfoByClient(ConnectedClient client)
        {
            lock (this.occupiedSlotsSyncLock)
            {
                foreach (OccupancyInfo occupancyInfo in this.occupiedSlots.Values)
                {
                    if (occupancyInfo.ContainsClient(client)) return occupancyInfo;
                }
            }

            return null;
        }

        #region Methods used for notifying members about slot changes (fill, confirm and clear).

        /// <summary>
        /// This method firstly sends message to player who filled the slot so that it succeeded.
        /// Next up, remaining members of this room will be notified that this slot has been filled.
        /// </summary>
        /// <param name="sender">Client who filled the specified slot.</param>
        /// <param name="identifier">Occupancy info (level) identifier in levelID_version format.</param>
        /// <param name="slot">Slot number.</param>
        private void broadcastFillSlot(ConnectedClient sender, String identifier, Int32 slot)
        {
            // First off notify the sender that he successfuly filled slot.
            sender.SendMessage(String.Format("fillSlot{0}`{1}`{2}`{3}`me", identifier, slot, sender.AccData.Username, sender.AccData.Rank));

            // Next up notify other members of this room that the slot has been filled.
            lock (this.membersSyncLock)
            {
                // TODO - Use LINQ?
                foreach (Int32 clientLID in this.members.Keys)
                {
                    if (sender.LoginID != clientLID)
                    {
                        this.members[clientLID].SendMessage(String.Format("fillSlot{0}`{1}`{2}`{3}", identifier, slot, sender.AccData.Username, sender.AccData.Rank));
                    }
                }
            }
        }

        /// <summary>
        /// Sends confirm slot notification to every single member of this right room.
        /// </summary>
        /// <param name="identifier">Occupancy info (level) identifier in levelID_version format.</param>
        /// <param name="slot">Slot number.</param>
        private void broadcastConfirmSlot(String identifier, Int32 slot)
        {
            lock (this.membersSyncLock)
            {
                foreach (Int32 clientLID in this.members.Keys)
                {
                    this.members[clientLID].SendMessage(String.Format("confirmSlot{0}`{1}", identifier, slot));
                }
            }
        }

        /// <summary>
        /// Sends clear slot message to every single member of this right room.
        /// </summary>
        /// <param name="levelIdentifier">Level identifier in levelId_version format.</param>
        /// <param name="removedSlotId">Slot which has been cleared.</param>
        private void broadcastClearSlot(String levelIdentifier, Int32 removedSlotId)
        {
            lock (this.membersSyncLock)
            {
                foreach (Int32 clientLId in this.members.Keys)
                {
                    this.members[clientLId].SendMessage(String.Format("clearSlot{0}`{1}", levelIdentifier, removedSlotId));
                }
            }
        }

        /// <summary>
        /// Sends info about all slots in this room to the client.
        /// Useful when client joins the room.
        /// </summary>
        /// <param name="client">Client who will receive info about slots.</param>
        private void sendMeSlotsStatus(ConnectedClient client)
        {
            lock (this.occupiedSlotsSyncLock)
            {
                foreach (String levelIdentifier in this.occupiedSlots.Keys)
                {
                    this.occupiedSlots[levelIdentifier].SendMeSlotsStatus(client);
                }
            }
        }

        #endregion
    }
}
