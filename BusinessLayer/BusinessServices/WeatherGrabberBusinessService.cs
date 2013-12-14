using System;
using System.Collections.Generic;
using Common;
using DataAccess.Entities;
using DataAccess.Repository;
using WeatherService.ServiceAggregator;

namespace BusinessLayer.BusinessServices
{
    public class WeatherGrabberBusinessService : IWeatherGrabberBusinessService
    {
        public IEnumerable<WeatherInfo> GrabWeatherInfos(params string[] cityNames)
        {
            var cityRepository = Ioc.Resolve<IRepository<WeatherInfo>>();

            var weatherInfos = new List<WeatherInfo>();
            foreach (var city in cityNames)
            {
                var weatherInfo = cityRepository.Get(p => p.CityName == city);
                if (weatherInfo == null || (weatherInfo.LastUpdated - DateTime.UtcNow) > TimeSpan.FromHours(4))
                {
                    var aggregator = Ioc.Resolve<IServiceAggregator>();
                    var grabbedWeatherInfo = aggregator.AggregateWeatherInfo(city);
                    if (weatherInfo != null)
                    {
                        grabbedWeatherInfo.Id = weatherInfo.Id;
                        cityRepository.Update(grabbedWeatherInfo);
                    }
                    else
                    {
                        cityRepository.Save(grabbedWeatherInfo);

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
