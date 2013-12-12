using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using AutoMapper;
using Castle.Windsor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using WeatherService;
using WeatherService.Services;

namespace WeatherServiceClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new WindsorContainer();
            container.Install(new WeatherServiceInstaller());

            Mapper
            .CreateMap<Dictionary<string, string>, WeatherInfoModel>()
            .ConvertUsing(x =>
            {
                var serializer = new JavaScriptSerializer();
                return serializer.Deserialize<WeatherInfoModel>(serializer.Serialize(x));
            });

            var services = container.ResolveAll<IWeatherService>();

            var allWeatherProperties = new Dictionary<string, string>();
            foreach (var weatherService in services)
            {
                var currentServiceWeatherProperties = weatherService.GetWeatherInfo("Chelyabinsk");

                foreach (var weatherProperties in currentServiceWeatherProperties)
                {
                    if (!allWeatherProperties.ContainsKey(weatherProperties.Key))
                    {
                        allWeatherProperties[weatherProperties.Key] = weatherProperties.Value;
                    }
                }
            }
            
            var model = Mapper.Map<Dictionary<string, string>, WeatherInfoModel>(allWeatherProperties);

            Console.ReadKey();
        }
    }
}
