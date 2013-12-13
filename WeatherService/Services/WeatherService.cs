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
    public abstract class WeatherService<TModel, TMapper, TEntity> : IService<TEntity>
        where TModel : ServiceModel, new()
        where TMapper : IServiceModelMapper<TEntity, TModel>
        where TEntity : Entity
    {
        protected RestClient RestClient;
        protected string RequestedUrl;

        public TEntity GetWeatherInfo(string cityName, TEntity entity)
        {
            var requestString = String.Format(RequestedUrl, cityName);

            var request = new RestRequest(requestString, Method.GET);

            var response = RestClient.Execute<TModel>(request);
            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new IOException("Web service request failed");
            }

            var mapper = Ioc.Resolve<TMapper>();
            mapper.Map(entity, response.Data);

            return entity;
        }
    }
}
