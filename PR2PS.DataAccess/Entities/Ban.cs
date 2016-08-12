using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR2PS.DataAccess.Entities
{
    public class Ban : BaseEntity
    {
        [InverseProperty("Bans")]
        public virtual Account Receiver { get; set; }
        public virtual Account Issuer { get; set; }
        public String IPAddress { get; set; }
        public Boolean IsIPBan { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public String Reason { get; set; }
        public String Extra { get; set; }
    }
}
