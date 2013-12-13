﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using DataAccess.Repository;
using WeatherInfo.Models;
using WeatherService.ServiceAggregator;

namespace WeatherInfo.Controllers
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index()
        {
            var cityRepository = Ioc.Resolve<IRepository<DataAccess.Entities.WeatherInfo>>();

            var cities = GetCitiesList();

            var weatherInfos = new List<DataAccess.Entities.WeatherInfo>();
            foreach (var city in cities)
            {
                var weatherInfo = cityRepository.Get(p => p.CityName == city);
                if (weatherInfo == null || (weatherInfo.LastUpdated - DateTime.UtcNow) > TimeSpan.FromHours(4))
                {
                    var aggregator = Ioc.Resolve<IServiceAggregator>();
                    var grabbedWeatherInfo = aggregator.AggregateWeatherInfo(city);
                    if (weatherInfo != null)
                    {
                        grabbedWeatherInfo.Id = weatherInfo.Id;
                        cityRepository.Update(grabbedWeatherInfo);
                    }
                    else
                    {
                        cityRepository.Save(grabbedWeatherInfo);
                        
                    }
                    weatherInfos.Add(grabbedWeatherInfo);
                }
                else
                {
                    weatherInfos.Add(weatherInfo);
                }
            }

            var model = new WeatherViewModel()
            {
                WeatherInfos = weatherInfos
            };

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
