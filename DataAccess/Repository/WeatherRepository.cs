using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Entities;
using NHibernate.Linq;

namespace DataAccess.Repository
{
    public class WeatherRepository : Repository<WeatherInfo>
    {
        public WeatherInfo GetByCityName(string cityName)
        {
            var session = Factory.OpenSession();
            
            var result = session.Query<WeatherInfo>().FirstOrDefault(p => p.CityName == cityName);

            session.Close();

            return result;
        }
    }
}
