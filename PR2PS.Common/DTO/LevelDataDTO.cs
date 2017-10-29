﻿using System;
using static PR2PS.Common.Enums;

namespace PR2PS.Common.DTO
{
    public class LevelDataDTO
    {
        public Int64 Id { get; set; }
        public Int64 UserId { get; set; }
        public String Title { get; set; }
        public String Note { get; set; }
        public Boolean Live { get; set; }
        public GameMode GameMode { get; set; }
        public Byte MinLevel { get; set; }
        public Single Gravity { get; set; }
        public Int16 Song { get; set; }
        public String Items { get; set; }
        public Int16 MaxTime { get; set; }
        public String Hash { get; set; }
        public String PassHash { get; set; }
        public String Data { get; set; }
        public Byte CowboyChance { get; set; }
    }
}
