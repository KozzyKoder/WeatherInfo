using System;
using System.Collections.Generic;
using DataAccess.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using WeatherService.ServiceModelMappers;
using WeatherService.ServiceModels;

namespace WeatherService.Services
{
    public class WundergroundWeatherService : WeatherService<WundergroundServiceModel, WundergroundServiceModelMapper, WeatherInfo>
    {
        public WundergroundWeatherService()
        {
            RestClient = new RestClient("http://api.wunderground.com");
            RequestedUrl = "/api/ce44da16196c57b7/conditions/q/CA/{0}.json";
        }
    }
}