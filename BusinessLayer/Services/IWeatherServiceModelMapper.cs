using DataAccess.Entities;

namespace BusinessLayer.Services
{
    public interface IWeatherServiceModelMapper<in TModel> where TModel : IServiceModel, new()
    {
        WeatherInfo Map(TModel model);
    }
}