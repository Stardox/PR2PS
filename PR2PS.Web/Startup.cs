using Microsoft.AspNet.SignalR;
using Owin;
using PR2PS.Common.Constants;
using PR2PS.DataAccess.LevelsDataAccess;
using PR2PS.DataAccess.MainDataAccess;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
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
            using (MainContext mainDb = new MainContext(ConnectionStringKeys.PR2_MAIN_DB))
            {
                mainDb.Database.Initialize(false);
            }

            using (LevelsContext levelsDb = new LevelsContext(ConnectionStringKeys.PR2_LEVELS_DB))
            {
                levelsDb.Database.Initialize(false);
            }

            HttpConfiguration webApiConfiguration = new HttpConfiguration();
            webApiConfiguration.MapHttpAttributeRoutes();

            Container container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            container.RegisterWebApiRequest(() => new MainContext(ConnectionStringKeys.PR2_MAIN_DB), true);
            container.RegisterWebApiRequest<IMainDataAccessEngine, MainDataAccessEngine>();
            container.RegisterWebApiRequest(() => new LevelsContext(ConnectionStringKeys.PR2_LEVELS_DB), true);
            container.RegisterWebApiRequest<ILevelsDataAccessEngine, LevelsDataAccessEngine>();
            container.RegisterWebApiControllers(webApiConfiguration);
            container.Verify();
            webApiConfiguration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            app.UseWebApi(webApiConfiguration);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.MapSignalR(new HubConfiguration() { EnableDetailedErrors = false });
        }
    }
}
