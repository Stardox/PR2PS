using PR2PS.Common.DTO;
using System;
using System.Collections.Generic;

namespace PR2PS.DataAccess.LevelsDataAccess
{
    public interface ILevelsDataAccessEngine : IDisposable
    {
        /// <summary>
        /// Validates and saves the level. New version of level is created if level with same title already exists.
        /// </summary>
        /// <param name="userId">Id of user who submitted the level.</param>
        /// <param name="levelData">Level data to be saved.</param>
        /// <param name="ipAddress">IPv4 address from which the request originates.</param>
        void SaveLevel(Int64 userId, LevelDataDTO levelData, String ipAddress);

        /// <summary>
        /// Gets list of levels created by specified user.
        /// </summary>
        /// <param name="userId">Id of user who submitted the levels.</param>
        /// <returns>Levels created by specified user.
        /// Note, that not all meta data are filled in as they are not available in this database context.</returns>
        List<LevelRowDTO> GetUserLevels(Int64 userId);
    }
}
