using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using AutoMapper;
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
            var service = new OpenWeatherService();

            var info = service.GetWeatherInfo("Chelyabinsk");

            Mapper
            .CreateMap<Dictionary<string, string>, WeatherInfoModel>()
            .ConvertUsing(x =>
                {
                    var serializer = new JavaScriptSerializer();
                    return serializer.Deserialize<WeatherInfoModel>(serializer.Serialize(x));
                });

            var model = Mapper.Map<Dictionary<string, string>, WeatherInfoModel>(info);

            Console.ReadKey();
        }
    }
}
