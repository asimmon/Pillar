using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Ioc.ServiceLookup
{
    /// <summary>
    /// Summary description for InstanceService
    /// </summary>
    internal class InstanceService : IService, IServiceCallSite
    {
        private readonly ServiceDescriptor _descriptor;

        public InstanceService(ServiceDescriptor descriptor)
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
            return _descriptor.ImplementationInstance;
        }

        public Expression Build(Expression provider)
        {
            return Expression.Constant(_descriptor.ImplementationInstance, _descriptor.ServiceType);
        }
    }
}
