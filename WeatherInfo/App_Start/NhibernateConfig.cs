using System.Configuration;
using DataAccess;

namespace WeatherInfo
{
    public class NhibernateConfig
    {
        public static void Setup(string databaseFullPath)
        {
            SqliteConfigurator.Configure(databaseFullPath);
        } 
    }
}