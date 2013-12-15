using DataAccess.Entities;
using WeatherService.ServiceParameters;

namespace WeatherService.Services
{
    public interface IWeatherService
    {
        WeatherInfo GetWeatherInfo(string cityName, WeatherInfo entity);
    }
}