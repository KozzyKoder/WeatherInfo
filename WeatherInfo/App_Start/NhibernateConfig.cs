using DataAccess;

namespace WeatherInfo.App_Start
{
    public class NhibernateConfig
    {
        public static void Setup(string databaseFullPath)
        {
            SqliteConfigurator.Configure(databaseFullPath);
        } 
    }
}