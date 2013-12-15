using System;
using System.Collections.Generic;

namespace WeatherService.ServiceModels
{
    public class OpenWeatherServiceModel : IServiceModel
    {
        public OpenWeatherCoordinates Coord { get; set; }
        public OpenWeatherSystem Sys { get; set; }
        public OpenWeatherWind Wind { get; set; }
        public List<OpenWeatherWeather> Weather { get; set; }
    }

    public class OpenWeatherCoordinates
    {
        public Double Lon { get; set; }
        public Double Lat { get; set; }
    }

    public class OpenWeatherSystem
    {
        public string Country { get; set; }
    }

    public class OpenWeatherWind
    {
        public Double Speed { get; set; }
    }

    public class OpenWeatherWeather
    {
        public string Description { get; set; }
    }
}
