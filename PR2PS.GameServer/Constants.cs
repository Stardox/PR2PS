using System;
using System.Collections.Generic;

namespace PR2PS.GameServer
{
    public static class GameConstants
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

        static GameConstants()
        {
            GameModeMap = new Dictionary<String, GameMode>();
            GameModeMap.Add(MODE_RACE, GameMode.RACE);
            GameModeMap.Add(MODE_DEATHMATCH, GameMode.DEATHMATCH);
            GameModeMap.Add(MODE_OBJECTIVE, GameMode.OBJECTIVE);
            GameModeMap.Add(MODE_EGG, GameMode.EGG);
        }
    }
}
