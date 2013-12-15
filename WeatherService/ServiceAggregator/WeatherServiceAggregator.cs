using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Common;
using DataAccess.Entities;
using log4net;
using WeatherService.ServiceParameters;
using WeatherService.Services;

namespace WeatherService.ServiceAggregator
{
    public class WeatherServiceAggregator : IWeatherServiceAggregator
    {
        private readonly IEnumerable<IWeatherService> _weatherServices;
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public WeatherServiceAggregator(IEnumerable<IWeatherService> weatherServices)
        {
            _weatherServices = weatherServices;
        }

        public WeatherInfo Aggregate(string cityName)
        {
            var weatherInfo = new WeatherInfo();
            foreach (IWeatherService service in _weatherServices)
            {
                try
                {
                    service.GetWeatherInfo(cityName, weatherInfo);
                }
                catch (IOException e)
                {
                    Logger.Error("Data format not fit or network error", e);
                }
                catch (Exception e)
                {
                    Logger.Error("Not Expected type of error", e);
                }
            }

            weatherInfo.CityName = cityName;
            weatherInfo.LastUpdated = DateTime.UtcNow;

            return weatherInfo;
        }
    }
}
