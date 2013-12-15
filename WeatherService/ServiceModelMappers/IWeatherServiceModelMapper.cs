using DataAccess.Entities;
using WeatherService.ServiceModels;

namespace WeatherService.ServiceModelMappers
{
    public interface IWeatherServiceModelMapper<in TModel> where TModel : IServiceModel, new()
    {
        WeatherInfo Map(TModel model);
    }
}