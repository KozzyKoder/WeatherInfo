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
using WeatherService.ServiceParameters;

namespace WeatherService.Services
{
    public interface IService<TEntity, in TParameters> where TEntity : Entity
                                                    where TParameters : IServiceParameters
    {
        TEntity MakeRequest(TParameters parameters, TEntity entity);
    }
}