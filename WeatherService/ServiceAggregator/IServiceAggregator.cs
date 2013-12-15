using DataAccess.Entities;
using WeatherService.ServiceParameters;

namespace WeatherService.ServiceAggregator
{
    public interface IServiceAggregator<out TModel, in TParameters> where TModel : Entity
                                                                    where TParameters : IServiceParameters
    {
        TModel AggregateServicesInfo(TParameters parameters);
    }
}
