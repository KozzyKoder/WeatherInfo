using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class WeatherInfo
    {
        public string CityName { get; set; }

        public Double TemperatureCelcius { get; set; }

        public string Elevation { get; set; }

        public int PressureMb { get; set; }

        public Double Longitude { get; set; }

        public Double Latitude { get; set; }

        public string WindDirection { get; set; }

        public Double WindSpeedKph { get; set; }

        public int WindAngle { get; set; }

        public string RelativeHumidity { get; set; }

        public Double VisibilityDistance { get; set; }

        public Double WindSpeedMs { get; set; }

        public string Description { get; set; }

        public string Country { get; set; }
    }
}
