using System.Collections.Generic;

namespace WeatherInfo.Models
{
    public class WeatherViewModel
    {
        public List<DataAccess.Entities.WeatherInfo> WeatherInfos;

        public WeatherViewModel(List<DataAccess.Entities.WeatherInfo> weatherInfos)
        {
            WeatherInfos = weatherInfos;
        }
    }
}