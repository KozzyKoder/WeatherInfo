﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess.Entities;
using log4net;
using RestSharp.Extensions;
using WeatherService.ServiceModelMappers;
using WeatherService.ServiceModels;
using WeatherService.ServiceParameters;
using WeatherService.Services;

namespace WeatherService.ServiceAggregator
{
    public interface IServiceAggregator<out TModel, in TParameters> where TModel : Entity
                                                                    where TParameters : IServiceParameters
    {
        TModel AggregateServicesInfo(TParameters parameters);
    }
}
