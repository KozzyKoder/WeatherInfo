using BusinessLayer.BusinessServices;
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
        }
    }
}
