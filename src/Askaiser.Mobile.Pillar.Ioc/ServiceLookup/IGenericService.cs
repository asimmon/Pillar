using System;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Ioc.ServiceLookup
{
    internal interface IGenericService
    {
        ServiceLifetime Lifetime { get; }

        IService GetService(Type closedServiceType);
    }
}
