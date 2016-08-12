using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR2PS.DataAccess.Entities
{
    public class Message : BaseEntity
    {
        [InverseProperty("Messages")]
        public virtual Account Recipient { get; set; }

        public virtual Account Sender { get; set; }

        public String Content { get; set; }

        public Int64 DateSent { get; set; }

        public String IPAddress { get; set; }

        public Boolean IsDeleted { get; set; }
    }
}
