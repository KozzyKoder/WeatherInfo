using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace WeatherInfo.Controllers
{
    public class ControllerBase : Controller
    {
        public List<string> GetCitiesList()
        {
            var cities = ConfigurationManager.AppSettings["Cities"];
            var citiesList = cities.Split(new[] {','}).ToList();
            return citiesList;
        }
    }
}