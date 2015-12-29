using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR2PS.GameServer
{
    public class OccupancyInfo
    {
        /// <summary>
        /// Level occupancy identifier. Usual format is levelID_version.
        /// </summary>
        public String LevelIdentifier { get; private set; }
        /// <summary>
        /// Information about slots for specific level.
        /// List is used to preserve insertion order which will be used for determining order at race start.
        /// TODO - Use better collection (OrderedDictionary for example).
        /// </summary>
        private List<SlotInfo> slots;
        private Object slotsSyncLock;

        public OccupancyInfo(String levelIdentifier)
        {
            this.LevelIdentifier = levelIdentifier;
            this.slots = new List<SlotInfo>();
            this.slotsSyncLock = new Object();
        }

        /// <summary>
        /// Creates new slot info, assigns client into it and finally adds it to list of slots for this occupancy info.
        /// </summary>
        /// <param name="sender">Client which will fill the specified slot.</param>
        /// <param name="slot">The slot.</param>
        public void FillSlot(ConnectedClient sender, Int32 slot)
        {
            lock (this.slotsSyncLock)
            {
                this.slots.Add(new SlotInfo(sender, slot, false));
            }
        }

        /// <summary>
        /// Finds slots based on the client and confirms it.
        /// </summary>
        /// <param name="sender">Client who requested to confirm his slot.</param>
        /// <returns>Slot id which client confirmed or null if failed.</returns>
        public Int32? ConfirmSlot(ConnectedClient sender)
        {
            lock (this.slotsSyncLock)
            {
                SlotInfo slotInfo = this.slots.Find(slot => slot.Client == sender);
                if (slotInfo == null)
                {
                    // Specific situation which should not occur. No slot info found for specified client.
                    return null;
                }
                else
                {
                    slotInfo.ConfirmSlot();
                    return slotInfo.Slot;
                }
            }
        }

        /// <summary>
        /// Clears slot which specified client is occupying.
        /// </summary>
        /// <param name="sender">Client who is going to be removed from his slot.</param>
        /// <returns>Slot id from which client has been removed or null if failed to remove.</returns>
        public Int32? ClearSlot(ConnectedClient sender)
        {
            lock (this.slotsSyncLock)
            {
                SlotInfo slotInfo = this.slots.Find(slot => slot.Client == sender);
                if (slotInfo == null)
                {
                    // Specific situation which should not occur. No such client found in one of slots for this level.
                    return null;
                }
                else
                {
                    this.slots.Remove(slotInfo);
                    return slotInfo.Slot;
                }
            }
        }

        /// <summary>
        /// [USELESS] Checks whether specified client is occupying any slots for this level.
        /// </summary>
        /// <param name="client">Client to search for.</param>
        /// <returns>True if specified client is occupying one of slots for this level or false otherwise.</returns>
        public Boolean ContainsClient(ConnectedClient client)
        {
            lock (this.slotsSyncLock)
            {
                foreach (SlotInfo slotInfo in this.slots)
                {
                    if (slotInfo.Client == client) return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks whether specified slot is occupied or not.
        /// </summary>
        /// <param name="slotId">Slot id to check.</param>
        /// <returns>True if slot is free (not yet occupied) or false otherwise.</returns>
        public Boolean IsSlotFree(Int32 slotId)
        {
            lock (this.slotsSyncLock)
            {
                foreach (SlotInfo slot in this.slots)
                {
                    if (slot.Slot == slotId) return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Performs check whether it is possible to start game.
        /// Returns true if all slots in this occupancy info are confirmed.
        /// TODO - What if lock does not prevent client from being added?
        /// </summary>
        /// <returns>True if it is possible to start game or false otherwise.</returns>
        public Boolean CanStartGame()
        { 
            Int32 allSlotsCount;
            Int32 confirmedSlotsCount;

            lock (this.slotsSyncLock)
            {
                allSlotsCount = this.slots.Count;
                confirmedSlotsCount = (this.slots.Where(slot => slot.Confirmed)).Count();
            }

            if (allSlotsCount > 0 && allSlotsCount == confirmedSlotsCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Extract list of clients from all slots in this occupancy info.
        /// Order of the clients should be same as order at which they filled their slots.
        /// </summary>
        /// <returns>List of clients.</returns>
        public List<ConnectedClient> GetClients()
        { 
            lock(this.slotsSyncLock)
            {
                return (from slot in this.slots select slot.Client).ToList<ConnectedClient>();
            }
        }

        /// <summary>
        /// Sends info about all slots in this occupancy info (level) to the client.
        /// </summary>
        /// <param name="client">Client who will receive info about slots.</param>
        public void SendMeSlotsStatus(ConnectedClient client)
        {
            lock (this.slotsSyncLock)
            {
                foreach (SlotInfo slot in this.slots)
                {
                    client.SendMessage(String.Format("fillSlot{0}`{1}`{2}`{3}", this.LevelIdentifier, slot.Slot, slot.Client.AccData.Username, slot.Client.AccData.Rank));

                    if (slot.Confirmed)
                    {
                        client.SendMessage(String.Format("confirmSlot{0}`{1}", this.LevelIdentifier, slot.Slot));
                    }
                }
            }
        }
    }

    /// <summary>
    /// Class used for grouping up information about level slot.
    /// </summary>
    public class SlotInfo
    {
        public ConnectedClient Client { get; private set; }
        public Int32 Slot { get; private set; }
        public Boolean Confirmed { get; private set; }

        public SlotInfo(ConnectedClient client, Int32 slot, Boolean confirmed)
        {
            this.Client = client;
            this.Slot = slot;
            this.Confirmed = confirmed;
        }

        public void ConfirmSlot()
        {
            this.Confirmed = true;
        }
    }
}
