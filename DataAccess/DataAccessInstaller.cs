using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DataAccess.Entities;
using DataAccess.Repository;

namespace DataAccess
{
    public class DataAccessInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(IRepository<WeatherInfo>))
                                        .ImplementedBy<WeatherRepository>()
                                        .LifestyleSingleton());
        }
    }
}