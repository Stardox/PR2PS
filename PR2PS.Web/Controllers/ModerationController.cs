using Microsoft.AspNet.SignalR;
using PR2PS.Common.Constants;
using PR2PS.Common.Exceptions;
using PR2PS.DataAccess.Core;
using PR2PS.DataAccess.Entities;
using PR2PS.Web.Core;
using PR2PS.Web.Core.FormModels;
using PR2PS.Web.Core.Management;
using PR2PS.Web.Core.SignalR;
using System;
using System.Net.Http;
using System.Web.Http;

namespace PR2PS.Web.Controllers
{
    public class ModerationController : ApiController
    {
        private IDataAccessEngine dataAccess;

        public ModerationController(IDataAccessEngine dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        /// <summary>
        /// Bans user specified by username.
        /// </summary>
        /// <param name="banData">Ban form data (receiver, duration, reason, etc.).</param>
        /// <returns>Success if has been banned or error otherwise.</returns>
        [HttpPost]
        [Route("ban_user.php")]
        public HttpResponseMessage Ban([FromBody] BanFormModel banData)
        {
            try
            {
                if (banData == null)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_NO_FORM_DATA);
                }
                else if (!banData.Duration.HasValue || banData.Duration < 1)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_INVALID_DURATION);
                }

                SessionInstance issuerSession = SessionManager.Instance.GetSessionByToken(banData.Token);
                if (issuerSession == null)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, ErrorMessages.ERR_NOT_LOGGED_IN);
                }

                Account bannedUser = this.dataAccess.Ban(
                    issuerSession.AccounData.UserId,
                    banData.Banned_Name,
                    banData.Duration.Value,
                    banData.Reason,
                    banData.Record,
                    banData.IsIPBan);

                // User has been banned, notify all registered game servers about this event.
                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SignalRHub>();
                context.Clients.All.ForceLogout(bannedUser.Id, (banData.IsIPBan) ? bannedUser.LoginIP : String.Empty);

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
    }
}
