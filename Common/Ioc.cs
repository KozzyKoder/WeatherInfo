using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;

namespace Common
{
    public static class Ioc
    {
        static Ioc()
        {
            Container = new WindsorContainer();
            Container.Register(Component.For<IWindsorContainer>().Instance(Container));
            Container.Kernel.Resolver.AddSubResolver(new CollectionResolver(Container.Kernel));
        }

        public static IWindsorContainer Container { get; set; }

        public static T Resolve<T>()
        {
            return Container.Resolve<T>();
        }
    }
}
