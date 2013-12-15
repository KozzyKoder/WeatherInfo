using System;
using DataAccess.Entities;
using RestSharp;
using WeatherService.ServiceModelMappers;
using WeatherService.ServiceModels;

namespace WeatherService.Services
{
    public class WundergroundWeatherService : WeatherService<WundergroundServiceModel, IServiceModelMapper<WeatherInfo, WundergroundServiceModel>>
    {
        public WundergroundWeatherService(IServiceModelMapper<WeatherInfo, WundergroundServiceModel> mapper): base(mapper)
        {
            RestClient = new RestClient("http://api.wunderground.com");
        }

        public override string ServiceName()
        {
            return "Wunderground Weather Service";
        }

        protected override RestRequest ProduceRequest(string cityName)
        {
            var requestString = String.Format("/api/ce44da16196c57b7/conditions/q/CA/{0}.json", cityName);

            var request = new RestRequest(requestString, Method.GET);

            return request;
        }
    }
}