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
        private readonly IRepository<WeatherInfo> _weatherInfoRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IServiceAggregator<WeatherInfo, WeatherServiceParameters> _weatherServiceAggregator;

        public WeatherGrabberBusinessService(IRepository<WeatherInfo> weatherInfoRepository,
                                             IDateTimeProvider dateTimeProvider,
                                             IServiceAggregator<WeatherInfo, WeatherServiceParameters> weatherServiceAggregator)
        {
            _weatherInfoRepository = weatherInfoRepository;
            _dateTimeProvider = dateTimeProvider;
            _weatherServiceAggregator = weatherServiceAggregator;
        }

        public IEnumerable<WeatherInfo> GrabWeatherInfos(List<string> cityNames)
        {
            var weatherInfos = new List<WeatherInfo>();
            foreach (var cityName in cityNames)
            {
                string city = cityName;
                var weatherServiceParameters = new WeatherServiceParameters(cityName);
                var weatherInfo = _weatherInfoRepository.Get(p => p.CityName == city);
                if ((weatherInfo == null) || (_dateTimeProvider.UtcNow() - weatherInfo.LastUpdated) > TimeSpan.FromHours(4))
                {
                    var grabbedWeatherInfo = _weatherServiceAggregator.AggregateServicesInfo(weatherServiceParameters);
                    if (weatherInfo != null)
                    {
                        grabbedWeatherInfo.Id = weatherInfo.Id;
                        _weatherInfoRepository.Update(grabbedWeatherInfo);
                    }
                    else
                    {
                        _weatherInfoRepository.Save(grabbedWeatherInfo);

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
