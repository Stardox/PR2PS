using PR2PS.Common.DTO;
using System;

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
    }
}
