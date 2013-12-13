using System.Linq;
using DataAccess.Entities;
using WeatherService.ServiceModels;

namespace WeatherService.ServiceModelMappers
{
    public class OpenWeatherServiceModelMapper : IServiceModelMapper<WeatherInfo, OpenWeatherServiceModel>
    {
        public void Map(WeatherInfo entity, OpenWeatherServiceModel model)
        {
            entity.Country = model.Sys.Country;
            entity.Latitude = model.Coord.Lat;
            entity.Longitude = model.Coord.Lon;
            var weather = model.Weather.FirstOrDefault();
            if (weather != null)
            {
                entity.Description = weather.Description;
            }
            entity.WindSpeedMs = model.Wind.Speed;
        }
    }
}