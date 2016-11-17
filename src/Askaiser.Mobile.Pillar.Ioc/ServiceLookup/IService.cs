using System;
using System.Collections.Generic;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Ioc.ServiceLookup
{
    internal interface IService
    {
        IService Next { get; set; }

        ServiceLifetime Lifetime { get; }

        IServiceCallSite CreateCallSite(ServiceProvider provider, ISet<Type> callSiteChain);
    }
}
