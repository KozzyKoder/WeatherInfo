﻿using System;
using RestSharp;
using WeatherService.ServiceModelMappers;
using WeatherService.ServiceModels;

namespace WeatherService.Services
{
    public class OpenWeatherService : WeatherService<OpenWeatherServiceModel, OpenWeatherServiceModelMapper>
    {
        public OpenWeatherService()
        {
            RestClient = new RestClient("http://api.openweathermap.org");
        }

        public override string ServiceName()
        {
            return "Open Weather Service";
        }

        protected override RestRequest ProduceRequest(string cityName)
        {
            var requestString = String.Format("/data/2.5/weather?q={0}", cityName);

            var request = new RestRequest(requestString, Method.GET);

            return request;
        }
    }
}