using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess.Entities;
using WeatherService.Extensions;
using WeatherService.ServiceModels;
using WeatherService.Services;

namespace WeatherService.ServiceAggregator
{
    public interface IServiceAggregator
    {
        WeatherInfo AggregateWeatherInfo(string cityName);
    }

    public class ServiceAggregator : IServiceAggregator
    {
        public WeatherInfo AggregateWeatherInfo(string cityName)
        {
            var openWeatherService = Ioc.Resolve<IWeatherService<OpenWeatherServiceModel>>();
            var wundergroundService = Ioc.Resolve<IWeatherService<WundergroundServiceModel>>();

            var openWeatherServiceModel = openWeatherService.GetWeatherInfo(cityName);
            var wundergroundServiceModel = wundergroundService.GetWeatherInfo(cityName);

            var weatherInfo = new WeatherInfo();
            weatherInfo.MapFromOpenWeatherServiceModel(openWeatherServiceModel);
            weatherInfo.MapFromWundergroundServiceModel(wundergroundServiceModel);
            
            weatherInfo.CityName = cityName;

            return weatherInfo;
        }
    }
}
