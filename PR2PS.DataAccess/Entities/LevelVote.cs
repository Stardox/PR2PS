using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR2PS.DataAccess.Entities
{
    public class LevelVote : BaseEntity
    {
        [InverseProperty("LevelVotes")]
        public virtual Account Voter { get; set; }

        public String VoterIP { get; set; }

        public Int64 LevelId { get; set; }

        public Byte Vote { get; set; }

        public DateTime VoteDate { get; set; }
    }
}
