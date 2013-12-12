using System;
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
            Dictionary<string, string> parsedContent;
            try
            {
                string content = ProcessRequest(cityName);
                parsedContent = ParseResponseContent(content, cityName);
            }
            catch (Exception)
            {
                parsedContent = new Dictionary<string, string>();
            }

            return parsedContent;
        }

        protected abstract string ProcessRequest(string cityName);

        protected abstract Dictionary<string, string> ParseResponseContent(string content, string cityName);
    }
}