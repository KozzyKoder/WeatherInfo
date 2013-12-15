using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using BusinessLayer.BusinessServices;
using Common;
using DataAccess.Repository;
using log4net;
using WeatherInfo.Models;
using WeatherService.ServiceAggregator;

namespace WeatherInfo.Controllers
{
    public class HomeController : Controller
    {
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public ActionResult Index()
        {
            var weatherInfosGrabberBusinessService = Ioc.Resolve<IWeatherGrabberBusinessService>();
            var cities = WeatherInfoConfiguration.Cities;

            var weatherInfos = weatherInfosGrabberBusinessService.GrabWeatherInfos(cities.ToArray()).ToList();

            var model = new WeatherViewModel(weatherInfos);

            return View(model);
        }
    }
}
