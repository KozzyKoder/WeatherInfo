using System;
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
using WeatherService.Services;

namespace WeatherService.ServiceAggregator
{
    public interface IServiceAggregator
    {
        WeatherInfo AggregateWeatherInfo(string cityName);
    }

    public class ServiceAggregator : IServiceAggregator
    {
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public WeatherInfo AggregateWeatherInfo(string cityName)
        {
            var weatherInfo = new WeatherInfo();
            var services = Ioc.Container.ResolveAll(typeof (IService<WeatherInfo>));
            foreach (IService<WeatherInfo> service in services)
            {
                try
                {
                    service.GetWeatherInfo(cityName, weatherInfo);
                }
                catch (IOException e)
                {
                    Logger.Error("Error while working with Service", e);
                }
            }

            weatherInfo.CityName = cityName;
            weatherInfo.LastUpdated = DateTime.UtcNow;

            return weatherInfo;
        }
    }
}
