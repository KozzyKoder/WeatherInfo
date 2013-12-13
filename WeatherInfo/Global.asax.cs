using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Common;
using DataAccess;
using DataAccess.Repository;
using log4net.Config;
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

            var databasePath = ConfigurationManager.AppSettings["DatabasePath"];
            var databaseFullPath = Server.MapPath(databasePath);
            NhibernateConfig.Setup(databaseFullPath);

            Ioc.Container.Install(new WeatherServiceInstaller(), new DataAccessInstaller());

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}