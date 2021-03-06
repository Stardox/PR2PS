﻿using PR2PS.Common.DTO;
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
        /// <param name="username">Username of user who submitted the level.</param>
        /// <param name="levelData">Level data to be saved.</param>
        /// <param name="ipAddress">IPv4 address from which the request originates.</param>
        void SaveLevel(Int64 userId, String username, LevelDataDTO levelData, String ipAddress);

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

        /// <summary>
        /// Saves submitted level rating.
        /// </summary>
        /// <param name="levelId">Id of the level.</param>
        /// <param name="rating">Level vote, expected 1-5.</param>
        /// <param name="userId">Id of user who submitted rating.</param>
        /// <param name="ipAddress">IP address of request from which rating request originates.</param>
        /// <returns></returns>
        RatingDataDTO SaveRating(Int64 levelId, Byte rating, Int64 userId, String ipAddress);

        /// <summary>
        /// Marks level as unpublished.
        /// </summary>
        /// <param name="levelId">Id of the level.</param>
        void UnpublishLevel(Int64 levelId);

        /// <summary>
        /// Gets newest levels specified by page.
        /// </summary>
        /// <param name="page">Specifies desired page of levels to return. There are nine levels per page.</param>
        /// <returns>Levels satisfying search term.
        /// Note, that not all meta data are filled in as they are not available in this database context.</returns>
        List<LevelRowDTO> GetNewestLevels(Byte? page);

        /// <summary>
        /// Verifies whether level password is correct.
        /// </summary>
        /// <param name="levelId">Id of level which user tries to unlock.</param>
        /// <param name="hash">Hash of level password sent in request.</param>
        /// <returns>True if hash from request matches the hash in the database, false otherwise.</returns>
        Boolean CheckLevelPassword(Int64 levelId, String hash);
    }
}
