using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using WeatherService.ServiceModels;

namespace WeatherService.Services
{
    public class OpenWeatherService : WeatherService<OpenWeatherServiceModel>
    {
        public OpenWeatherService()
        {
            RestClient = new RestClient("http://api.openweathermap.org");
            RequestedUrl = "/data/2.5/weather?q={0}";
        }
    }
}