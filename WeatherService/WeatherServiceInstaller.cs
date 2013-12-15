﻿using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DataAccess.Entities;
using WeatherService.ServiceAggregator;
using WeatherService.ServiceModelMappers;
using WeatherService.Services;

namespace WeatherService
{
    public class WeatherServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For(typeof(IWeatherServiceAggregator))
                                        .ImplementedBy<WeatherServiceAggregator>()
                                        .LifestyleSingleton());

             
            container.Register(
               Classes.FromThisAssembly()
                  .Where(Component.IsInSameNamespaceAs(typeof(IWeatherServiceModelMapper<>)))
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
