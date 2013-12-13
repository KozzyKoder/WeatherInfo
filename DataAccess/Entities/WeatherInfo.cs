using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class WeatherInfo : Entity
    {
        public virtual string CityName { get; set; }

        public virtual Double TemperatureCelcius { get; set; }

        public virtual string Elevation { get; set; }

        public virtual int PressureMb { get; set; }

        public virtual Double Longitude { get; set; }

        public virtual Double Latitude { get; set; }

        public virtual string WindDirection { get; set; }

        public virtual Double WindSpeedKph { get; set; }

        public virtual int WindAngle { get; set; }

        public virtual string RelativeHumidity { get; set; }

        public virtual Double VisibilityDistance { get; set; }

        public virtual Double WindSpeedMs { get; set; }

        public virtual string Description { get; set; }

        public virtual string Country { get; set; }
    }
}
