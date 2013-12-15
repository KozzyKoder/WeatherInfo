using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Common;
using Common.Extensions;
using DataAccess.Entities;
using log4net;
using IWeatherService = BusinessLayer.Services.IWeatherService;

namespace BusinessLayer.ServiceAggregator
{
    public class WeatherServiceAggregator : IWeatherServiceAggregator
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IEnumerable<IWeatherService> _weatherServices;
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public WeatherServiceAggregator(IEnumerable<IWeatherService> weatherServices, IDateTimeProvider dateTimeProvider)
        {
            _dateTimeProvider = dateTimeProvider;
            _weatherServices = weatherServices.OrderByDescending(p => p.Priority());
        }

        public WeatherInfo Aggregate(string cityName)
        {
            var grabbedWeatherInfos = new List<WeatherInfo>();
            foreach (IWeatherService service in _weatherServices)
            {
                try
                {
                    grabbedWeatherInfos.Add(service.GetWeatherInfo(cityName));
                }
                catch (IOException e)
                {
                    Logger.Error("Data format not fit or network error", e);
                }
                catch (Exception e)
                {
                    //todo we are not living in ideal world, so everything could happen
                    Logger.Error("Not Expected type of error", e);
                    throw;
                }
            }

            var weatherInfo = AggregateInternal(grabbedWeatherInfos);
            weatherInfo.CityName = cityName;
            weatherInfo.LastUpdated = _dateTimeProvider.UtcNow();

            return weatherInfo;
        }

        private WeatherInfo AggregateInternal(List<WeatherInfo> weatherInfos)
        {
            var weatherInfo = new WeatherInfo();

            var existingWeatherInfos = weatherInfos.Where(p => p != null).ToList();

            weatherInfo.Country = existingWeatherInfos.GetOneResult(info => info.Country != null, info => info.Country);
            weatherInfo.Description = existingWeatherInfos.GetOneResult(info => info.Description != null, info => info.Description);
            weatherInfo.Elevation = existingWeatherInfos.GetOneResult(info => info.Elevation != null, info => info.Elevation);
            weatherInfo.RelativeHumidity = existingWeatherInfos.GetOneResult(info => info.RelativeHumidity != null, info => info.RelativeHumidity);
            weatherInfo.WindDirection = existingWeatherInfos.GetOneResult(info => info.WindDirection != null, info => info.WindDirection);

            var latititudes = existingWeatherInfos.GetMultipleResults(p => p.Latitude.HasValue, p => p.Latitude);
            weatherInfo.Latitude = latititudes.Any() ? latititudes.Average() : null;

            var longitudes = existingWeatherInfos.GetMultipleResults(p => p.Longitude.HasValue, p => p.Longitude);
            weatherInfo.Longitude = longitudes.Any() ? longitudes.Average() : null;

            var pressuresInMb = existingWeatherInfos.GetMultipleResults(p => p.PressureMb.HasValue, p => p.PressureMb);
            weatherInfo.PressureMb = pressuresInMb.Any() ? MathExtensions.Floor(pressuresInMb.Average()) : null;

            var temperaturesCelcius = existingWeatherInfos.GetMultipleResults(p => p.TemperatureCelcius.HasValue, p => p.TemperatureCelcius);
            weatherInfo.TemperatureCelcius = temperaturesCelcius.Any() ? temperaturesCelcius.Average() : null;

            var visibilityDistances = existingWeatherInfos.GetMultipleResults(p => p.VisibilityDistance.HasValue, p => p.VisibilityDistance);
            weatherInfo.VisibilityDistance = visibilityDistances.Any() ? visibilityDistances.Average() : null;

            var windAngles = existingWeatherInfos.GetMultipleResults(p => p.WindAngle.HasValue, p => p.WindAngle);
            weatherInfo.WindAngle = windAngles.Any() ? MathExtensions.Floor(windAngles.Average()) : null;

            var windSpeedsKph = existingWeatherInfos.GetMultipleResults(p => p.WindSpeedKph.HasValue, p => p.WindSpeedKph);
            weatherInfo.WindSpeedKph = windSpeedsKph.Any() ? windSpeedsKph.Average() : null;

            var windSpeedsMs = existingWeatherInfos.GetMultipleResults(p => p.WindSpeedMs.HasValue, p => p.WindSpeedMs);
            weatherInfo.WindSpeedMs = windSpeedsMs.Any() ? windSpeedsMs.Average() : null;

            return weatherInfo;
        }
    }
}
