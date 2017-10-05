using System;
using static PR2PS.Common.Enums;

namespace PR2PS.Common.DTO
{
    public class LevelDataDTO
    {
        public String Title { get; set; }
        public String Note { get; set; }
        public Boolean Live { get; set; }
        public GameMode GameMode { get; set; }
        public Byte MinLevel { get; set; }
        public Single Gravity { get; set; }
        public UInt16 Song { get; set; }
        public String Items { get; set; }
        public UInt16 MaxTime { get; set; }
        public String Hash { get; set; }
        public String PassHash { get; set; }
        public String Data { get; set; }
        public Byte CowboyChance { get; set; }
    }
}
