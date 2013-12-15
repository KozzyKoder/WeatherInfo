using System.Linq;
using DataAccess.Entities;
using WeatherService.ServiceModels;

namespace WeatherService.ServiceModelMappers
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