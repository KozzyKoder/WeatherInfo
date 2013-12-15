using DataAccess.Entities;
using WeatherService.ServiceParameters;

namespace WeatherService.ServiceAggregator
{
    public interface IWeatherServiceAggregator
    {
        WeatherInfo Aggregate(string cityName);
    }
}
