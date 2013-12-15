using System.Collections.Generic;
using DataAccess.Entities;

namespace BusinessLayer.BusinessServices
{
    public interface IWeatherGrabberBusinessService
    {
        IEnumerable<WeatherInfo> GrabWeatherInfos(List<string> cityNames);
    }
}
