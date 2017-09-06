using PR2PS.Common.Constants;
using System.ComponentModel;

namespace PR2PS.Common
{
    public class Enums
    {
        public enum GameMode
        {
            [Description(GameModes.UNKNOWN)]
            Unknown = 0,

            [Description(GameModes.RACE)]
            Race = 1,

            [Description(GameModes.DEATHMATCH)]
            Deathmatch = 2,

            [Description(GameModes.EGG)]
            Egg = 3,

            [Description(GameModes.OBJECTIVE)]
            Objective = 4
        }
    }
}
