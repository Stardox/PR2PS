using System;

namespace PR2PS.DataAccess.Entities
{
    public class CustomizeInfo : BaseEntity
    {
        public Int32 Hat { get; set; }
        public Int32 Head { get; set; }
        public Int32 Body { get; set; }
        public Int32 Feet { get; set; }
        public Int32 HatColor { get; set; }
        public Int32 HeadColor { get; set; }
        public Int32 BodyColor { get; set; }
        public Int32 FeetColor { get; set; }
        public Int32 HatColor2 { get; set; }
        public Int32 HeadColor2 { get; set; }
        public Int32 BodyColor2 { get; set; }
        public Int32 FeetColor2 { get; set; }
        public String HatSeq { get; set; }
        public String HeadSeq { get; set; }
        public String BodySeq { get; set; }
        public String FeetSeq { get; set; }
        public String HatSeqEpic { get; set; }
        public String HeadSeqEpic { get; set; }
        public String BodySeqEpic { get; set; }
        public String FeetSeqEpic { get; set; }
        public Int32 Speed { get; set; }
        public Int32 Accel { get; set; }
        public Int32 Jump { get; set; }
        public Int32 Rank { get; set; }
        public Int32 UsedRankTokens { get; set; }
        public Int32 ObtainedRankTokens { get; set; }

        public CustomizeInfo()
        {
            this.Hat = 1;
            this.Head = 1;
            this.Body = 1;
            this.Feet = 1;
            this.HatColor = 0;
            this.HeadColor = 0;
            this.BodyColor = 0;
            this.FeetColor = 0;
            this.HatColor2 = -1;
            this.HeadColor2 = -1;
            this.BodyColor2 = -1;
            this.FeetColor2 = -1;
            this.HatSeq = "1";
            this.HeadSeq = "1";
            this.BodySeq = "1";
            this.FeetSeq = "1";
            this.HatSeqEpic = "";
            this.HeadSeqEpic = "";
            this.BodySeqEpic = "";
            this.FeetSeqEpic = "";
            this.Speed = 50;
            this.Accel = 50;
            this.Jump = 50;
            this.Rank = 0;
            this.UsedRankTokens = 0;
            this.ObtainedRankTokens = 0;
        }
    }
}
