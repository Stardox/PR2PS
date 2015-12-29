using Newtonsoft.Json;
using PR2PS.Web.Core;
using PR2PS.Web.Core.JSONClasses;
using PR2PS.Web.Core.Management;
using PR2PS.Web.DataAccess;
using PR2PS.Web.DataAccess.Entities;
using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace PR2PS.Web.Controllers
{
    public class MiscController : ApiController
    {
        /// <summary>
        /// Default action. This gets called once no valid route is found.
        /// </summary>
        /// <returns>Error message indicating that the feature is not implemented yet.</returns>
        [HttpGet]
        [HttpPost]
        public HttpResponseMessage NotImplementedYet()
        {
            return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ErrorJSON
            {
                Error = "This feature is not implemented yet."
            }));
        }

        /// <summary>
        /// Gets welcome page.
        /// </summary>
        /// <returns>Gets welcome page lol.</returns>
        [HttpGet]
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
        public HttpResponseMessage GetServerStatus()
        {
            try
            {
                if (ServerManager.Instance.ServerCount <= 0)
                {
                    return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ErrorJSON
                    {
                        Error = "No servers available."
                    }));
                }

                return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ServerListJSON
                {
                    Servers = ServerManager.Instance.GetServers()
                }));
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
        public HttpResponseMessage GetPolicyFile()
        {
            try
            {
                return HttpResponseFactory.Response200XML(Constants.FILE_POLICY_XML);
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
        public HttpResponseMessage GetPlayerInfo(String name, String token, String rand)
        {
            try
            {
                DatabaseContext db = new DatabaseContext();
                Account foundUser = db.Accounts.FirstOrDefault(acc => acc.Username.ToUpper() == name.ToUpper());
                if (foundUser == null)
                {
                    return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ErrorJSON
                    {
                        Error = Constants.ERR_NO_USER_WITH_SUCH_NAME
                    }));
                }

                return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new UserInfoJSON
                {
                    UserId = foundUser.Id.ToString(),
                    Name = foundUser.Username,
                    Group = foundUser.Group.ToString(),
                    LoginDate = foundUser.LoginDate.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture),
                    RegisterDate = foundUser.RegisterDate.ToString("dd/MMM/yyyy", CultureInfo.InvariantCulture),
                    Status = foundUser.Status,

                    Rank = foundUser.CustomizeInfo.Rank,
                    Hats = foundUser.CustomizeInfo.HatSeq.Split(Constants.SEPARATOR_COMMA, StringSplitOptions.RemoveEmptyEntries).Length - 1, // TODO - Less retarded approach.
                    Hat = foundUser.CustomizeInfo.Hat.ToString(),
                    HatColor = foundUser.CustomizeInfo.HatColor.ToString(),
                    HatColor2 = foundUser.CustomizeInfo.HatColor2.ToString(),
                    Head = foundUser.CustomizeInfo.Head.ToString(),
                    HeadColor = foundUser.CustomizeInfo.HeadColor.ToString(),
                    HeadColor2 = foundUser.CustomizeInfo.HeadColor2.ToString(),
                    Body = foundUser.CustomizeInfo.Body.ToString(),
                    BodyColor = foundUser.CustomizeInfo.BodyColor.ToString(),
                    BodyColor2 = foundUser.CustomizeInfo.BodyColor2.ToString(),
                    Feet = foundUser.CustomizeInfo.Feet.ToString(),
                    FeetColor = foundUser.CustomizeInfo.FeetColor.ToString(),
                    FeetColor2 = foundUser.CustomizeInfo.FeetColor2.ToString(),

                    GuildId = "0", // TODO - For now...
                    GuildName = "", // TODO - For now...

                    Friend = 0, // TODO.
                    Ignored = 0 // TODO.
                }));
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }
    }
}
