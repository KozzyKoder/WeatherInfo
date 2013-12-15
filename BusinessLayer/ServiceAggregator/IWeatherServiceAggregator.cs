using DataAccess.Entities;

namespace BusinessLayer.ServiceAggregator
{
    public interface IWeatherServiceAggregator
    {
        WeatherInfo Aggregate(string cityName);
    }
}
