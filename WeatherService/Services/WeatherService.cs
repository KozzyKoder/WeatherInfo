using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess.Entities;
using RestSharp;
using WeatherService.ServiceModelMappers;
using WeatherService.ServiceModels;

namespace WeatherService.Services
{
    public abstract class WeatherService<TModel, TMapper> : IService<WeatherInfo>
        where TModel : ServiceModel, new()
        where TMapper : IServiceModelMapper<WeatherInfo, TModel>
    {
        protected RestClient RestClient;
        protected string RequestedUrl;

        public WeatherInfo GetWeatherInfo(string cityName, WeatherInfo entity)
        {
            var serviceModel = ProduceAndExecuteRequest(cityName);

            var mapper = Ioc.Resolve<TMapper>();
            mapper.Map(entity, serviceModel);

            return entity;
        }

        public abstract string ServiceName();

        protected TModel ProduceAndExecuteRequest(string cityName)
        {
            var request = ProduceRequest(cityName);
            var model = ExecuteRequest(request, cityName);
            return model;
        }

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
