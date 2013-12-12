using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace WeatherService.Services
{
    public class WundergroundWeatherService : WeatherService
    {
        public WundergroundWeatherService()
        {
            _restClient = new RestClient("http://api.wunderground.com");
        }

        protected override string ProcessRequest(string cityName)
        {
            var requestString = String.Format("/api/ce44da16196c57b7/conditions/q/CA/{0}.json", cityName);

            var request = new RestRequest(requestString, Method.GET);

            var response = _restClient.Execute(request);

            var content = response.Content;

            return content;
        }

        protected override Dictionary<string, string> ParseResponseContent(string content, string cityName)
        {
            var parsedValues = new Dictionary<string, string>();
            
            var deserializedContent = JsonConvert.DeserializeObject<JObject>(content);

            var currentObservation = deserializedContent["current_observation"];
            var observationLocation = currentObservation["observation_location"];

            parsedValues["TemperatureCelcius"] = currentObservation["temp_c"].ToString();
            parsedValues["RelativeHumidity"] = currentObservation["relative_humidity"].ToString();
            parsedValues["PressureMb"] = currentObservation["pressure_mb"].ToString();
            parsedValues["VisibilityDistance"] = currentObservation["visibility_km"].ToString();
            parsedValues["WindSpeedKph"] = currentObservation["wind_kph"].ToString();
            parsedValues["WindAngle"] = currentObservation["wind_degrees"].ToString();
            parsedValues["WindDirection"] = currentObservation["wind_dir"].ToString();

            parsedValues["Elevation"] = observationLocation["elevation"].ToString();
            
            parsedValues["CityName"] = cityName;

            return parsedValues;
        }
    }
}