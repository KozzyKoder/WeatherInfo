using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Common;
using DataAccess.Entities;
using log4net;
using WeatherService.ServiceParameters;
using WeatherService.Services;

namespace WeatherService.ServiceAggregator
{
    public class WeatherServiceAggregator : IServiceAggregator<WeatherInfo, WeatherServiceParameters>
    {
        protected static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public WeatherInfo AggregateServicesInfo(WeatherServiceParameters parameters)
        {
            var weatherInfo = new WeatherInfo();
            var services = Ioc.Container.ResolveAll(typeof(IService<WeatherInfo, WeatherServiceParameters>));
            foreach (IService<WeatherInfo, WeatherServiceParameters> service in services)
            {
                try
                {
                    service.MakeRequest(parameters, weatherInfo);
                }
                catch (IOException e)
                {
                    Logger.Error("Data format not fit or network error", e);
                }
                catch (Exception e)
                {
                    Logger.Error("Not Expected type of error", e);
                }
            }

            weatherInfo.CityName = parameters.CityName;
            weatherInfo.LastUpdated = DateTime.UtcNow;

            return weatherInfo;
        }
    }
}
