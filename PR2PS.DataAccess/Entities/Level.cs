using PR2PS.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PR2PS.DataAccess.Entities
{
    public class Level : BaseEntity
    {
        public Int64 AuthorId { get; set; }

        public String Title { get; set; }

        public Boolean IsDeleted { get; set; }

        public Boolean IsPublished { get; set; }

        public virtual ICollection<LevelVersion> Versions { get; set; }
        public virtual ICollection<LevelVote> Votes { get; set; }
        public virtual ICollection<LevelPlay> Plays { get; set; }

        public Level()
        {
            this.Versions = new List<LevelVersion>();
            this.Votes = new List<LevelVote>();
            this.Plays = new List<LevelPlay>();
        }
        
        public LevelDataDTO ToLevelDataDTO(LevelVersion version, Int32 versionNumber)
        {
            return new LevelDataDTO
            {
                Id = this.Id,
                Version = versionNumber,
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
                PassHash = version.PassHash,
                Data = version.Data,
                CowboyChance = version.CowboyChance,
                SubmittedDate = version.SubmittedDate
            };
        }

        public static Expression<Func<Level, LevelRowDTO>> ToLevelRowDTO = (level) => new LevelRowDTO
        {
            LevelId = level.Id,
            Version = level.Versions.Count,
            Title = level.Title,
            IsPublished = level.IsPublished,
            UserlId = level.AuthorId
        };
    }
}
