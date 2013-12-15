using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Common;
using Common.Extensions;
using DataAccess.Entities;
using log4net;
using WeatherService.Services;

namespace WeatherService.ServiceAggregator
{
    public class WeatherServiceAggregator : IWeatherServiceAggregator
    {
        private readonly IEnumerable<IWeatherService> _weatherServices;
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public WeatherServiceAggregator(IEnumerable<IWeatherService> weatherServices)
        {
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
            weatherInfo.LastUpdated = DateTime.UtcNow;

            return weatherInfo;
        }

        private WeatherInfo AggregateInternal(List<WeatherInfo> weatherInfos)
        {
            var weatherInfo = new WeatherInfo();

            weatherInfo.Country = weatherInfos.GetOneResult(info => info.Country != null, info => info.Country);
            weatherInfo.Description = weatherInfos.GetOneResult(info => info.Description != null, info => info.Description);
            weatherInfo.Elevation = weatherInfos.GetOneResult(info => info.Elevation != null, info => info.Elevation);
            weatherInfo.RelativeHumidity = weatherInfos.GetOneResult(info => info.RelativeHumidity != null, info => info.RelativeHumidity);
            weatherInfo.WindDirection = weatherInfos.GetOneResult(info => info.WindDirection != null, info => info.WindDirection);

            var latititudes = weatherInfos.GetMultipleResults(p => p.Latitude.HasValue, p => p.Latitude);
            weatherInfo.Latitude = latititudes.Any() ? latititudes.Average() : null;

            var longitudes = weatherInfos.GetMultipleResults(p => p.Longitude.HasValue, p => p.Longitude);
            weatherInfo.Longitude = longitudes.Any() ? longitudes.Average() : null;

            var pressuresInMb = weatherInfos.GetMultipleResults(p => p.PressureMb.HasValue, p => p.PressureMb);
            weatherInfo.PressureMb = pressuresInMb.Any() ? MathExtensions.Floor(pressuresInMb.Average()) : null;

            var temperaturesCelcius = weatherInfos.GetMultipleResults(p => p.TemperatureCelcius.HasValue, p => p.TemperatureCelcius);
            weatherInfo.TemperatureCelcius = temperaturesCelcius.Any() ? temperaturesCelcius.Average() : null;

            var visibilityDistances = weatherInfos.GetMultipleResults(p => p.VisibilityDistance.HasValue, p => p.VisibilityDistance);
            weatherInfo.VisibilityDistance = visibilityDistances.Any() ? visibilityDistances.Average() : null;

            var windAngles = weatherInfos.GetMultipleResults(p => p.WindAngle.HasValue, p => p.WindAngle);
            weatherInfo.WindAngle = windAngles.Any() ? MathExtensions.Floor(windAngles.Average()) : null;

            var windSpeedsKph = weatherInfos.GetMultipleResults(p => p.WindSpeedKph.HasValue, p => p.WindSpeedKph);
            weatherInfo.VisibilityDistance = windSpeedsKph.Any() ? windSpeedsKph.Average() : null;

            var windSpeedsMs = weatherInfos.GetMultipleResults(p => p.WindSpeedMs.HasValue, p => p.WindSpeedMs);
            weatherInfo.WindSpeedMs = windSpeedsMs.Any() ? windSpeedsMs.Average() : null;

            return weatherInfo;
        }
    }
}
