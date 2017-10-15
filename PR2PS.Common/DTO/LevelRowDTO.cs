using System;
using static PR2PS.Common.Enums;

namespace PR2PS.Common.DTO
{
    public class LevelRowDTO
    {
        public Int64 LevelId { get; set; }
        public Int64 Version { get; set; }
        public String Title { get; set; }
        public String Note { get; set; }
        public Byte MinRank { get; set; }
        public Boolean IsPublished { get; set; }
        public Boolean HasPass { get; set; }
        public GameMode GameMode { get; set; }

        public Int64 UserlId { get; set; }
        public String Username { get; set; }
        public Byte Group { get; set; }
        public Double Rating { get; set; }
        public Int64 PlayCount { get; set; }
    }
}
