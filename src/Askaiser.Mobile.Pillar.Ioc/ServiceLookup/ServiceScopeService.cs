using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Ioc.ServiceLookup
{
    internal class ServiceScopeService : IService, IServiceCallSite
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
            return new ServiceScopeFactory(provider);
        }

        public Expression Build(Expression provider)
        {
            return Expression.New(
                typeof(ServiceScopeFactory).GetTypeInfo()
                    .DeclaredConstructors
                    .Single(),
                provider);
        }
    }
}
