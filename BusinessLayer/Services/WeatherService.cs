﻿using System;
using System.IO;
using System.Net;
using System.Reflection;
using DataAccess.Entities;
using log4net;
using RestSharp;

namespace BusinessLayer.Services
{
    public abstract class WeatherService<TModel, TMapper> : IWeatherService
        where TModel : IServiceModel, new()
        where TMapper : IWeatherServiceModelMapper<TModel>
    {
        protected readonly TMapper Mapper;
        protected RestClient RestClient;
        protected string RequestedUrl;
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected WeatherService(TMapper mapper)
        {
            Mapper = mapper;
        }

        public WeatherInfo GetWeatherInfo(string cityName)
        {
            var request = ProduceRequest(cityName);
            var model = ExecuteRequest(request, cityName);
            var weatherInfo = new WeatherInfo();

            try
            {
                weatherInfo = Mapper.Map(model);
            }
            catch (Exception e)
            {
                var message = string.Format("Error occured while service model mapped in {0} weather service. Maybe, changed data format?", ServiceName());
                Logger.Error(message, e);
            }

            return weatherInfo;
        }

        public abstract int Priority();

        public abstract string ServiceName();

        protected abstract RestRequest ProduceRequest(string cityName);

        protected virtual TModel ExecuteRequest(RestRequest request, string cityName)
        {
            var response = RestClient.Execute<TModel>(request);
            if ((response.StatusCode != HttpStatusCode.OK) || (response.ResponseStatus == ResponseStatus.Error))
            {
                var message = string.Format("Web service request to {0} failed for city {1}", ServiceName(), cityName);
                throw new IOException(message);
            }
            return response.Data;
        }
    }
}
