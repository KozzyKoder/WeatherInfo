using DataAccess.Entities;

namespace WeatherService.ServiceAggregator
{
    public interface IWeatherServiceAggregator
    {
        WeatherInfo Aggregate(string cityName);
    }
}
