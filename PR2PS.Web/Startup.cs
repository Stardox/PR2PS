using Microsoft.AspNet.SignalR;
using Owin;
using PR2PS.DataAccess.Core;
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
            using (DatabaseContext db = new DatabaseContext("PR2Context"))
            {
                db.Database.Initialize(false);
            }

            HttpConfiguration webApiConfiguration = ConfigureWebApi();

            Container container = new Container();
            container.Options.DefaultScopedLifestyle = new WebApiRequestLifestyle();
            container.RegisterWebApiRequest(() => new DatabaseContext("PR2Context"), true);
            container.RegisterWebApiRequest<IDataAccessEngine, DataAccessEngine>();
            container.RegisterWebApiControllers(webApiConfiguration);
            container.Verify();
            webApiConfiguration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);

            app.UseWebApi(webApiConfiguration);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.MapSignalR(new HubConfiguration() { EnableDetailedErrors = false });
        }

        private HttpConfiguration ConfigureWebApi()
        {
            HttpConfiguration config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();

            return config;
        }
    }
}
