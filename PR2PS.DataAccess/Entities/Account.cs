using PR2PS.Common.Constants;
using PR2PS.Common.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PR2PS.DataAccess.Entities
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

        [Required]
        [Index(IsUnique = true)]
        [ForeignKey("CustomizeInfo")]
        public Int64 CustomizeInfoId { get; set; }
        public virtual CustomizeInfo CustomizeInfo { get; set; }

        [Required]
        [Index(IsUnique = true)]
        [ForeignKey("Experience")]
        public Int64 ExperienceId { get; set; }
        public virtual Experience Experience { get; set; }

        public virtual ICollection<Ban> Bans { get; set; }
        public virtual ICollection<Message> Messages { get; set; }

        public Account()
        {
            this.Group = UserGroup.MEMBER;
            this.RegisterDate = DateTime.UtcNow;
            this.LoginDate = DateTime.UtcNow;
            this.Status = StatusMessages.STR_OFFLINE;
        }

        // Questionable approach. Should entity worry about DTO or vice versa?
        public AccountDataDTO ToAccountDataDTO()
        {
            return new AccountDataDTO
            {
                UserId = this.Id,
                Username = this.Username,
                Group = this.Group,
                Hat = this.CustomizeInfo.Hat,
                Head = this.CustomizeInfo.Head,
                Body = this.CustomizeInfo.Body,
                Feet = this.CustomizeInfo.Feet,
                HatColor = this.CustomizeInfo.HatColor,
                HeadColor = this.CustomizeInfo.HeadColor,
                BodyColor = this.CustomizeInfo.BodyColor,
                FeetColor = this.CustomizeInfo.FeetColor,
                HatColor2 = this.CustomizeInfo.HatColor2,
                HeadColor2 = this.CustomizeInfo.HeadColor2,
                BodyColor2 = this.CustomizeInfo.BodyColor2,
                FeetColor2 = this.CustomizeInfo.FeetColor2,
                HatSeq = this.CustomizeInfo.HatSeq,
                HeadSeq = this.CustomizeInfo.HeadSeq,
                BodySeq = this.CustomizeInfo.BodySeq,
                FeetSeq = this.CustomizeInfo.FeetSeq,
                HatSeqEpic = this.CustomizeInfo.HatSeqEpic,
                HeadSeqEpic = this.CustomizeInfo.HeadSeqEpic,
                BodySeqEpic = this.CustomizeInfo.BodySeqEpic,
                FeetSeqEpic = this.CustomizeInfo.FeetSeqEpic,
                Speed = this.CustomizeInfo.Speed,
                Accel = this.CustomizeInfo.Accel,
                Jump = this.CustomizeInfo.Jump,
                Rank = this.CustomizeInfo.Rank,
                UsedRankTokens = this.CustomizeInfo.UsedRankTokens,
                ObtainedRankTokens = this.CustomizeInfo.ObtainedRankTokens
            };
        }
    }
}
