﻿using DataAccess.Entities;

namespace BusinessLayer.Services.Wunderground
{
    public class WundergroundWeatherServiceModelMapper : IWeatherServiceModelMapper<BusinessLayer.Services.Wunderground.WundergroundServiceModel>
    {
        public WeatherInfo Map(WundergroundServiceModel model)
        {
            var co = model.CurrentObservation;
            var entity = new WeatherInfo
            {
                Elevation = co.ObservationLocation.Elevation,
                PressureMb = co.PressureMb,
                RelativeHumidity = co.RelativeHumidity,
                VisibilityDistance = co.VisibilityKm,
                TemperatureCelcius = co.TempC,
                WindAngle = co.WindDegrees,
                WindDirection = co.WindDir,
                WindSpeedKph = co.WindKph
            };

            return entity;
        }
    }
}