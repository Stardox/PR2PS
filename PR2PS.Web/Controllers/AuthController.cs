using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using PR2PS.Common.Constants;
using PR2PS.Common.DTO;
using PR2PS.Common.Exceptions;
using PR2PS.Common.Extensions;
using PR2PS.DataAccess.Core;
using PR2PS.DataAccess.Entities;
using PR2PS.Web.Core;
using PR2PS.Web.Core.FormModels;
using PR2PS.Web.Core.JsonModels;
using PR2PS.Web.Core.Management;
using PR2PS.Web.Core.SignalR;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Helpers;
using System.Web.Http;

namespace PR2PS.Web.Controllers
{
    public class AuthController : ApiController
    {
        private IDataAccessEngine dataAccess { get; set; }

        public AuthController(IDataAccessEngine dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        /// <summary>
        /// Checks if user is logged in.
        /// </summary>
        /// <param name="token">Session token to identify the user.</param>
        /// <param name="rand">Random string.</param>
        /// <returns>Key-value pair containing username. Example: user_name=Admin</returns>
        [HttpGet]
        [Route("check_login.php")]
        public HttpResponseMessage CheckLogin(String token = "", String rand = "")
        {
            SessionInstance session = SessionManager.Instance.GetSessionByToken(token);

            return HttpResponseFactory.Response200Plain(StatusKeys.USERNAME, session?.AccounData.Username);
        }

        /// <summary>
        /// Authenticate user and if successful, then create session for him and notify
        /// appropriate game server that the authentication has been successful.
        /// </summary>
        /// <param name="loginData">Login form data.</param>
        /// <returns>Login attempt status or error.</returns>
        [HttpPost]
        [Route("login.php")]
        public HttpResponseMessage Login([FromBody] LoginFormModel loginData)
        {
            try
            {
                // TODO - Token processing, version check and more checks based on received data.

                if (loginData == null || String.IsNullOrEmpty(loginData.I))
                {
                    return HttpResponseFactory.Response200Json(new ErrorJson { Error = ErrorMessages.ERR_NO_LOGIN_DATA });
                }

                String rawloginDataJSON = loginData.I.FromBase64ToString();
                LoginDataJson loginDataJSON = JsonConvert.DeserializeObject<LoginDataJson>(rawloginDataJSON);

                // Authenticate user.
                Account acc = this.dataAccess.AuthenticateUser(loginDataJSON.UserName, loginDataJSON.UserPass, this.Request.GetRemoteIPAddress());

                // Check if such server even exists.
                ServerInstance server = ServerManager.Instance.GetServer(loginDataJSON.Server.Server_name);
                if (server == null)
                {
                    return HttpResponseFactory.Response200Json(new ErrorJson { Error = ErrorMessages.ERR_NO_SUCH_SERVER });
                }

                // Check whether this user already has session.
                SessionInstance session = SessionManager.Instance.GetSessionByUsername(acc.Username);
                if (session != null)
                {
                    // He has, lets log him out.
                    IHubContext context2 = GlobalHost.ConnectionManager.GetHubContext<SignalRHub>();
                    context2.Clients.Client(server.SignalRClientId).ForceLogout(session.AccounData.UserId, session.IP);

                    return HttpResponseFactory.Response200Json(new ErrorJson { Error = ErrorMessages.ERR_ALREADY_IN });
                }

                // No, he has no session. Lets make him one.
                AccountDataDTO accData = acc.ToAccountDataDTO();
                session = new SessionInstance(
                    Guid.NewGuid(),
                    loginDataJSON.LoginId,
                    accData,
                    loginDataJSON.Server.Server_name,
                    this.Request.GetRemoteIPAddress(),
                    this.Request.GetRemotePort());

                SessionManager.Instance.StoreSession(session);

                // Notify relevant server that authentication succeeded.
                IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SignalRHub>();
                context.Clients.Client(server.SignalRClientId).LoginSuccessful(session.LoginId, accData);

                this.dataAccess.UpdateAccountStatus(
                    acc,
                    String.Concat(StatusMessages.STR_PLAYING_ON, session.Server),
                    this.Request.GetRemoteIPAddress());

                return HttpResponseFactory.Response200Json(new LoginReplyJson
                {
                    Ant = false, // TODO - Has Ant set.
                    Email = false, // TODO - Has email set.
                    Emblem = "", // TODO - Guild emblem URL.
                    Guild = "0", // TODO - Guild id.
                    GuildName = "", // TODO - Guild name.
                    GuildOwner = 0, // TODO - Guild owner uid.
                    LastRead = 0, // TODO - Seconds since UNIX time when user last read PMs.
                    LastRecv = 0, // TODO - Seconds since UNIX time when user last received PM.
                    Status = StatusMessages.STR_SUCCESS,
                    Time = 0, // TODO - Seconds since UNIX time.
                    Token = session.Token.ToString(),
                    UserId = acc.Id
                });
            }
            catch (PR2Exception ex)
            {
                return HttpResponseFactory.Response200Json(new ErrorJson { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Deauthenticate user, however token is always empty so this method does nothing.
        /// TODO - Take a look at cookies if its possible to identify session using that.
        /// </summary>
        /// <param name="token">Unique session token.</param>
        /// <param name="rand">Random string.</param>
        /// <returns>This method always returns succes, lol.</returns>
        [HttpGet]
        [Route("logout.php")]
        public HttpResponseMessage Logout(String token = "", String rand = "")
        {
            // TODO - Figure out how real PR2 identifies your session since token is always empty.
            // Probably using cookies or idk. For now we will rely on actual game server (TCP).
            return HttpResponseFactory.Response200Plain(StatusKeys.SUCCESS, "true");
        }

        /// <summary>
        /// Checks if such user profile can be created and if so creates it.
        /// </summary>
        /// <param name="registerData">Register form data (username, password, etc).</param>
        /// <returns>Success if profile has been created or error otherwise.</returns>
        [HttpPost]
        [Route("register_user.php")]
        public HttpResponseMessage Register([FromBody] RegisterFormModel registerData)
        {
            try
            {
                if (registerData == null)
                {
                    return HttpResponseFactory.Response200Json(new ErrorJson { Error = ErrorMessages.ERR_NO_REGISTER_DATA });
                }

                this.dataAccess.RegisterUser(registerData.Name, registerData.Password, registerData.Email, this.Request.GetRemoteIPAddress());

                return HttpResponseFactory.Response200Json(new ResultJson { Result = StatusMessages.STR_SUCCESS });
            }
            catch (PR2Exception ex)
            {
                return HttpResponseFactory.Response200Json(new ErrorJson { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }

        /// <summary>
        /// Checks if such user profile can be created and if so creates it.
        /// </summary>
        /// <param name="registerData">Register form data (username, password, etc).</param>
        /// <returns>Success if profile has been created or error otherwise.</returns>
        [HttpPost]
        [Route("change_password.php")]
        public HttpResponseMessage ChangePassword([FromBody] ChangePassFormModel changePassData)
        {
            try
            {
                if (changePassData == null)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, "No data received.");
                }

                // TODO - Probably some more validation like length and special characters contraints.
                if (String.IsNullOrEmpty(changePassData.Token)) changePassData.Token = String.Empty;
                if (String.IsNullOrEmpty(changePassData.Old_Pass)) changePassData.Old_Pass = String.Empty;
                if (String.IsNullOrEmpty(changePassData.New_Pass)) changePassData.New_Pass = String.Empty;

                SessionInstance mySession = SessionManager.Instance.GetSessionByToken(changePassData.Token);
                if (mySession == null)
                {
                    return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, "You are not logged in.");
                }

                using (DatabaseContext db = new DatabaseContext("PR2Context"))
                {
                    Account me = db.Accounts.FirstOrDefault(a => a.Id == mySession.AccounData.UserId);
                    if (!Crypto.VerifyHashedPassword(me.PasswordHash, changePassData.Old_Pass))
                    {
                        return HttpResponseFactory.Response200Plain(StatusKeys.ERROR, "You have entered a wrong password.");
                    }

                    me.PasswordHash = Crypto.HashPassword(changePassData.New_Pass);
                    db.SaveChanges();

                    return HttpResponseFactory.Response200Plain(StatusKeys.MESSAGE, "The password has been changed succesfully!");
                }
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }
    }
}
