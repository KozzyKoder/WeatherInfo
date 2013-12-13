using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using RestSharp;

namespace WeatherService.Services
{
    public interface IWeatherService<out T> where T :class, new()
    {
        T GetWeatherInfo(string cityName);
    }

    public abstract class WeatherService<T> : IWeatherService<T> where T : class, new()
    {
        protected RestClient RestClient;
        protected string RequestedUrl;
        
        public T GetWeatherInfo(string cityName)
        {
            var requestString = String.Format(RequestedUrl, cityName);

            var request = new RestRequest(requestString, Method.GET);

            var response = RestClient.Execute<T>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new IOException("Web service request failed");
            }

            return response.Data;
        }
    }
}