using System;
using Askaiser.Mobile.Pillar.Ioc;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Interfaces
{
    public class PillarDefaultIoc : IContainerAdapter
    {
        private readonly IServiceCollection _services;
        private IServiceProvider _provider;
        private bool _isBuilt;

        public PillarDefaultIoc()
        {
            _services = new ServiceCollection();
            _provider = null;
            _isBuilt = false;
        }

        private void BuildProvider()
        {
            if (!_isBuilt)
            {
                _provider = _services.BuildServiceProvider();
                _isBuilt = true;
            }
        }

        private void EnsureThatProviderIsNotBuildYet()
        {
            if (_isBuilt)
            {
                throw new InvalidOperationException("Could not register any new dependency, the container has already been built.");
            }
        }

        public object Resolve(Type serviceType)
        {
            if (serviceType == null)
            {
                throw new ArgumentNullException(nameof(serviceType));
            }

            BuildProvider();

            var service = _provider.GetService(serviceType);
            if (service == null)
            {
                throw new InvalidOperationException($"No service for type '{serviceType}' has been registered.");
            }

            return service;
        }

        public void RegisterType(Type serviceType, Func<object> implementationFactory)
        {
            EnsureThatProviderIsNotBuildYet();
            _services.AddTransient(serviceType, c => implementationFactory());
        }

        public void RegisterType(Type serviceType, Type implementationType)
        {
            EnsureThatProviderIsNotBuildYet();
            _services.AddTransient(serviceType, implementationType);
        }

        public void RegisterSingleton(Type serviceType, Type implementationType)
        {
            EnsureThatProviderIsNotBuildYet();
            _services.AddSingleton(serviceType, implementationType);
        }

        public void RegisterSingleton(Type serviceType, object implementationInstance)
        {
            EnsureThatProviderIsNotBuildYet();
            _services.AddSingleton(serviceType, implementationInstance);
        }

        public void RegisterSingleton(Type serviceType, Func<object> implementationFactory)
        {
            EnsureThatProviderIsNotBuildYet();
            _services.AddSingleton(serviceType, c => implementationFactory());
        }
    }
}