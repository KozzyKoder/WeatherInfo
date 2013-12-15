namespace WeatherService.ServiceParameters
{
    public class WeatherServiceParameters : IServiceParameters
    {
        public string CityName { get; set; }

        public WeatherServiceParameters(string cityName)
        {
            CityName = cityName;
        }
    }
}
