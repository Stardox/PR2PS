using PR2PS.Common.Constants;
using System;

namespace PR2PS.Common.DTO
{
    public class AccountDataDTO
    {
        public Int64 UserId { get; set; }
        public String Username { get; set; }
        public Byte Group { get; set; }

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

        // TODO - Experience points, guild, etc.

        public Int32 GetRemainingStats()
        {
            return (150 + this.Rank - this.Speed - this.Accel - this.Jump);
        }

        public override String ToString()
        {
            return String.Join(
                Separators.ARG_STR,
                this.HatColor.ToString(),
                this.HeadColor.ToString(),
                this.BodyColor.ToString(),
                this.FeetColor.ToString(),
                this.Hat.ToString(),
                this.Head.ToString(),
                this.Body.ToString(),
                this.Feet.ToString(),
                this.HatSeq ?? string.Empty,
                this.HeadSeq ?? string.Empty,
                this.BodySeq ?? string.Empty,
                this.FeetSeq ?? string.Empty,
                this.Speed.ToString(),
                this.Accel.ToString(),
                this.Jump.ToString(),
                this.Rank.ToString(),
                this.UsedRankTokens.ToString(),
                this.ObtainedRankTokens.ToString(),
                this.HatColor2.ToString(),
                this.HeadColor2.ToString(),
                this.BodyColor2.ToString(),
                this.FeetColor2.ToString(),
                this.HatSeqEpic ?? string.Empty,
                this.HeadSeqEpic ?? string.Empty,
                this.BodySeqEpic ?? string.Empty,
                this.FeetSeqEpic ?? string.Empty);
        }
    }
}
