using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR2PS.DataAccess.Entities
{
    public class LevelVote : BaseEntity
    {
        [InverseProperty("Votes")]
        public virtual Level Level { get; set; }

        public Int64 UserId { get; set; }

        public String VoterIP { get; set; }

        public Byte Vote { get; set; }

        public DateTime VoteDate { get; set; }
    }
}
