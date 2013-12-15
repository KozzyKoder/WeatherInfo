using DataAccess.Entities;

namespace WeatherService.Services
{
    public interface IWeatherService
    {
        WeatherInfo GetWeatherInfo(string cityName, WeatherInfo entity);
    }
}