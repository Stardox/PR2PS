using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PR2PS.Common.Enums;

namespace PR2PS.DataAccess.Entities
{
    public class Level : BaseEntity
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Version { get; set; }

        public Int64 AuthorId { get; set; }

        public String AuthorIP { get; set; }

        public DateTime CreatedDate { get; set; }

        public Boolean IsDeleted { get; set; }

        public Boolean IsPublished { get; set; }

        public String Title { get; set; }

        public String Note { get; set; }
        
        public GameMode GameMode { get; set; }

        public Byte MinRank { get; set; }

        public Byte CowboyChance { get; set; }

        public Single Gravity { get; set; }

        public UInt16 Song { get; set; }

        public UInt16 MaxTime { get; set; }

        public String Items { get; set; }

        // TODO - How about compressing this?
        public String Data { get; set; }

        public String Hash { get; set; }

        public String PassHash { get; set; }
    }
}
