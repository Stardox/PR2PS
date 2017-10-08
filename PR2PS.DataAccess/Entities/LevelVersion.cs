using System;
using static PR2PS.Common.Enums;

namespace PR2PS.DataAccess.Entities
{
    public class LevelVersion : BaseEntity
    {
        public virtual Level Level { get; set; }

        public DateTime SubmittedDate { get; set; }

        public String SubmittedIP { get; set; }

        public String Note { get; set; }

        public GameMode GameMode { get; set; }

        public Byte MinRank { get; set; }

        public Byte CowboyChance { get; set; }

        public Single Gravity { get; set; }

        public Int16 Song { get; set; }

        public Int16 MaxTime { get; set; }

        public String Items { get; set; }

        // TODO - How about compressing this?
        public String Data { get; set; }

        public String Hash { get; set; }

        public String PassHash { get; set; }
    }
}
