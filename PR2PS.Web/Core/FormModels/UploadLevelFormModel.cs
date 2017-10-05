using PR2PS.Common.DTO;
using System;
using static PR2PS.Common.Enums;

namespace PR2PS.Web.Core.FormModels
{
    public class UploadLevelFormModel
    {
        public String Title { get; set; }
        public String Note { get; set; }
        public String Credits { get; set; }
        public Byte Live { get; set; }
        public GameMode GameMode { get; set; }
        public Byte Min_Level { get; set; }
        public Single Gravity { get; set; }
        public UInt16 Song { get; set; }
        public String Items { get; set; }
        public UInt16 Max_Time { get; set; }
        public String Hash { get; set; }
        public String PassHash { get; set; }
        public String Data { get; set; }
        public Byte CowboyChance { get; set; }
        public Byte HasPass { get; set; }
        public String Token { get; set; }
        public String Rand { get; set; }

        public LevelDataDTO ToDTO()
        {
            return new LevelDataDTO
            {
                Title = this.Title,
                Note = this.Note,
                Live = this.Live != 0,
                GameMode = this.GameMode,
                MinLevel = this.Min_Level,
                Gravity = this.Gravity,
                Song = this.Song,
                Items = this.Items,
                MaxTime = this.Max_Time,
                Hash = this.Hash,
                PassHash = this.PassHash,
                Data = this.Data,
                CowboyChance = this.CowboyChance
            };
        }
    }
}
