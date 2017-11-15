using Newtonsoft.Json;
using PR2PS.Common.Constants;
using PR2PS.Common.DTO;
using PR2PS.Common.Exceptions;
using PR2PS.Common.Extensions;
using PR2PS.DataAccess.Entities;
using PR2PS.DataAccess.LevelsDataAccess;
using PR2PS.DataAccess.MainDataAccess;
using PR2PS.Web.Core;
using PR2PS.Web.Core.FormModels;
using PR2PS.Web.Core.JsonModels;
using PR2PS.Web.Core.Management;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using static PR2PS.Common.Enums;

namespace PR2PS.Web.Controllers
{
    /// <summary>
    /// Controller responsible for serving level lists, levels themself and also for handling level editor.
    /// </summary>
    public class LevelsController : ApiController
    {
        private ILevelsDataAccessEngine levelsDAL;
        private IMainDataAccessEngine mainDAL;

        public LevelsController(IMainDataAccessEngine mainDAL, ILevelsDataAccessEngine levelsDAL)
        {
            this.levelsDAL = levelsDAL;
            this.mainDAL = mainDAL;
        }

        /// <summary>
        /// Gets file containing campaign maps.
        /// </summary>
        /// <param name="campaignId">Campaign levels collection id (1 is original campaign, 2 is speed campaign, etc).</param>
        /// <returns>File containing campaign maps if exists or internal server error otherwise.</returns>
        [HttpGet]
        [Route("files/lists/campaign/{campaignId}")]
        public HttpResponseMessage GetCampaign(Int32 campaignId = 1, String token = "", String rand = "")
        {
            try
            {
                String campaignMaps;
                if (!WebConstants.FILE_CAMPAIGN.TryGetValue(campaignId, out campaignMaps))
                {
                    return HttpResponseFactory.Response200Json(new ErrorJson
                    {
                        Error = ErrorMessages.ERR_NO_SUCH_LEVELS
                    });
                }

                return HttpResponseFactory.Response200Plain(campaignMaps);
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Performs search according to search query specified in query string.
        /// </summary>
        /// <param name="searchData">Search query.</param>
        /// <returns>List of found maps.</returns>
        [HttpGet]
        [Route("search_levels.php")]
        public HttpResponseMessage SearchLevelsGet([FromUri] SearchLevelsFormModel searchData)
        {
            try
            {
                return GetSearchResults(searchData);
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Performs search according to search query specified in form data.
        /// </summary>
        /// <param name="searchData">Search query.</param>
        /// <returns>List of found maps.</returns>
        [HttpPost]
        [Route("search_levels.php")]
        public HttpResponseMessage SearchLevelsPost([FromBody] SearchLevelsFormModel searchData)
        {
            try
            {
                return GetSearchResults(searchData);
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Handles request for uploading level from the Level Editor and stores it into database.
        /// </summary>
        /// <param name="levelData">Uploaded level data.</param>
        /// <returns>Status indicating whether level has been correctly saved.</returns>
        [HttpPost]
        [Route("upload_level.php")]
        public HttpResponseMessage UploadLevel([FromBody] UploadLevelFormModel levelData)
        {
            try
            {
                if (levelData == null)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_NO_FORM_DATA);
                }

                SessionInstance mySession = SessionManager.Instance.GetSessionByToken(levelData.Token);
                if (mySession == null)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_NOT_LOGGED_IN);
                }

                this.levelsDAL.SaveLevel(mySession.AccounData.UserId, mySession.AccounData.Username, levelData.ToDTO(), this.Request.GetRemoteIPAddress());

                return HttpResponseFactory.Response200Plain(StatusKeys.MESSAGE, StatusMessages.SAVE_SUCCESSFUL);
            }
            catch (PR2Exception ex)
            {
                return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ex.Message);
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Downloads level specified by id and version number.
        /// </summary>
        /// <param name="id">Unique session token.</param>
        /// <param name="version">Random string.</param>
        /// <returns>String containing all levels created by current user.</returns>
        [HttpGet]
        [Route("levels/{id}.txt")]
        public HttpResponseMessage DownloadLevel(Int64? id, Int32? version = -1)
        {
            try
            {
                LevelDataDTO level = this.levelsDAL.GetLevel(id ?? -1, version ?? -1);

                return HttpResponseFactory.Response200Plain(level.ToString());
            }
            catch (PR2Exception ex)
            {
                return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ex.Message);
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Requests list of levels for the level editor for specific user.
        /// </summary>
        /// <param name="token">Unique session token.</param>
        /// <param name="rand">Random string.</param>
        /// <returns>String containing all levels created by current user.</returns>
        [HttpGet]
        [Route("get_levels.php")]
        public HttpResponseMessage GetLevels(String token = "", String rand = "")
        {
            try
            {
                SessionInstance mySession = SessionManager.Instance.GetSessionByToken(token);
                if (mySession == null)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_NOT_LOGGED_IN);
                }

                List<LevelRowDTO> levels = this.levelsDAL.GetUserLevels(mySession.AccounData.UserId);
                this.mainDAL.FillLevelListMetadata(levels);

                return HttpResponseFactory.Response200Plain(levels.GetLevelListString());
            }
            catch (PR2Exception ex)
            {
                return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ex.Message);
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Handles request for soft deleting level.
        /// </summary>
        /// <param name="levelData">Data about level which is getting deleted.</param>
        /// <returns>Status indicating whether level has been deleted.</returns>
        [HttpPost]
        [Route("delete_level.php")]
        public HttpResponseMessage DeleteLevel([FromBody] DeleteLevelFormModel levelData)
        {
            try
            {
                if (levelData == null || !levelData.Level_Id.HasValue)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_NO_FORM_DATA);
                }

                SessionInstance mySession = SessionManager.Instance.GetSessionByToken(levelData.Token);
                if (mySession == null)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_NOT_LOGGED_IN);
                }

                this.levelsDAL.SoftDeleteLevel(mySession.AccounData.UserId, levelData.Level_Id.Value);

                return HttpResponseFactory.Response200Plain(StatusKeys.SUCCESS, StatusMessages.TRUE);
            }
            catch (PR2Exception ex)
            {
                return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ex.Message);
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Submits level rating.
        /// </summary>
        /// <param name="ratingData">Data about level vote which is being cast.</param>
        /// <returns>Status indicating whether voting has been successfully processed.</returns>
        [HttpPost]
        [Route("submit_rating.php")]
        public HttpResponseMessage SubmitRating([FromBody] SubmitRatingFormModel ratingData)
        {
            try
            {
                if (ratingData == null || !ratingData.Level_Id.HasValue || !ratingData.Rating.HasValue)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_NO_FORM_DATA);
                }

                SessionInstance mySession = SessionManager.Instance.GetSessionByToken(ratingData.Token);
                if (mySession == null)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_NOT_LOGGED_IN);
                }

                RatingDataDTO ratingResult = this.levelsDAL.SaveRating(ratingData.Level_Id.Value, ratingData.Rating.Value, mySession.AccounData.UserId, this.Request.GetRemoteIPAddress());

                return HttpResponseFactory.Response200Plain(StatusKeys.MESSAGE, ratingResult.ToString());
            }
            catch (PR2Exception ex)
            {
                return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ex.Message);
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Handles request for unpublishing level by moderators and administrators.
        /// </summary>
        /// <param name="levelData">Data about level which is getting unpublished.</param>
        /// <returns>Status indicating whether level has been unpublished.</returns>
        [HttpPost]
        [Route("remove_level.php")]
        public HttpResponseMessage RemoveLevel([FromBody] DeleteLevelFormModel levelData)
        {
            try
            {
                if (levelData == null || !levelData.Level_Id.HasValue)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_NO_LEVEL_ID);
                }

                SessionInstance mySession = SessionManager.Instance.GetSessionByToken(levelData.Token);
                if (mySession == null)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_NOT_LOGGED_IN);
                }

                if (mySession.AccounData.Group < UserGroup.MODERATOR)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_NO_RIGHTS);
                }

                this.levelsDAL.UnpublishLevel(levelData.Level_Id.Value);

                return HttpResponseFactory.Response200Plain(StatusKeys.SUCCESS, StatusMessages.TRUE);
            }
            catch (PR2Exception ex)
            {
                return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ex.Message);
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Requests list of levels for the newest tab.
        /// </summary>
        /// <param name="page">Page number.</param>
        /// <param name="token">Unique session token.</param>
        /// <param name="rand">Random string.</param>
        /// <returns>String containing newest levels specified by page.</returns>
        [HttpGet]
        [Route("files/lists/newest/{page}")]
        public HttpResponseMessage GetLevels(Byte? page, String token = "", String rand = "")
        {
            try
            {
                List<LevelRowDTO> levels = this.levelsDAL.GetNewestLevels(page);
                this.mainDAL.FillLevelListMetadata(levels);

                return HttpResponseFactory.Response200Plain(levels.GetLevelListString());
            }
            catch (PR2Exception ex)
            {
                return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ex.Message);
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Attempts to unlock password protected level.
        /// </summary>
        /// <param name="courseID">Id of level which user wants to unlock.</param>
        /// <param name="hash">Hashed attempted level password that user entered.</param>
        /// <param name="token">Unique session token.</param>
        /// <param name="rand">Random string.</param>
        /// <returns>Status indicating whether level has been successfully unlocked.</returns>
        [HttpGet]
        [Route("level_pass_check.php")]
        public HttpResponseMessage PasswordCheck(Int64? courseID, String hash = "", String token = "", String rand = "")
        {
            try
            {
                if (!courseID.HasValue)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_NO_LEVEL_ID);
                }

                SessionInstance mySession = SessionManager.Instance.GetSessionByToken(token);
                if (mySession == null)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_NOT_LOGGED_IN);
                }

                Boolean unlocked = this.levelsDAL.CheckLevelPassword(courseID.Value, hash);
                String serializedResult = JsonConvert.SerializeObject(new LevelPassCheckResultJson
                {
                    Access = unlocked ? StatusMessages.ONE : StatusMessages.ZERO,
                    LevelId = courseID.Value.ToString(),
                    UserId = mySession.AccounData.UserId.ToString()
                });
                String encodedResult = serializedResult.ToBase64();

                return HttpResponseFactory.Response200Plain(StatusKeys.RESULT, encodedResult);
            }
            catch (PR2Exception ex)
            {
                return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ex.Message);
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Performs search according to specified search query.
        /// </summary>
        /// <param name="searchData">Search query.</param>
        /// <returns>List of found maps.</returns>
        private HttpResponseMessage GetSearchResults(SearchLevelsFormModel searchData)
        {
            try
            {
                if (searchData == null)
                {
                    return HttpResponseFactory.Response200Json(new ErrorJson
                    {
                        Error = ErrorMessages.ERR_NO_FORM_DATA
                    });
                }

                if (String.IsNullOrEmpty(ConfigurationManager.Instance.SearchUrl))
                {
                    // External level search web API is not specified, handle the request internally.

                    List<LevelRowDTO> levels = null;

                    if (searchData.Mode == SearchMode.User)
                    {
                        Account acc = this.mainDAL.GetAccountByUsername(searchData.Search_Str);
                        if (acc == null)
                        {
                            return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_NO_USER_WITH_SUCH_NAME);
                        }

                        levels = this.levelsDAL.SearchLevelsByUserId(acc.Id, searchData.Order, searchData.Dir, searchData.Page);
                    }
                    else
                    {
                        levels = this.levelsDAL.SearchLevelsByTerm(searchData.Search_Str, searchData.Order, searchData.Dir, searchData.Page);
                    }

                    this.mainDAL.FillLevelListMetadata(levels);

                    return HttpResponseFactory.Response200Plain(levels.GetLevelListString());
                }
                else
                {
                    // Let the external API handle the level search.

                    using (WebClient webClient = new WebClient())
                    {
                        webClient.QueryString.Add("search_str", searchData.Search_Str);
                        webClient.QueryString.Add("order", searchData.Order.ToString().ToLower());
                        webClient.QueryString.Add("mode", searchData.Mode.ToString().ToLower());
                        webClient.QueryString.Add("dir", searchData.Dir.ToString().ToLower());
                        webClient.QueryString.Add("page", (searchData.Page ?? 1).ToString());
                        webClient.QueryString.Add("rand", searchData.Rand);

                        String result = webClient.DownloadString(ConfigurationManager.Instance.SearchUrl);

                        return HttpResponseFactory.Response200Plain(result);
                    }
                }
            }
            catch (PR2Exception ex)
            {
                return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ex.Message);
            }
        }
    }
}
