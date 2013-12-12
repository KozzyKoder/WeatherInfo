using System.Collections.Generic;

namespace WeatherService.Services
{
    public interface IWeatherService
    {
        Dictionary<string, string> GetWeatherInfo(string cityName);
    }
}