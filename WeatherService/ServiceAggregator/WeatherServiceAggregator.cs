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
    public class WeatherServiceAggregator : IServiceAggregator<WeatherInfo, WeatherServiceParameters>
    {
        private readonly IEnumerable<IService<WeatherInfo, WeatherServiceParameters>> _weatherServices;
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public WeatherServiceAggregator(IEnumerable<IService<WeatherInfo, WeatherServiceParameters>> weatherServices)
        {
            _weatherServices = weatherServices;
        }

        public WeatherInfo AggregateServicesInfo(WeatherServiceParameters parameters)
        {
            var weatherInfo = new WeatherInfo();
            foreach (IService<WeatherInfo, WeatherServiceParameters> service in _weatherServices)
            {
                try
                {
                    service.MakeRequest(parameters, weatherInfo);
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

            weatherInfo.CityName = parameters.CityName;
            weatherInfo.LastUpdated = DateTime.UtcNow;

            return weatherInfo;
        }
    }
}
