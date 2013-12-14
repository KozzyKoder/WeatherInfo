using System;
using System.Collections.Generic;
using Common;
using DataAccess.Entities;
using DataAccess.Repository;
using WeatherService.ServiceAggregator;
using WeatherService.ServiceParameters;

namespace BusinessLayer.BusinessServices
{
    public class WeatherGrabberBusinessService : IWeatherGrabberBusinessService
    {
        public IEnumerable<WeatherInfo> GrabWeatherInfos(params string[] cityNames)
        {
            var weatherInfosRepository = Ioc.Resolve<IRepository<WeatherInfo>>();
            var dateTimeProvider = Ioc.Resolve<IDateTimeProvider>();

            var weatherInfos = new List<WeatherInfo>();
            foreach (var city in cityNames)
            {
                var weatherServiceParameters = new WeatherServiceParameters(city);
                var weatherInfo = weatherInfosRepository.Get(p => p.CityName == city);
                if ((weatherInfo == null) || (dateTimeProvider.UtcNow() - weatherInfo.LastUpdated) > TimeSpan.FromHours(4))
                {
                    var aggregator = Ioc.Resolve<IServiceAggregator<WeatherInfo, WeatherServiceParameters>>();
                    var grabbedWeatherInfo = aggregator.AggregateServicesInfo(weatherServiceParameters);
                    if (weatherInfo != null)
                    {
                        grabbedWeatherInfo.Id = weatherInfo.Id;
                        weatherInfosRepository.Update(grabbedWeatherInfo);
                    }
                    else
                    {
                        weatherInfosRepository.Save(grabbedWeatherInfo);

                    }
                    weatherInfos.Add(grabbedWeatherInfo);
                }
                else
                {
                    weatherInfos.Add(weatherInfo);
                }
            }

            return weatherInfos;
        }
    }
}
