using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using AutoMapper;
using Castle.Windsor;
using Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using WeatherService;
using WeatherService.ServiceAggregator;
using WeatherService.Services;

namespace WeatherServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Ioc.Container.Install(new WeatherServiceInstaller());
            
            var aggregator = Ioc.Resolve<IServiceAggregator>();

            var model = aggregator.AggregateWeatherInfo("Chelyabinsk");

            Console.ReadKey();
        }
    }
}
