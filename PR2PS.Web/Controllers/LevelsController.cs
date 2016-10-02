using Newtonsoft.Json;
using PR2PS.Common.Constants;
using PR2PS.Web.Core;
using PR2PS.Web.Core.FormModels;
using PR2PS.Web.Core.JsonModels;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PR2PS.Web.Controllers
{
    /// <summary>
    /// Controller responsible for serving map lists and maps themself.
    /// </summary>
    public class LevelsController : ApiController
    {
        /// <summary>
        /// Gets file containing campaign maps.
        /// </summary>
        /// <param name="campaignId">Campaign levels collection id (1 is original campaign, 2 is speed campaign, etc).</param>
        /// <returns>File containing campaign maps if exists or internal server error otherwise.</returns>
        [HttpGet]
        public HttpResponseMessage GetCampaign(Int32 campaignId, String token, String rand)
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
    }
}
