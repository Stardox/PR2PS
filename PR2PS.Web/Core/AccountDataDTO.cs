using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PR2PS.Web.Core
{
    /// <summary>
    /// Serves as account model wrapper.
    /// </summary>
    public class AccountDataDTO
    {
        // Account.
        public Int64    UserId      { get; set; }
        public String   Username    { get; set; }
        public Int32    Group       { get; set; }

        // Customize info.
        public Int32    Hat         { get; set; }
        public Int32    Head        { get; set; }
        public Int32    Body        { get; set; }
        public Int32    Feet        { get; set; }
        public Int32    HatColor    { get; set; }
        public Int32    HeadColor   { get; set; }
        public Int32    BodyColor   { get; set; }
        public Int32    FeetColor   { get; set; }
        public Int32    HatColor2   { get; set; }
        public Int32    HeadColor2  { get; set; }
        public Int32    BodyColor2  { get; set; }
        public Int32    FeetColor2  { get; set; }
        public String   HatSeq      { get; set; }
        public String   HeadSeq     { get; set; }
        public String   BodySeq     { get; set; }
        public String   FeetSeq     { get; set; }
        public String   HatSeqEpic  { get; set; }
        public String   HeadSeqEpic { get; set; }
        public String   BodySeqEpic { get; set; }
        public String   FeetSeqEpic { get; set; }
        public Int32    Speed       { get; set; }
        public Int32    Accel       { get; set; }
        public Int32    Jump        { get; set; }
        public Int32    Rank        { get; set; }
        public Int32    UsedRankTokens      { get; set; }
        public Int32    ObtainedRankTokens  { get; set; }

        // TODO - Experience points, guild, etc.

        public Int32 GetRemainingStats()
        {
            return (150 + this.Rank - this.Speed - this.Accel - this.Jump);
        }

        public override String ToString()
        {
            StringBuilder strBuilder = new StringBuilder();
            strBuilder.Append(this.HatColor);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.HeadColor);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.BodyColor);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.FeetColor);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.Hat);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.Head);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.Body);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.Feet);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.HatSeq);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.HeadSeq);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.BodySeq);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.FeetSeq);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.Speed);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.Accel);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.Jump);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.Rank);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.UsedRankTokens);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.ObtainedRankTokens);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.HatColor2);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.HeadColor2);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.BodyColor2);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.FeetColor2);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.HatSeqEpic);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.HeadSeqEpic);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.BodySeqEpic);
            strBuilder.Append(Constants.ARG_CHAR);
            strBuilder.Append(this.FeetSeqEpic);

            return strBuilder.ToString();
        }
    }
}
