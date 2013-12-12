using System.Linq;
using DataAccess.Entities;
using WeatherService.ServiceModels;

namespace WeatherService.Extensions
{
    public static class CompanyInfoExtensions
    {
        public static void MapFromOpenWeatherServiceModel(this WeatherInfo info, OpenWeatherServiceModel model)
        {
            info.Country = model.Sys.Country;
            info.Latitude = model.Coord.Lat;
            info.Longitude = model.Coord.Lon;
            var weather = model.Weather.FirstOrDefault();
            if (weather != null)
            {
                info.Description = weather.Description;
            }
            info.WindSpeedMs = model.Wind.Speed;
        }

        public static void MapFromWundergroundServiceModel(this WeatherInfo info, WundergroundServiceModel model)
        {
            var co = model.CurrentObservation;
            info.Elevation = co.ObservationLocation.Elevation;
            info.PressureMb = co.PressureMb;
            info.RelativeHumidity = co.RelativeHumidity;
            info.VisibilityDistance = co.VisibilityKm;
            info.TemperatureCelcius = co.TempC;
            info.WindAngle = co.WindDegrees;
            info.WindDirection = co.WindDir;
            info.WindSpeedKph = co.WindKph;
        }
    }
}
