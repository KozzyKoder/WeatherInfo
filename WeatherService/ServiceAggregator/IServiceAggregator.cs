using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess.Entities;
using log4net;
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
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public WeatherInfo AggregateWeatherInfo(string cityName)
        {
            var openWeatherService = Ioc.Resolve<IWeatherService<OpenWeatherServiceModel>>();
            var wundergroundService = Ioc.Resolve<IWeatherService<WundergroundServiceModel>>();
            
            var weatherInfo = new WeatherInfo();
            
            try
            {
                var openWeatherServiceModel = openWeatherService.GetWeatherInfo(cityName);
                weatherInfo.MapFromOpenWeatherServiceModel(openWeatherServiceModel);
            }
            catch (IOException e)
            {
                Logger.Error("Error while working with Open Weather Service", e);
            }
            try
            {
                var wundergroundServiceModel = wundergroundService.GetWeatherInfo(cityName);
                weatherInfo.MapFromWundergroundServiceModel(wundergroundServiceModel);
            }
            catch (IOException e)
            {
                Logger.Error("Error while working with Wunderground Service", e);
            }
            
            weatherInfo.CityName = cityName;
            weatherInfo.LastUpdated = DateTime.UtcNow;

            return weatherInfo;
        }
    }
}
