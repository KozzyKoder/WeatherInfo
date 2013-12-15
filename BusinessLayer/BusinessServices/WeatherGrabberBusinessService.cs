using System;
using System.Collections.Generic;
using BusinessLayer.ServiceAggregator;
using Common;
using DataAccess.Entities;
using DataAccess.Repository;

namespace BusinessLayer.BusinessServices
{
    public class WeatherGrabberBusinessService : IWeatherGrabberBusinessService
    {
        private readonly IRepository<WeatherInfo> _weatherInfoRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IWeatherServiceAggregator _weatherWeatherServiceAggregator;
        private readonly Dictionary<string, WeatherInfo> _cache;

        public WeatherGrabberBusinessService(IRepository<WeatherInfo> weatherInfoRepository,
                                             IDateTimeProvider dateTimeProvider,
                                             IWeatherServiceAggregator weatherWeatherServiceAggregator)
        {
            _weatherInfoRepository = weatherInfoRepository;
            _dateTimeProvider = dateTimeProvider;
            _weatherWeatherServiceAggregator = weatherWeatherServiceAggregator;
            _cache = new Dictionary<string, WeatherInfo>();
        }

        public IEnumerable<WeatherInfo> GrabWeatherInfos(List<string> cityNames)
        {
            var weatherInfos = new List<WeatherInfo>();
            foreach (var cityName in cityNames)
            {
                string city = cityName;
                var weatherInfo = _cache.ContainsKey(city) ? _cache[city] : _weatherInfoRepository.Get(p => p.CityName == city);

                if ((weatherInfo == null) || (_dateTimeProvider.UtcNow() - weatherInfo.LastUpdated) > TimeSpan.FromHours(4))
                {
                    var grabbedWeatherInfo = _weatherWeatherServiceAggregator.Aggregate(city);
                    if (weatherInfo != null)
                    {
                        grabbedWeatherInfo.Id = weatherInfo.Id;
                        _weatherInfoRepository.Update(grabbedWeatherInfo);
                        _cache[city] = grabbedWeatherInfo;
                    }
                    else
                    {
                        _weatherInfoRepository.Save(grabbedWeatherInfo);
                        _cache[city] = grabbedWeatherInfo;
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
