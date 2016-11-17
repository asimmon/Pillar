using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Ioc.ServiceLookup
{
    internal class FactoryService : IService, IServiceCallSite
    {
        private readonly ServiceDescriptor _descriptor;

        public FactoryService(ServiceDescriptor descriptor)
        {
            _descriptor = descriptor;
        }

        public IService Next { get; set; }

        public ServiceLifetime Lifetime
        {
            get { return _descriptor.Lifetime; }
        }

        public IServiceCallSite CreateCallSite(ServiceProvider provider, ISet<Type> callSiteChain)
        {
            return this;
        }

        public object Invoke(ServiceProvider provider)
        {
            return _descriptor.ImplementationFactory(provider);
        }

        public Expression Build(Expression provider)
        {
            Expression<Func<IServiceProvider, object>> factory =
                serviceProvider => _descriptor.ImplementationFactory(serviceProvider);

            return Expression.Invoke(factory, provider);
        }
    }
}
