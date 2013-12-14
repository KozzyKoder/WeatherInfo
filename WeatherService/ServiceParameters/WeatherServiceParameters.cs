using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherService.ServiceParameters
{
    public class WeatherServiceParameters : IServiceParameters
    {
        public string CityName { get; set; }

        public WeatherServiceParameters(string cityName)
        {
            CityName = cityName;
        }
    }
}
