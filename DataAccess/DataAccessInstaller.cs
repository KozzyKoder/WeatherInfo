using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DataAccess.Repository;

namespace DataAccess
{
    public class DataAccessInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(IRepository<>))
                                        .ImplementedBy(typeof(Repository<>))
                                        .LifestyleSingleton());
        }
    }
}