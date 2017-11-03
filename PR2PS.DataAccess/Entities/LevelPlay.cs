using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR2PS.DataAccess.Entities
{
    public class LevelPlay : BaseEntity
    {
        [InverseProperty("Plays")]
        public virtual Level Level { get; set; }

        public Int64 UserId { get; set; }

        public DateTime PlayDate { get; set; }
    }
}
