﻿using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using BusinessLayer.BusinessServices;
using Common;
using log4net;
using WeatherInfo.Models;

namespace WeatherInfo.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWeatherGrabberBusinessService _weatherGrabberBusinessService;
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public HomeController(IWeatherGrabberBusinessService weatherGrabberBusinessService)
        {
            _weatherGrabberBusinessService = weatherGrabberBusinessService;
        }

        public ActionResult Index()
        {
            var cities = WeatherInfoConfiguration.Cities;

            var weatherInfos = _weatherGrabberBusinessService.GrabWeatherInfos(cities).ToList();

            var model = new WeatherViewModel(weatherInfos);

            return View(model);
        }
    }
}
