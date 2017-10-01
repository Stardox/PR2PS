using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR2PS.DataAccess.Entities
{
    public class LevelPlay : BaseEntity
    {
        [InverseProperty("LevelPlays")]
        public virtual Account User { get; set; }

        public Int64 LevelId { get; set; }

        public DateTime PlayDate { get; set; }
    }
}
