using System.Linq;
using DataAccess.Entities;

namespace BusinessLayer.Services.OpenWeather
{
    public class OpenWeatherServiceModelMapper : IWeatherServiceModelMapper<OpenWeatherServiceModel>
    {
        public WeatherInfo Map(OpenWeatherServiceModel model)
        {
            var entity = new WeatherInfo
            {
                Country = model.Sys.Country,
                Latitude = model.Coord.Lat,
                Longitude = model.Coord.Lon,
                WindSpeedMs = model.Wind.Speed
            };
            var weather = model.Weather.FirstOrDefault();
            if (weather != null)
            {
                entity.Description = weather.Description;
            }
            return entity;
        }
    }
}