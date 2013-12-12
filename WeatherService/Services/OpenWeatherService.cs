using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace WeatherService.Services
{
    public class OpenWeatherService : WeatherService
    {
        public OpenWeatherService()
        {
            _restClient = new RestClient("http://api.openweathermap.org");
        }
        
        protected override string ProcessRequest(string cityName)
        {
            var requestString = String.Format("/data/2.5/weather?q={0}", cityName);

            var request = new RestRequest(requestString, Method.GET);

            var response = _restClient.Execute(request);

            var content = response.Content;

            return content;
        }

        protected override Dictionary<string, string> ParseResponseContent(string content, string cityName)
        {
            var parsedValues = new Dictionary<string, string>();
            
            var deserializedContent = JsonConvert.DeserializeObject<JObject>(content);

            var coordData = deserializedContent["coord"];
            var systemData = deserializedContent["sys"];
            var windData = deserializedContent["wind"];
            var weatherArray = deserializedContent["weather"];
            var weatherInfo = weatherArray.First();
            
            parsedValues["Description"] = weatherInfo["description"].ToString();

            parsedValues["WindSpeedMs"] = windData["speed"].ToString();

            parsedValues["Country"] = systemData["country"].ToString();

            parsedValues["Latitude"] = coordData["lat"].ToString();
            parsedValues["Longitude"] = coordData["lon"].ToString();

            return parsedValues;
        }
    }
}