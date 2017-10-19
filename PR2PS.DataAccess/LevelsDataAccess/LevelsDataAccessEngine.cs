using PR2PS.Common.Constants;
using PR2PS.Common.DTO;
using PR2PS.Common.Exceptions;
using PR2PS.DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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

        public void SaveLevel(Int64 userId, LevelDataDTO levelData, String ipAddress)
        {
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

            // TODO - Validation of items.
            // TODO - Validation of level data.
            // TODO - Validation of hash.
            // TODO - Validation of password hash.
            // TODO - Consider the difference based approach when saving levels.

            Level level = this.dbContext.Levels
                                        .Include(l => l.Versions)
                                        .FirstOrDefault(l => l.AuthorId == userId && l.Title == levelData.Title);
            DateTime utcDateTime = DateTime.UtcNow; // To prevent usage of SQL functions.

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
                SubmittedDate = utcDateTime,
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
                Hash = levelData.Hash,
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
                .Select(l => new LevelRowDTO
                {
                    LevelId = l.Id,
                    Version = l.Versions.Count,
                    Title = l.Title,
                    IsPublished = l.IsPublished,
                    UserlId = l.AuthorId
                })
                .ToList();

            foreach (LevelRowDTO levelRow in levelRows)
            {
                LevelVersion latest = this.dbContext.LevelVersions
                    .OrderByDescending(v => v.Id)
                    .FirstOrDefault(v => v.Level.Id == levelRow.LevelId);

                levelRow.Note = latest?.Note;
                levelRow.MinRank = latest?.MinRank ?? 0;
                levelRow.HasPass = !string.IsNullOrEmpty(latest?.PassHash);
                levelRow.GameMode = latest?.GameMode ?? GameMode.Unknown;
            }

            return levelRows;
        }
    }
}
