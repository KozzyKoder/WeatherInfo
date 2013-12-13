using DataAccess.Entities;
using WeatherService.ServiceModels;

namespace WeatherService.ServiceModelMappers
{
    public class WundergroundServiceModelMapper : IServiceModelMapper<WeatherInfo, WundergroundServiceModel>
    {
        public void Map(WeatherInfo entity, WundergroundServiceModel model)
        {
            var co = model.CurrentObservation;
            entity.Elevation = co.ObservationLocation.Elevation;
            entity.PressureMb = co.PressureMb;
            entity.RelativeHumidity = co.RelativeHumidity;
            entity.VisibilityDistance = co.VisibilityKm;
            entity.TemperatureCelcius = co.TempC;
            entity.WindAngle = co.WindDegrees;
            entity.WindDirection = co.WindDir;
            entity.WindSpeedKph = co.WindKph;
        }
    }
}