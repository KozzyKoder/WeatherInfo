using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using BusinessLayer;
using Common;
using DataAccess;
using log4net.Config;
using WeatherInfo.App_Start;
using WeatherService;

namespace WeatherInfo
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            XmlConfigurator.Configure();

            var databasePath = WeatherInfoConfiguration.DatabasePath;
            var databaseFullPath = Server.MapPath(databasePath);
            NhibernateConfig.Setup(databaseFullPath);

            var controllerFactory = new WeatherInfoControllerFactory(Ioc.Container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);

            Ioc.Container.Install(new WeatherServiceInstaller(),
                                  new DataAccessInstaller(),
                                  new BusinessLayerInstaller(),
                                  new CommonInstaller(),
                                  new WeatherInfoInstaller());

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}