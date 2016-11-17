using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Ioc.ServiceLookup
{
    internal class ServiceScopeFactory : IServiceScopeFactory
    {
        private readonly ServiceProvider _provider;

        public ServiceScopeFactory(ServiceProvider provider)
        {
            _provider = provider;
        }

        public IServiceScope CreateScope()
        {
            return new ServiceScope(new ServiceProvider(_provider));
        }
    }
}
