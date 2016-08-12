using Microsoft.AspNet.SignalR;
using Owin;
using PR2PS.DataAccess;
using System.Web.Http;

namespace PR2PS.Web
{
    /// <summary>
    /// TODO - Description.
    /// </summary>
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                db.Database.Initialize(false);
            }

            HttpConfiguration webApiConfiguration = ConfigureWebApi();
            app.UseWebApi(webApiConfiguration);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.MapSignalR(new HubConfiguration() { EnableDetailedErrors = false });
        }

        private HttpConfiguration ConfigureWebApi()
        {
            HttpConfiguration config = new HttpConfiguration();

            #region Routing

            // TODO - Attribute routing.

            #region Miscellaneous

            config.Routes.MapHttpRoute(
                "Welcome page",
                "",
                new { controller = "Misc", action = "GetWelcomeMessage" });

            config.Routes.MapHttpRoute(
                "Server status",
                "files/server_status_2.txt",
                new { controller = "Misc", action = "GetServerStatus" });

            config.Routes.MapHttpRoute(
                "Crossdomain policy",
                "crossdomain.xml",
                new { controller = "Misc", action = "GetPolicyFile" });

            config.Routes.MapHttpRoute(
                "Get player info",
                "get_player_info_2.php",
                new
                {
                    controller = "Misc",
                    action = "GetPlayerInfo",
                    name = "",
                    rand = "",
                    token = ""
                });

            #endregion

            #region Authentication/Deauthentication

            config.Routes.MapHttpRoute(
                "Check login",
                "check_login.php",
                new
                {
                    controller = "Auth",
                    action = "CheckLogin",
                    token = "",
                    rand = ""
                }
            );

            config.Routes.MapHttpRoute(
                "Log in",
                "login.php",
                new { controller = "Auth", action = "Login" });

            config.Routes.MapHttpRoute(
                "Log out",
                "logout.php",
                new
                {
                    controller = "Auth",
                    action = "Logout",
                    rand = "",
                    token = ""
                });

            config.Routes.MapHttpRoute(
                "Register",
                "register_user.php",
                new { controller = "Auth", action = "Register" });

            config.Routes.MapHttpRoute(
                "Change Password",
                "change_password.php",
                new { controller = "Auth", action = "ChangePassword" });

            #endregion

            #region Levels

            config.Routes.MapHttpRoute(
                "Campaign",
                "files/lists/campaign/{campaignId}",
                new
                {
                    controller = "Levels",
                    action = "GetCampaign",
                    campaignId = 1,
                    rand = "",
                    token = ""
                });

            config.Routes.MapHttpRoute(
               "Search Levels",
               "search_levels.php",
               new
               {
                   controller = "Levels", action = "SearchLevels"
               });

            #endregion

            #region Moderation

            config.Routes.MapHttpRoute(
                "Ban",
                "ban_user.php",
                new { controller = "Moderation", action = "Ban" });

            #endregion

            #region Messaging

            config.Routes.MapHttpRoute(
                "Get messages",
                "messages_get.php",
                new { controller = "Messaging", action = "GetMessages" });

            config.Routes.MapHttpRoute(
                "Send message",
                "message_send.php",
                new { controller = "Messaging", action = "SendMessage" });

            config.Routes.MapHttpRoute(
                "Report message",
                "message_report.php",
                new { controller = "Messaging", action = "ReportMessage" });

            config.Routes.MapHttpRoute(
                "Delete message",
                "message_delete.php",
                new { controller = "Messaging", action = "DeleteMessage" });

            config.Routes.MapHttpRoute(
                "Delete all messages",
                "messages_delete_all.php",
                new
                {
                    controller = "Messaging",
                    action = "DeleteAllMessages",
                    rand = "",
                    token = ""
                });

            #endregion

            //config.Routes.MapHttpRoute(
            //    "Default",
            //    "{*any}",
            //    new
            //    {
            //        controller = "Misc",
            //        action = "NotImplementedYet" });

            //config.Routes.MapHttpRoute(
            //    "DefaultApi",
            //    "api/{controller}/{id}",
            //    new { id = RouteParameter.Optional });

            #endregion

            return config;
        }
    }
}
