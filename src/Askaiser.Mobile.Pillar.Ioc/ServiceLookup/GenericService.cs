using System;
using System.Reflection;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Ioc.ServiceLookup
{
    internal class GenericService : IGenericService
    {
        private readonly ServiceDescriptor _descriptor;

        public GenericService(ServiceDescriptor descriptor)
        {
            _descriptor = descriptor;
        }

        public ServiceLifetime Lifetime
        {
            get { return _descriptor.Lifetime; }
        }

        public IService GetService(Type closedServiceType)
        {
            Type[] genericArguments = closedServiceType.GetTypeInfo().GenericTypeArguments;
            Type closedImplementationType =
                _descriptor.ImplementationType.MakeGenericType(genericArguments);

            var closedServiceDescriptor = new ServiceDescriptor(closedServiceType, closedImplementationType, Lifetime);
            return new Service(closedServiceDescriptor);
        }
    }
}
