using DataAccess.Entities;

namespace BusinessLayer.Services
{
    public interface IWeatherService
    {
        WeatherInfo GetWeatherInfo(string cityName);
        int Priority();
    }
}