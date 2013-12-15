using System.IO;
using System.Net;
using Common;
using DataAccess.Entities;
using RestSharp;
using WeatherService.ServiceModelMappers;
using WeatherService.ServiceModels;
using WeatherService.ServiceParameters;

namespace WeatherService.Services
{
    public abstract class WeatherService<TModel, TMapper> : IService<WeatherInfo, WeatherServiceParameters>
        where TModel : IServiceModel, new()
        where TMapper : IServiceModelMapper<WeatherInfo, TModel>
    {
        protected readonly TMapper Mapper;
        protected RestClient RestClient;
        protected string RequestedUrl;

        protected WeatherService(TMapper mapper)
        {
            Mapper = mapper;
        }

        public WeatherInfo MakeRequest(WeatherServiceParameters parameters, WeatherInfo entity)
        {
            var serviceModel = ProduceAndExecuteRequest(parameters.CityName);

            Mapper.Map(entity, serviceModel);

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
