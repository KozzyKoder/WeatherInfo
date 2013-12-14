using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using Common;
using DataAccess.Entities;
using RestSharp;
using WeatherService.ServiceModelMappers;
using WeatherService.ServiceModels;

namespace WeatherService.Services
{
    public interface IService<TEntity> where TEntity : Entity
    {
        TEntity GetWeatherInfo(string cityName, TEntity entity);
    }
}