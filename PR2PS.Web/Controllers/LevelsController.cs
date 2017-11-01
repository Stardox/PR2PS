using PR2PS.Common.Constants;
using PR2PS.Common.DTO;
using PR2PS.Common.Exceptions;
using PR2PS.Common.Extensions;
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
        /// Performs search according to specified search query.
        /// </summary>
        /// <param name="searchData">Search query.</param>
        /// <returns>List of found maps.</returns>
        [HttpPost]
        [Route("search_levels.php")]
        public HttpResponseMessage SearchLevels([FromBody] SearchLevelsFormModel searchData)
        {
            // TODO - This is temporary solution, will perform request to Jiggy.

            try
            {
                if (searchData == null)
                {
                    return HttpResponseFactory.Response200Json(new ErrorJson
                    {
                        Error = ErrorMessages.ERR_SEARCH_FAILED
                    });
                }

                using (WebClient webClient = new WebClient())
                {
                    webClient.QueryString.Add("search_str", searchData.Search_Str);
                    webClient.QueryString.Add("order", searchData.Order);
                    webClient.QueryString.Add("mode", searchData.Mode);
                    webClient.QueryString.Add("dir", searchData.Dir);
                    webClient.QueryString.Add("page", searchData.Page);
                    webClient.QueryString.Add("token", searchData.Token);
                    webClient.QueryString.Add("rand", searchData.Rand);

                    String result = webClient.DownloadString("https://pr2hub.com/search_levels.php");

                    return HttpResponseFactory.Response200Plain(result);
                }
            }
            catch(Exception ex)
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

                this.levelsDAL.SaveLevel(mySession.AccounData.UserId, levelData.ToDTO(), this.Request.GetRemoteIPAddress());

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
        public HttpResponseMessage Levels(Int64? id, Int32? version = -1)
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
    }
}
