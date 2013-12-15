using System;
using DataAccess.Entities;
using RestSharp;
using WeatherService.ServiceModelMappers;
using WeatherService.ServiceModels;

namespace WeatherService.Services
{
    public class OpenWeatherService : WeatherService<OpenWeatherServiceModel, IWeatherServiceModelMapper<OpenWeatherServiceModel>>
    {
        public OpenWeatherService(IWeatherServiceModelMapper<OpenWeatherServiceModel> mapper) : base(mapper)
        {
            RestClient = new RestClient("http://api.openweathermap.org");
        }

        public override int Priority()
        {
            return 1;
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