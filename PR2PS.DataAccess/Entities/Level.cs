using PR2PS.Common.DTO;
using System;
using System.Collections.Generic;

namespace PR2PS.DataAccess.Entities
{
    public class Level : BaseEntity
    {
        public Int64 AuthorId { get; set; }

        public String Title { get; set; }

        public Boolean IsDeleted { get; set; }

        public Boolean IsPublished { get; set; }

        public virtual ICollection<LevelVersion> Versions { get; set; }

        public Level()
        {
            Versions = new List<LevelVersion>();
        }
        
        public LevelDataDTO ToDTO(LevelVersion version)
        {
            return new LevelDataDTO
            {
                Id = this.Id,
                UserId = this.AuthorId,
                Title = this.Title,
                Note = version.Note,
                Live = this.IsPublished,
                GameMode = version.GameMode,
                MinLevel = version.MinRank,
                Gravity = version.Gravity,
                Song = version.Song,
                Items = version.Items,
                MaxTime = version.MaxTime,
                Hash = version.Hash,
                PassHash = version.PassHash,
                Data = version.Data,
                CowboyChance = version.CowboyChance
            };
        }
    }
}
