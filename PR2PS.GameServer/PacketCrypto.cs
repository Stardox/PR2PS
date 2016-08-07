using PR2PS.Common.Constants;
using System;

namespace PR2PS.GameServer
{
    /// <summary>
    /// Class used for hashing and verifying packet checksums.
    /// Implementation has been removed.
    /// </summary>
    public class PacketCrypto
    {
        public Boolean VerifyPacket(String packet)
        {
            return true;
        }

        public String GetPacketWithHash(String packetWithoutHash)
        {
            return String.Concat(StatusMessages.NAH, Separators.ARG_CHAR, packetWithoutHash);
        }
    }
}
