using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using WeatherService.ServiceModels;

namespace WeatherService.Services
{
    public class WundergroundWeatherService : WeatherService<WundergroundServiceModel>
    {
        public WundergroundWeatherService()
        {
            RestClient = new RestClient("http://api.wunderground.com");
            RequestedUrl = "/api/ce44da16196c57b7/conditions/q/CA/{0}.json";
        }
    }
}