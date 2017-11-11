using PR2PS.Common.Constants;
using PR2PS.Common.Cryptography;
using PR2PS.Common.DTO;
using PR2PS.Common.Exceptions;
using PR2PS.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text.RegularExpressions;
using static PR2PS.Common.Enums;

namespace PR2PS.DataAccess.LevelsDataAccess
{
    public class LevelsDataAccessEngine : ILevelsDataAccessEngine
    {
        private LevelsContext dbContext;

        public LevelsDataAccessEngine(LevelsContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Dispose()
        {
            if (this.dbContext != null)
            {
                this.dbContext.Dispose();
                this.dbContext = null;
            }
        }

        public void SaveLevel(Int64 userId, String username, LevelDataDTO levelData, String ipAddress)
        {
            // TODO - Validation of level data.
            // TODO - Consider the difference based approach when saving levels.

            if (levelData == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_LEVEL_DATA);
            }
            else if (String.IsNullOrEmpty(levelData.Title))
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_LEVEL_TITLE);
            }
            else if (levelData.GameMode == GameMode.Unknown)
            {
                throw new PR2Exception(ErrorMessages.ERR_INVALID_GAME_MODE);
            }
            else if (!String.IsNullOrEmpty(levelData.Items)
                     && !Regex.IsMatch(levelData.Items, ValidationConstraints.LEVEL_ITEMS_PATTERN))
            {
                throw new PR2Exception(ErrorMessages.ERR_INVALID_ITEMS);
            }

            using (MD5Wrapper md5 = new MD5Wrapper())
            {
                String hash = md5.GetHashedString(
                    String.Concat(levelData.Title, username?.ToLower() ?? String.Empty, levelData.Data, Pepper.LEVEL_SAVE));

                if (String.CompareOrdinal(hash, levelData.Hash) != 0)
                {
                    throw new PR2Exception(ErrorMessages.ERR_LEVEL_DATA_HASH_MISMATCH);
                }
            }

            DateTime utcDateMinus2mins = DateTime.UtcNow.AddMinutes(-2); // To prevent usage of SQL functions.
            LevelVersion lastSaved = this.dbContext.LevelVersions
                .Where(v => v.Level != null && v.Level.AuthorId == userId && DateTime.Compare(v.SubmittedDate, utcDateMinus2mins) > 0)
                .OrderByDescending(v => v.SubmittedDate).FirstOrDefault();

            if (lastSaved != null)
            {
                throw new PR2Exception(String.Format(
                    ErrorMessages.ERR_WAIT_BEFORE_SAVING,
                    Math.Round(TimeSpan.FromMinutes(2).Subtract(DateTime.UtcNow.Subtract(lastSaved.SubmittedDate.ToUniversalTime())).TotalSeconds)));
            }

            Level level = this.dbContext.Levels
                                        .Include(l => l.Versions)
                                        .FirstOrDefault(l => l.AuthorId == userId && l.Title == levelData.Title);

            if (level == null)
            {
                level = new Level
                {
                    AuthorId = userId,
                    Title = levelData.Title,
                    IsDeleted = false
                };

                this.dbContext.Levels.Add(level);
            }

            level.IsPublished = levelData.Live;
            level.Versions.Add(new LevelVersion
            {
                SubmittedDate = DateTime.UtcNow,
                SubmittedIP = ipAddress,
                Note = levelData.Note,
                GameMode = levelData.GameMode,
                MinRank = levelData.MinLevel,
                CowboyChance = levelData.CowboyChance,
                Gravity = levelData.Gravity,
                Song = levelData.Song,
                MaxTime = levelData.MaxTime,
                Items = levelData.Items,
                Data = levelData.Data,
                PassHash = levelData.PassHash
            });

            this.dbContext.SaveChanges();
        }

        public List<LevelRowDTO> GetUserLevels(Int64 userId)
        {
            // If only LINQ2SQL for SQLite supported advanced joins. This couldve been one liner.
            // Performance is also questionable.

            List<LevelRowDTO> levelRows = this.dbContext.Levels
                .Where(l => !l.IsDeleted && l.AuthorId == userId && l.Versions.Count > 0)
                .Select(Level.ToLevelRowDTO)
                .ToList();
            FillLevelVersionData(levelRows);

            return levelRows;
        }

        public LevelDataDTO GetLevel(Int64 levelId, Int32 versionNum)
        {
            Level level = this.dbContext.Levels.FirstOrDefault(l => l.Id == levelId && !l.IsDeleted);
            if (level == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_SUCH_LEVEL);
            }

            LevelVersion version = null;
            IQueryable<LevelVersion> vQuery = this.dbContext.LevelVersions.Where(v => v.Level.Id == levelId);
            if (versionNum < 1 || versionNum > vQuery.Count())
            {
                versionNum = vQuery.Count();
                version = vQuery.OrderByDescending(v => v.Id).FirstOrDefault();
            }
            else
            {
                version = vQuery.OrderBy(v => v.Id).Skip(versionNum - 1).Take(1).FirstOrDefault();
            }

            if (version == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_VERSION);
            }

            return level.ToLevelDataDTO(version, versionNum);
        }

        public void SoftDeleteLevel(Int64 userId, Int64 levelId)
        {
            Level level = this.dbContext.Levels.FirstOrDefault(l => l.AuthorId == userId && l.Id == levelId && !l.IsDeleted);
            if (level == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_SUCH_LEVEL);
            }

            level.IsDeleted = true;

            this.dbContext.SaveChanges();
        }

        public List<LevelRowDTO> SearchLevelsByUserId(Int64 userId, SearchOrder order, SearchDirection dir, Int16? page)
        {
            IQueryable<Level> levels = this.dbContext.Levels
                .Where(l => l.AuthorId == userId && !l.IsDeleted && l.IsPublished && l.Versions.Count > 0);
            FilterAndSortLevels(ref levels, order, dir, page);

            List<LevelRowDTO> levelRows = levels.Select(Level.ToLevelRowDTO).ToList();
            FillLevelVersionData(levelRows);

            return levelRows;
        }

        public List<LevelRowDTO> SearchLevelsByTerm(String term, SearchOrder order, SearchDirection dir, Int16? page)
        {
            term = term ?? String.Empty;
            if (term.Length < 3)
            {
                throw new PR2Exception(ErrorMessages.ERR_SEARCH_TERM_TOO_SHORT);
            }

            IQueryable<Level> levels = this.dbContext.Levels
                .Where(l => l.Title != null && l.Title.ToUpper().Contains(term.ToUpper()) && !l.IsDeleted && l.IsPublished && l.Versions.Count > 0);
            FilterAndSortLevels(ref levels, order, dir, page);

            List<LevelRowDTO> levelRows = levels.Select(Level.ToLevelRowDTO).ToList();
            FillLevelVersionData(levelRows);

            return levelRows;
        }

        public RatingDataDTO SaveRating(Int64 levelId, Byte rating, Int64 userId, String ipAddress)
        {
            if (rating < 1 || rating > 5)
            {
                throw new PR2Exception(ErrorMessages.ERR_INVALID_RATING);
            }

            Level level = this.dbContext.Levels.FirstOrDefault(l => l.Id == levelId && !l.IsDeleted);
            if (level == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_SUCH_LEVEL);
            }

            DateTime utcDateTimeMinusWeek = DateTime.UtcNow.AddDays(-7); // To prevent usage of SQL functions.
            if (this.dbContext.LevelVotes.Any(v => v.Level.Id == levelId && (v.UserId == userId || v.VoterIP == ipAddress) && DateTime.Compare(v.VoteDate, utcDateTimeMinusWeek) > 0))
            {
                throw new PR2Exception(ErrorMessages.ERR_ALREADY_VOTED);
            }

            RatingDataDTO ratingData = new RatingDataDTO
            {
                Vote = rating,
                OldRating = this.dbContext.LevelVotes.Where(l => l.Level.Id == levelId).Select(l => l.Vote).DefaultIfEmpty().Average(l => l)
            };

            this.dbContext.LevelVotes.Add(new LevelVote
            {
                Level = level,
                UserId = userId,
                Vote = rating,
                VoteDate = DateTime.UtcNow,
                VoterIP = ipAddress
            });
            this.dbContext.SaveChanges();

            ratingData.NewRating = this.dbContext.LevelVotes.Where(l => l.Level.Id == levelId).Select(l => l.Vote).DefaultIfEmpty().Average(l => l);

            return ratingData;
        }

        public void UnpublishLevel(Int64 levelId)
        {
            Level level = this.dbContext.Levels.FirstOrDefault(l => l.Id == levelId && !l.IsDeleted);
            if (level == null)
            {
                throw new PR2Exception(ErrorMessages.ERR_NO_SUCH_LEVEL);
            }

            level.IsPublished = false;

            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Fills in missing meta data about latest version of levels.
        /// </summary>
        /// <param name="levelRows">Enumeration of levels for which should be data about versions loaded.</param>
        private void FillLevelVersionData(IEnumerable<LevelRowDTO> levelRows)
        {
            foreach (LevelRowDTO levelRow in levelRows)
            {
                LevelVersion latest = this.dbContext.LevelVersions
                    .OrderByDescending(v => v.Id)
                    .FirstOrDefault(v => v.Level.Id == levelRow.LevelId);
                // LongCount is not supported at db level for SQLite.
                Int32 playCount = this.dbContext.LevelPlays.Count(l => l.Level.Id == levelRow.LevelId);
                Double rating = this.dbContext.LevelVotes
                    .Where(l => l.Level.Id == levelRow.LevelId)
                    .Select(l => l.Vote)
                    .DefaultIfEmpty()
                    .Average(l => l);

                levelRow.Note = latest?.Note;
                levelRow.MinRank = latest?.MinRank ?? 0;
                levelRow.HasPass = !String.IsNullOrEmpty(latest?.PassHash);
                levelRow.GameMode = latest?.GameMode ?? GameMode.Unknown;
                levelRow.PlayCount = playCount;
                levelRow.Rating = rating;
            }
        }

        /// <summary>
        /// Applies filtering, sorting and pagination according to search criteria.
        /// </summary>
        /// <param name="levels">Levels query on which the filtering, sorting and pagination will be applied.</param>
        /// <param name="order">Search order (date, alphabet, etc).</param>
        /// <param name="dir">Search direction.</param>
        /// <param name="page">Specifies desired page of levels to return. There are six levels per page.</param>
        private void FilterAndSortLevels(ref IQueryable<Level> levels, SearchOrder order, SearchDirection dir, Int16? page)
        {
            // Rant: SQLite in conjunction with EF is very limited when it comes to joins, thats why some parts of the search
            // functionality are not supported. Another option would be executing raw SQL instead of using LINQ,
            // but it would make some things more complicated (like Skip and Take for example).
            // Search order is therefore implemented as follows:
            // Date - Ordered by level id rather than date of submission of latest version.
            // Alphabetical - Supported.
            // Rating - Unsupported.
            // Popularity - Unsupported.

            if (!page.HasValue || page.Value < 1)
            {
                page = 1;
            }

            if (order == SearchOrder.Date)
            {
                if (dir == SearchDirection.Asc)
                {
                    levels = levels.OrderBy(l => l.Id);
                }
                else if (dir == SearchDirection.Desc)
                {
                    levels = levels.OrderByDescending(l => l.Id);
                }
            }
            else if (order == SearchOrder.Alphabetical)
            {
                if (dir == SearchDirection.Asc)
                {
                    levels = levels.OrderBy(l => l.Title);
                }
                else if (dir == SearchDirection.Desc)
                {
                    levels = levels.OrderByDescending(l => l.Title);
                }
            }
            else if (order == SearchOrder.Rating || order == SearchOrder.Popularity)
            {
                throw new PR2Exception(ErrorMessages.ERR_UNSUPPORTED_SEARCH_ORDER);
            }

            levels = levels.Skip((page.Value - 1) * 6).Take(6);
        }
    }
}
