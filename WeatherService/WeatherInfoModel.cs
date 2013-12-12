﻿namespace WeatherService
{
    public class WeatherInfoModel
    {
        public string CityName { get; set; } 

        public string TemperatureCelcius { get; set; }

        public string Elevation { get; set; }

        public string PressureMb { get; set; }

        public string Longitude { get; set; }

        public string Latitude { get; set; }

        public string WindDirection { get; set; }

        public string WindSpeedKph { get; set; }

        public string WindAngle { get; set; }

        public string RelativeHumidity { get; set; }

        public string VisibilityDistance { get; set; }

        public string WindSpeedMs { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }
    }
}