using BusinessLayer.BusinessServices;
using BusinessLayer.ServiceAggregator;
using BusinessLayer.Services;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace BusinessLayer
{
    public class BusinessLayerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IWeatherGrabberBusinessService>()
                .ImplementedBy<WeatherGrabberBusinessService>()
                .LifestyleSingleton());

            container.Register(Component.For(typeof(IWeatherServiceAggregator))
                            .ImplementedBy<WeatherServiceAggregator>()
                            .LifestyleSingleton());


            container.Register(
               Classes.FromThisAssembly()
                  .BasedOn(typeof(IWeatherServiceModelMapper<>))
                  .WithService.AllInterfaces()
                  .LifestyleSingleton()
            );

            container.Register(Classes.FromThisAssembly()
                                    .BasedOn(typeof(IWeatherService))
                                    .WithService.AllInterfaces()
                                    .LifestyleSingleton());
        }
    }
}
