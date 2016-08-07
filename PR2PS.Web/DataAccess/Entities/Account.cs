using PR2PS.Common.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR2PS.Web.DataAccess.Entities
{
    public class Account : BaseEntity
    {
        public String Username { get; set; }
        public String PasswordHash { get; set; }
        public String Email { get; set; }
        public Byte Group { get; set; }
        public DateTime RegisterDate { get; set; }
        public String RegisterIP { get; set; }
        public DateTime LoginDate { get; set; }
        public String LoginIP { get; set; }
        public String Status { get; set; }

        [Required, Index(IsUnique = true), ForeignKey("CustomizeInfo")]
        public Int64 CustomizeInfoId { get; set; }
        public virtual CustomizeInfo CustomizeInfo { get; set; }

        [Required, Index(IsUnique = true), ForeignKey("Experience")]
        public Int64 ExperienceId { get; set; }
        public virtual Experience Experience { get; set; }

        public virtual ICollection<Ban> Bans { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public Account()
        {
            this.Group = 1;
            this.RegisterDate = DateTime.UtcNow;
            this.LoginDate = DateTime.UtcNow;
            this.Status = StatusMessages.STR_OFFLINE;

            //this.Bans = new List<Ban>();
            //this.Messages = new List<Message>();
        }
    }
}
