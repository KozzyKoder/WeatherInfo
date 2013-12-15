using System;
using System.IO;
using System.Net;
using System.Reflection;
using Common;
using DataAccess.Entities;
using log4net;
using RestSharp;
using WeatherService.ServiceModelMappers;
using WeatherService.ServiceModels;
using WeatherService.ServiceParameters;

namespace WeatherService.Services
{
    public abstract class WeatherService<TModel, TMapper> : IWeatherService
        where TModel : IServiceModel, new()
        where TMapper : IServiceModelMapper<WeatherInfo, TModel>
    {
        protected readonly TMapper Mapper;
        protected RestClient RestClient;
        protected string RequestedUrl;
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected WeatherService(TMapper mapper)
        {
            Mapper = mapper;
        }

        public WeatherInfo GetWeatherInfo(string cityName, WeatherInfo entity)
        {
            var request = ProduceRequest(cityName);
            var model = ExecuteRequest(request, cityName);

            try
            {
                Mapper.Map(entity, model);
            }
            catch (Exception e)
            {
                var message = string.Format("Error occured while service model mapped in {0} weather service. Maybe, changed data format?", ServiceName());
                Logger.Error(message, e);
            }

            return entity;
        }

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
