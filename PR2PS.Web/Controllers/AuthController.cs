using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using PR2PS.Common;
using PR2PS.Common.Constants;
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
using System.Web.Helpers;
using System.Web.Http;

namespace PR2PS.Web.Controllers
{
    public class AuthController : ApiController
    {
        /// <summary>
        /// Checks if user is logged in.
        /// TODO - Only partially implemented.
        /// </summary>
        /// <param name="token">Session token to identify the user.</param>
        /// <param name="rand">Random string.</param>
        /// <returns>Username if found session exists or empty string otherwise.</returns>
        [HttpGet]
        public HttpResponseMessage CheckLogin(String token, String rand)
        {
            // TODO - check token and return username if available.
            return HttpResponseFactory.Response200Plain("user_name=");
        }

        /// <summary>
        /// Authenticate user and if everything is good then create session and notify
        /// appropriate game server that the user is clean.
        /// </summary>
        /// <param name="loginData">Login form data.</param>
        /// <returns>Login attempt result or internal server error.</returns>
        [HttpPost]
        public HttpResponseMessage Login([FromBody] LoginFormModel loginData)
        {
            try
            {
                // TODO - Token processing, version check and more checks based on received data.

                if (loginData == null)
                {
                    return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ErrorJSON
                    {
                        Error = "No login data received."
                    }));
                }

                if (String.IsNullOrEmpty(loginData.I))
                {
                    return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ErrorJSON
                    {
                        Error = "Missing login parameter 'i'."
                    }));
                }

                String rawloginDataJSON = Base64.Decode(loginData.I);
                LoginDataJSON loginDataJSON = JsonConvert.DeserializeObject<LoginDataJSON>(rawloginDataJSON);

                using (DatabaseContext db = new  DatabaseContext())
                {
                    Account accModel = db.Accounts.FirstOrDefault(a => a.Username.ToUpper() == loginDataJSON.User_name.ToUpper());
                        
                    if (accModel == null)
                    {
                        return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ErrorJSON
                        {
                            Error = ErrorMessages.ERR_NO_USER_WITH_SUCH_NAME
                        }));
                    }

                    if (!Crypto.VerifyHashedPassword(accModel.PasswordHash, loginDataJSON.User_pass))
                    {
                        return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ErrorJSON
                        {
                            Error = ErrorMessages.ERR_WRONG_PASS
                        }));
                    }

                    // We made it this far, therefore user exists and has been authenticated.

                    // Check if such server even exists.
                    ServerInstance foundServer = ServerManager.Instance.GetServer(loginDataJSON.Server.Server_name);
                    if (foundServer == null)
                    {
                        return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ErrorJSON
                        {
                            Error = ErrorMessages.ERR_NO_SUCH_SERVER
                        }));
                    }

                    // Check whether this user is banned.
                    String ip = this.Request.GetRemoteIPAddress();
                    DateTime utcDateTime = DateTime.UtcNow; // To prevent usage of SQLite functions.
                    Ban foundBan = db.Bans
                        .Where(ban =>
                            (ban.Receiver.Id == accModel.Id ||
                            (ban.IsIPBan && ban.IPAddress == ip)) &&
                            DateTime.Compare(ban.ExpirationDate, utcDateTime) > 0)
                        .OrderByDescending(ban => ban.ExpirationDate)
                        .FirstOrDefault();
                    if (foundBan != null)
                    {
                        return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ErrorJSON
                        {
                            Error = String.Format(
                                ErrorMessages.ERR_BANNED,
                                foundBan.Issuer.Username,
                                String.IsNullOrWhiteSpace(foundBan.Reason) ? StatusMessages.STR_NO_REASON: foundBan.Reason,
                                foundBan.Id,
                                foundBan.ExpirationDate.ToUniversalTime().GetPrettyBanExpirationString())
                        }));
                    }

                    // Check whether this user already has session.
                    SessionInstance session = SessionManager.Instance.GetSessionByUsername(accModel.Username);
                    if (session != null)
                    { 
                        // He has, lets log him out.
                        IHubContext context2 = GlobalHost.ConnectionManager.GetHubContext<SignalRHub>();
                        context2.Clients.Client(foundServer.SignalRClientId).ForceLogout(session.AccounData.UserId, session.IP);

                        return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ErrorJSON
                        {
                            Error = ErrorMessages.ERR_ALREADY_IN
                        }));
                    }

                    // No he has not, but thats good, lets make him one.
                    AccountDataDTO accData = new AccountDataDTO()
                    {
                        UserId = accModel.Id,
                        Username = accModel.Username,
                        Group = accModel.Group,
                        Hat = accModel.CustomizeInfo.Hat,
                        Head = accModel.CustomizeInfo.Head,
                        Body = accModel.CustomizeInfo.Body,
                        Feet = accModel.CustomizeInfo.Feet,
                        HatColor = accModel.CustomizeInfo.HatColor,
                        HeadColor = accModel.CustomizeInfo.HeadColor,
                        BodyColor = accModel.CustomizeInfo.BodyColor,
                        FeetColor = accModel.CustomizeInfo.FeetColor,
                        HatColor2 = accModel.CustomizeInfo.HatColor2,
                        HeadColor2 = accModel.CustomizeInfo.HeadColor2,
                        BodyColor2 = accModel.CustomizeInfo.BodyColor2,
                        FeetColor2 = accModel.CustomizeInfo.FeetColor2,
                        HatSeq = accModel.CustomizeInfo.HatSeq,
                        HeadSeq = accModel.CustomizeInfo.HeadSeq,
                        BodySeq = accModel.CustomizeInfo.BodySeq,
                        FeetSeq = accModel.CustomizeInfo.FeetSeq,
                        HatSeqEpic = accModel.CustomizeInfo.HatSeqEpic,
                        HeadSeqEpic = accModel.CustomizeInfo.HeadSeqEpic,
                        BodySeqEpic = accModel.CustomizeInfo.BodySeqEpic,
                        FeetSeqEpic = accModel.CustomizeInfo.FeetSeqEpic,
                        Speed = accModel.CustomizeInfo.Speed,
                        Accel = accModel.CustomizeInfo.Accel,
                        Jump = accModel.CustomizeInfo.Jump,
                        Rank = accModel.CustomizeInfo.Rank,
                        UsedRankTokens = accModel.CustomizeInfo.UsedRankTokens,
                        ObtainedRankTokens = accModel.CustomizeInfo.ObtainedRankTokens
                    };

                    session = new SessionInstance(
                        Guid.NewGuid(),
                        loginDataJSON.Login_id,
                        accData,
                        loginDataJSON.Server.Server_name,
                        this.Request.GetRemoteIPAddress(),
                        this.Request.GetRemotePort());

                    IHubContext context = GlobalHost.ConnectionManager.GetHubContext<SignalRHub>();
                    context.Clients.Client(foundServer.SignalRClientId).LoginSuccessful(session.LoginId, accData);

                    accModel.Status = String.Concat(StatusMessages.STR_PLAYING_ON, session.Server);
                    accModel.LoginDate = DateTime.UtcNow;
                    accModel.LoginIP = this.Request.GetRemoteIPAddress();
                    accModel.CustomizeInfo = accModel.CustomizeInfo;
                    accModel.Experience = accModel.Experience;
                    db.SaveChanges();

                    SessionManager.Instance.StoreSession(session);

                    // Success.
                    return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new LoginReplyJSON()
                    {
                        Ant = false, // TODO.
                        Email = false, // TODO.
                        Emblem = "", // TODO.
                        Guild = "0", // TODO.
                        GuildName = "", // TODO.
                        GuildOwner = 0, // TODO.
                        LastRead = 0, // TODO.
                        LastRecv = 0, // TODO.
                        Status = StatusMessages.STR_SUCCESS,
                        Time = 0, // TODO,
                        Token = session.Token.ToString(),
                        UserId = accModel.Id
                    }));
                }
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
        public HttpResponseMessage Logout(String token, String rand)
        {
            // TODO - Figure out how real PR2 identifies your session since token is always empty.
            // Probably using cookies or idk. For now we will rely on actual game server (TCP).
            return HttpResponseFactory.Response200Plain("success=true");
        }

        /// <summary>
        /// Checks if such user profile can be created and if so creates it.
        /// </summary>
        /// <param name="registerData">Register form data (username, password, etc).</param>
        /// <returns>Success if profile has been created or error otherwise.</returns>
        [HttpPost]
        public HttpResponseMessage Register([FromBody] RegisterFormModel registerData)
        {
            try
            {
                if (registerData == null)
                {
                    return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ErrorJSON
                    {
                        Error = "No register data received."
                    }));
                }

                // TODO - Probably some more validation like length and special characters contraints.
                if (String.IsNullOrEmpty(registerData.Name)) registerData.Name = String.Empty;
                if (String.IsNullOrEmpty(registerData.Password)) registerData.Password = String.Empty;
                if (String.IsNullOrEmpty(registerData.Email)) registerData.Email = String.Empty;

                using (DatabaseContext db = new DatabaseContext())
                {
                    if (db.Accounts.Any(a => a.Username.ToUpper() == registerData.Name.ToUpper()))
                    {
                        return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new ErrorJSON
                        {
                            Error = ErrorMessages.ERR_USER_EXISTS
                        }));
                    }

                    Account newAcc = new Account()
                    {
                        Username = registerData.Name,
                        PasswordHash = Crypto.HashPassword(registerData.Password),
                        Email = registerData.Email,
                        RegisterIP = this.Request.GetRemoteIPAddress(),
                        CustomizeInfo = new CustomizeInfo(),
                        Experience = new Experience()
                    };
                    db.Accounts.Add(newAcc);
                    db.SaveChanges();

                    return HttpResponseFactory.Response200JSON(JsonConvert.SerializeObject(new
                    {
                        result = StatusMessages.STR_SUCCESS,
                    }));
                }
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
        public HttpResponseMessage ChangePassword([FromBody] ChangePassFormModel changePassData)
        {
            try
            {
                if (changePassData == null)
                {
                    return HttpResponseFactory.Response200Plain("error=No data received.");
                }

                // TODO - Probably some more validation like length and special characters contraints.
                if (String.IsNullOrEmpty(changePassData.Token)) changePassData.Token = String.Empty;
                if (String.IsNullOrEmpty(changePassData.Old_Pass)) changePassData.Old_Pass = String.Empty;
                if (String.IsNullOrEmpty(changePassData.New_Pass)) changePassData.New_Pass = String.Empty;

                SessionInstance mySession = SessionManager.Instance.GetSessionByToken(changePassData.Token);
                if (mySession == null)
                {
                    return HttpResponseFactory.Response200Plain("error=You are not logged in.");
                }

                using (DatabaseContext db = new DatabaseContext())
                {
                    Account me = db.Accounts.FirstOrDefault(a => a.Id == mySession.AccounData.UserId);
                    if (!Crypto.VerifyHashedPassword(me.PasswordHash, changePassData.Old_Pass))
                    {
                        return HttpResponseFactory.Response200Plain("error=You have entered a wrong password.");
                    }

                    me.PasswordHash = Crypto.HashPassword(changePassData.New_Pass);
                    db.SaveChanges();

                    return HttpResponseFactory.Response200Plain("message=The password has been changed succesfully!");
                }
            }
            catch (Exception ex)
            {
                return HttpResponseFactory.Response500Plain(ex.Message);
            }
        }
    }
}
