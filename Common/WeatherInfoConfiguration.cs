using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Common
{
    public static class WeatherInfoConfiguration
    {
        public static List<string> Cities
        {
            get
            {
                var cities = ConfigurationManager.AppSettings["Cities"];
                return cities.Split(new[] { ',' }).ToList();
            }
        }

        public static string DatabasePath
        {
            get
            {
                return ConfigurationManager.AppSettings["DatabasePath"];
            }
        }
    }
}
