using System.Collections.Generic;
using RestSharp;

namespace WeatherService.Services
{
    public interface IWeatherService
    {
        Dictionary<string, string> GetWeatherInfo(string cityName);
    }

    public abstract class WeatherService : IWeatherService
    {
        protected RestClient _restClient;
        
        public Dictionary<string, string> GetWeatherInfo(string cityName)
        {
            var content = ProcessRequest(cityName);

            var parsedContent = ParseResponseContent(content, cityName);

            return parsedContent;
        }

        protected abstract string ProcessRequest(string cityName);

        protected abstract Dictionary<string, string> ParseResponseContent(string content, string cityName);
    }
}