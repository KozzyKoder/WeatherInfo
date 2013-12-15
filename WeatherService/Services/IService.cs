using DataAccess.Entities;
using WeatherService.ServiceParameters;

namespace WeatherService.Services
{
    public interface IService<TEntity, in TParameters> where TEntity : Entity
                                                    where TParameters : IServiceParameters
    {
        TEntity MakeRequest(TParameters parameters, TEntity entity);
    }
}