using PR2PS.Common.DTO;
using System;
using System.Collections.Generic;
using static PR2PS.Common.Enums;

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

        /// <summary>
        /// Gets level data.
        /// </summary>
        /// <param name="levelId">Id of the level.</param>
        /// <param name="versionNum">Version of the level. If not provided then latest version will be loaded.</param>
        /// <returns>Actual level data or exception if level with specified id can not be found.</returns>
        LevelDataDTO GetLevel(Int64 levelId, Int32 versionNum);

        /// <summary>
        /// Marks level as deleted.
        /// </summary>
        /// <param name="userId">Id of level author.</param>
        /// <param name="levelId">Id of the level.</param>
        void SoftDeleteLevel(Int64 userId, Int64 levelId);


        /// <summary>
        /// Seaches through levels belonging to specified user based on search criteria.
        /// </summary>
        /// <param name="userId">Id of user who submitted the levels.</param>
        /// <param name="order">Search order (date, alphabet, etc).</param>
        /// <param name="dir">Search direction.</param>
        /// <param name="page">Specifies desired page of levels to return. There are six levels per page.</param>
        /// <returns>Levels created by specified user.
        /// Note, that not all meta data are filled in as they are not available in this database context.</returns>
        List<LevelRowDTO> SearchLevelsByUserId(Int64 userId, SearchOrder order, SearchDirection dir, Int16? page);

        /// <summary>
        /// Seaches through levels based on search term and criteria.
        /// </summary>
        /// <param name="userId">Id of user who submitted the levels.</param>
        /// <param name="order">Search order (date, alphabet, etc).</param>
        /// <param name="dir">Search direction.</param>
        /// <param name="page">Specifies desired page of levels to return. There are six levels per page.</param>
        /// <returns>Levels satisfying search term.
        /// Note, that not all meta data are filled in as they are not available in this database context.</returns>
        List<LevelRowDTO> SearchLevelsByTerm(String term, SearchOrder order, SearchDirection dir, Int16? page);
    }
}
