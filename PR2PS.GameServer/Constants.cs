using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR2PS.GameServer
{
    public static class Constants
    {
        /// <summary>
        /// Policy file XML request message.
        /// </summary>
        public const String POLICY_FILE_XML_REQUEST = "<policy-file-request/>";
        /// <summary>
        /// Policy file XML which will be sent on request.
        /// </summary>
        public const String POLICY_FILE_XML_RESPONSE = "<?xml version=\"1.0\"?><!DOCTYPE cross-domain-policy SYSTEM \"/xml/dtds/cross-domain-policy.dtd\"><!-- Policy file for xmlsocket://socks.example.com --><cross-domain-policy><!-- This is a master socket policy file --><!-- No other socket policies on the host will be permitted --><site-control permitted-cross-domain-policies=\"master-only\"/><allow-access-from domain=\"*\" to-ports=\"9000-10000\" /></cross-domain-policy>\0";
        /// <summary>
        /// Regular expression used for message format verification.
        /// </summary>
        public const String PACKET_REGEX = "[a-f0-9]{3}`[0-9]+`.*\x04";

        public const String RPC_REGISTER = "RegisterServer";
        public const String RPC_ALIVE = "ServerAlive";

        public const Double KEEPALIVE_INTERVAL = 120000;

        public const Char EOT_CHAR = '\x04';
        public const Char ARG_CHAR = '`';
        public const Char COMMA_CHAR = ',';
        public const Char PERIOD_CHAR = '.';
        public static Char[] UNDERSCORE_SEPARATOR = new Char[] { '_' };
        public static Char[] COMMA_SEPARATOR = new Char[] { ',' };
        public static Char[] ARG_SEPARATOR = new Char[] { '`' };
        public static Char[] EOT_SEPARATOR = new Char[] { EOT_CHAR };

        public const String NAH = "nah";

        public const String ROOM_NONE = "none";
        public const String ROOM_MAIN = "main";
        public const String ROOM_CAMPAIGN = "campaign";
        public const String ROOM_ALLTIMEBEST = "best";
        public const String ROOM_TODAYSBEST = "best_today";
        public const String ROOM_SEARCH = "search";

        public const String FORFEIT = "forfeit";
        public const String ONE = "1";

        public const String PARTS_ALL_HATS = "1,2,3,4,5,6,7,8,9,10,11,12,13,14";
        public const String PARTS_ALL_HEADS = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39";
        public const String PARTS_ALL_BODIES = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39";
        public const String PARTS_ALL_FEET = "1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,36,37,38,39";

        public enum GameMode
        {
            UNKNOWN,
            RACE,
            DEATHMATCH,
            OBJECTIVE,
            EGG
        }

        public const String MODE_RACE = "race";
        public const String MODE_DEATHMATCH = "deathmatch";
        public const String MODE_OBJECTIVE = "objective";
        public const String MODE_EGG = "egg";
        public static Dictionary<String, GameMode> GameModeMap;

        static Constants()
        {
            GameModeMap = new Dictionary<String, GameMode>();
            GameModeMap.Add(MODE_RACE, GameMode.RACE);
            GameModeMap.Add(MODE_DEATHMATCH, GameMode.DEATHMATCH);
            GameModeMap.Add(MODE_OBJECTIVE, GameMode.OBJECTIVE);
            GameModeMap.Add(MODE_EGG, GameMode.EGG);
        }
    }
}
