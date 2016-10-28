using PR2PS.Common.Constants;
using PR2PS.DataAccess.Core;
using PR2PS.DataAccess.Entities;
using PR2PS.Web.Core;
using PR2PS.Web.Core.JsonModels;
using PR2PS.Web.Core.Management;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace PR2PS.Web.Controllers
{
    public class MiscController : ApiController
    {
        private IDataAccessEngine dataAccess;

        public MiscController(IDataAccessEngine dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        /// <summary>
        /// Gets welcome page.
        /// </summary>
        /// <returns>Gets welcome page lol.</returns>
        [HttpGet]
        [Route("")]
        public HttpResponseMessage GetWelcomeMessage()
        {
            // TODO - Nicer welcome page. :-)
            return HttpResponseFactory.Response200Plain("PR2PS.Web works!");
        }

        /// <summary>
        /// Gets list of available game servers along with their statuses.
        /// </summary>
        /// <returns>List of available game servers.</returns>
        [HttpGet]
        [Route("files/server_status_2.txt")]
        public HttpResponseMessage GetServerStatus()
        {
            try
            {
                if (ServerManager.Instance.ServerCount <= 0)
                {
                    return HttpResponseFactory.Response200Json(new ErrorJson
                    {
                        Error = ErrorMessages.ERR_NO_SERVERS
                    });
                }

                return HttpResponseFactory.Response200Json(new ServerListJson
                {
                    Servers = ServerManager.Instance.GetServers()
                });
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Gets crossdomain policy XML file.
        /// </summary>
        /// <returns>The crossdomain policy XML if found or internal server error otherwise.</returns>
        [HttpGet]
        [Route("crossdomain.xml")]
        public HttpResponseMessage GetPolicyFile()
        {
            try
            {
                return HttpResponseFactory.Response200Xml(WebConstants.FILE_POLICY_XML);
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Attemps to find PR2 user by given username.
        /// </summary>
        /// <param name="name">Username to search for.</param>
        /// <param name="token">Session token. Used to determine if found user is friend or ignored.</param>
        /// <param name="rand">Random string.</param>
        /// <returns>JSON containing found user data or internal server error if something goes wrong.</returns>
        [HttpGet]
        [Route("get_player_info_2.php")]
        public HttpResponseMessage GetPlayerInfo(String name = "", String token = "", String rand = "")
        {
            try
            {
                // TODO - Handle token to check if lookup player is friend and/or ignored.

                Account acc = this.dataAccess.GetAccountByUsername(name);
                if (acc == null)
                {
                    return HttpResponseFactory.Response200Json(new ErrorJson { Error = ErrorMessages.ERR_NO_USER_WITH_SUCH_NAME });
                }

                return HttpResponseFactory.Response200Json(UserInfoJson.ToUserInfoJson(acc));
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }
    }
}
