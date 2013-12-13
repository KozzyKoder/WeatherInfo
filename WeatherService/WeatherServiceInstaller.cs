using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using WeatherService.ServiceAggregator;
using WeatherService.ServiceModelMappers;
using WeatherService.ServiceModels;
using WeatherService.Services;

namespace WeatherService
{
    public class WeatherServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IServiceAggregator>()
                                        .ImplementedBy<ServiceAggregator.ServiceAggregator>()
                                        .LifestyleSingleton());

             
            container.Register(
               Classes.FromThisAssembly()
                  .Where(Component.IsInSameNamespaceAs(typeof(IServiceModelMapper<,>)))
                  .WithServiceDefaultInterfaces()
                  .LifestyleSingleton()
            );

            container.Register(Classes.FromThisAssembly()
                                    .BasedOn(typeof(IService<>))
                                    .WithService.AllInterfaces()
                                    .LifestyleSingleton());
        }
    }
}
