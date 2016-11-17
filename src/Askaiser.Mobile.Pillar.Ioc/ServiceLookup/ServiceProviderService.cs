using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Ioc.ServiceLookup
{
    internal class ServiceProviderService : IService, IServiceCallSite
    {
        public IService Next { get; set; }

        public ServiceLifetime Lifetime
        {
            get { return ServiceLifetime.Transient; }
        }

        public IServiceCallSite CreateCallSite(ServiceProvider provider, ISet<Type> callSiteChain)
        {
            return this;
        }

        public object Invoke(ServiceProvider provider)
        {
            return provider;
        }

        public Expression Build(Expression provider)
        {
            return provider;
        }
    }
}
