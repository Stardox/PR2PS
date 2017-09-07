using System;
using static PR2PS.Common.Enums;

namespace PR2PS.Web.Core.FormModels
{
    public class UploadLevelFormModel
    {
        public String Title { get; set; }
        public String Note { get; set; }
        public String Credits { get; set; }
        public Boolean Live { get; set; }
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
        public Boolean HasPass { get; set; }
        public String Token { get; set; }
        public String Rand { get; set; }
    }
}
