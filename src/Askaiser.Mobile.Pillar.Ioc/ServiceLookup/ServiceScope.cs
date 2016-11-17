using System;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Ioc.ServiceLookup
{
    internal class ServiceScope : IServiceScope
    {
        private readonly ServiceProvider _scopedProvider;

        public ServiceScope(ServiceProvider scopedProvider)
        {
            _scopedProvider = scopedProvider;
        }

        public IServiceProvider ServiceProvider
        {
            get { return _scopedProvider; }
        }

        public void Dispose()
        {
            _scopedProvider.Dispose();
        }
    }
}
