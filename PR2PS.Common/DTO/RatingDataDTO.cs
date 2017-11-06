using PR2PS.Common.Constants;
using System;
using System.Globalization;

namespace PR2PS.Common.DTO
{
    public class RatingDataDTO
    {
        public Byte Vote { get; set; }
        public Double OldRating { get; set; }
        public Double NewRating { get; set; }

        public override string ToString()
        {
            return String.Format(StatusMessages.VOTE_CAST,
                this.Vote,
                this.OldRating == 0 ? StatusMessages.NONE : this.OldRating.ToString(StringFormat.DECIMAL_TWO, CultureInfo.InvariantCulture),
                this.NewRating.ToString(StringFormat.DECIMAL_TWO, CultureInfo.InvariantCulture));
        }
    }
}
