using PR2PS.Common.Constants;
using PR2PS.Common.DTO;
using PR2PS.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static PR2PS.Common.Enums;

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

        public static Level FromString(String data, Int64 authorId)
        {
            if (String.IsNullOrEmpty(data) || data.Length < 32)
            {
                return null;
            }

            // Trim out the checksum.
            data = data.Substring(0, data.Length - 32);

            IEnumerable<KeyValuePair<String, String>> attributes = data
                .Split(Separators.AMPERSAND_SEPARATOR, StringSplitOptions.RemoveEmptyEntries)
                .Where(g => g.Contains(Separators.EQ_CHAR))
                .Select(g => new KeyValuePair<String, String>(g.Split(Separators.EQ_CHAR)[0], g.Split(Separators.EQ_CHAR)[1]));

            Level level = new Level
            {
                AuthorId = authorId,
                IsDeleted = false,
                IsPublished = true,
                Title = String.Concat(DateTime.UtcNow.GetSecondsSinceUnixTime(), DateTime.UtcNow.Millisecond)
            };

            LevelVersion version = new LevelVersion
            {
                Note = String.Empty,
                GameMode = GameMode.Race,
                MinRank = 0,
                MaxTime = 600,
                Gravity = 1,
                Song = 1,
                CowboyChance = 0,
                Items = String.Empty,
                Data = String.Empty,
                PassHash = String.Empty,
                SubmittedDate = DateTime.UtcNow.AddSeconds(-60),
                SubmittedIP = String.Empty,
                Level = level
            };

            level.Versions.Add(version);

            foreach (KeyValuePair<String, String> attribute in attributes)
            {
                // .NET reflection anyone? Noone? Ok...

                String value = attribute.Value;

                switch (attribute.Key)
                {
                    case LevelDataKeys.TITLE:
                        level.Title = value;
                        break;

                    case LevelDataKeys.NOTE:
                        version.Note = value;
                        break;

                    case LevelDataKeys.MIN_LEVEL:
                        Byte minRankParsed;
                        if (Byte.TryParse(value, out minRankParsed))
                        {
                            version.MinRank = minRankParsed;
                        }
                        break;

                    case LevelDataKeys.SONG:
                        Int16 songParsed;
                        if (Int16.TryParse(value, out songParsed))
                        {
                            version.Song = songParsed;
                        }
                        break;

                    case LevelDataKeys.GRAVITY:
                        Single gravityParsed;
                        if (Single.TryParse(value, out gravityParsed))
                        {
                            version.Gravity = gravityParsed;
                        }
                        break;

                    case LevelDataKeys.MAX_TIME:
                        Int16 maxTimeParsed;
                        if (Int16.TryParse(value, out maxTimeParsed))
                        {
                            version.MaxTime = maxTimeParsed;
                        }
                        break;

                    case LevelDataKeys.COWBOY_CHANCE:
                        Byte cowboyChanceParsed;
                        if (Byte.TryParse(value, out cowboyChanceParsed))
                        {
                            version.CowboyChance = cowboyChanceParsed;
                        }
                        break;

                    case LevelDataKeys.LIVE:
                        Byte liveParsed;
                        if (Byte.TryParse(value, out liveParsed))
                        {
                            level.IsPublished = Convert.ToBoolean(liveParsed);
                        }
                        break;

                    case LevelDataKeys.ITEMS:
                        version.Items = value;
                        break;

                    case LevelDataKeys.GAME_MODE:
                        GameMode gameModeParsed = attribute.Value.FromEnumDescription(GameMode.Unknown);
                        if (gameModeParsed != GameMode.Unknown)
                        {
                            version.GameMode = gameModeParsed;
                        }
                        break;

                    case LevelDataKeys.DATA:
                        version.Data = attribute.Value;
                        break;
                }
            }

            return level;
        }
    }
}
