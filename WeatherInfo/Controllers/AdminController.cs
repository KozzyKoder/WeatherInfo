using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using DataAccess;
using DataAccess.Repository;

namespace WeatherInfo.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult CreateDatabase()
        {
            var databasePath = ConfigurationManager.AppSettings["DatabasePath"];
            var databaseFullPath = Server.MapPath(databasePath);

            SqliteConfigurator.CreateDatabase(databaseFullPath);

            return RedirectToAction("Index", "Home");
        }
    }
}
