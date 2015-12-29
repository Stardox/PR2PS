using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

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
            return String.Concat(Constants.NAH, Constants.ARG_CHAR, packetWithoutHash);
        }
    }
}
