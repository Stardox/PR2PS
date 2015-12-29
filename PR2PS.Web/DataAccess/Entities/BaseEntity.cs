using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR2PS.Web.DataAccess.Entities
{
    public abstract class BaseEntity
    {
        [KeyAttribute]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }
    }
}
