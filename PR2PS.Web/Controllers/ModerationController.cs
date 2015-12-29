using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using PR2PS.Web.Core;
using PR2PS.Web.Core.FormModels;
using PR2PS.Web.Core.JSONClasses;
using PR2PS.Web.Core.Management;
using PR2PS.Web.Core.SignalR;
using PR2PS.Web.DataAccess;
using PR2PS.Web.DataAccess.Entities;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace PR2PS.Web.Controllers
{
    public class ModerationController : ApiController
    {
        /// <summary>
        /// Bans user specified by username.
        /// </summary>
        /// <param name="banData">Ban form data (receiver, duration, reason, etc.).</param>
        /// <returns>Success if has been banned error otherwise.</returns>
        [HttpPost]
        public HttpResponseMessage Ban([FromBody] BanFormModel banData)
        {
            try
            {
                if (banData == null)
                {
                    return HttpResponseFactory.Response200Plain(String.Concat("error=", Constants.ERR_NO_FORM_DATA));
                }

                // TODO - Probably some more validation like length and special characters contraints.
                if (String.IsNullOrEmpty(banData.Banned_Name)) banData.Banned_Name = String.Empty;
                if (String.IsNullOrEmpty(banData.Duration)) banData.Duration = String.Empty;
                if (String.IsNullOrEmpty(banData.Token)) banData.Token = String.Empty;

                SessionInstance issuerSession = SessionManager.Instance.GetSessionByToken(banData.Token);
                if (issuerSession == null)
                {
                    return HttpResponseFactory.Response200Plain(String.Concat("error=", Constants.ERR_NOT_LOGGED_IN));
                }

                if (issuerSession.AccounData.Group < 2)
                {
                    return HttpResponseFactory.Response200Plain(String.Concat("error=", Constants.ERR_NO_RIGHTS));
                }

                Int32 duration;
                if (!Int32.TryParse(banData.Duration, out duration) || duration < 0)
                {
                    return HttpResponseFactory.Response200Plain(String.Concat("error=", Constants.ERR_INVALID_DURATION));
                }

                using (DatabaseContext db = new DatabaseContext())
                {
                    Account receiver = db.Accounts.FirstOrDefault(acc => acc.Username.ToUpper() == banData.Banned_Name.ToUpper());
                    Account issuer = db.Accounts.FirstOrDefault(acc => acc.Id == issuerSession.AccounData.UserId);

                    if (receiver == null)
                    {
                        return HttpResponseFactory.Response200Plain(String.Concat("error=", Constants.ERR_NO_USER_WITH_SUCH_NAME));
                    }

                    if (receiver.Group == 3)
                    {
                        return HttpResponseFactory.Response200Plain(String.Concat("error=", "Administrators are absolute!"));                        
                    }

                    Ban ban = new Ban
                    {
                        Issuer = issuer,
                        IPAddress = receiver.LoginIP,
                        IsIPBan = banData.IsIPBan,
                        StartDate = DateTime.UtcNow,
                        ExpirationDate = DateTime.UtcNow.AddSeconds(duration),
                        Reason = banData.Reason,
                        Extra = banData.Record
                    };

                    // User has been banned.
                    IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SignalRHub>();
                    context.Clients.All.ForceLogout(receiver.Id, (ban.IsIPBan) ? receiver.LoginIP : String.Empty);
                    receiver.Bans.Add(ban);
                    db.SaveChanges();

                    return HttpResponseFactory.Response200Plain("success=true");
                }
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }
    }
}
