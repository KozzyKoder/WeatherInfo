using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using WeatherService.Services;

namespace WeatherService
{
    public class WeatherServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Classes.FromThisAssembly()
                    .Where(Component.IsInSameNamespaceAs<IWeatherService>())
                    .WithService.DefaultInterfaces()
                    .LifestyleSingleton());
        }
    }
}