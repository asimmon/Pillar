using System;
using Askaiser.Mobile.Pillar.Ioc;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Interfaces
{
    public interface IContainerAdapter
    {
        object Resolve(Type serviceType);

        T Resolve<T>()
            where T : class;

        void RegisterType<TInterface>()
            where TInterface : class;

        void RegisterType<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom;

        void RegisterType<TInterface>(Func<TInterface> implementationFactory)
            where TInterface : class;

        void RegisterType(Type serviceType, Func<object> implementationFactory);

        void RegisterType(Type serviceType, Type implementationType);

        void RegisterSingleton<TInterface>()
            where TInterface : class;

        void RegisterSingleton<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom;

        void RegisterSingleton<TInterface>(TInterface implementationInstance)
            where TInterface : class;

        void RegisterSingleton(Type serviceType, object implementationInstance);

        void RegisterSingleton<TInterface>(Func<TInterface> implementationFactory)
            where TInterface : class;

        void RegisterSingleton(Type serviceType, Func<object> implementationFactory);
    }

    public class AspNetDependencyInjectionAdapter : IContainerAdapter
    {
        private IServiceCollection _services;
        private IServiceProvider _provider;

        public AspNetDependencyInjectionAdapter()
        {
            _services = new ServiceCollection();
            _provider = null;
        }

        private void BuildProvider()
        {
            if (_provider == null)
            {
                _provider = _services.BuildServiceProvider();
            }
        }

        public void Clear()
        {
            _services.Clear();
        }

        public object Resolve(Type serviceType)
        {
            BuildProvider();

            return _provider.GetService(serviceType);
        }

        public T Resolve<T>()
            where T : class
        {
            BuildProvider();

            return _provider.GetService<T>();
        }

        public void RegisterType<TInterface>() where TInterface : class
        {
            RegisterType<TInterface, TInterface>();
        }

        public void RegisterType<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom
        {
            _services.AddTransient<TFrom, TTo>();
        }

        public void RegisterType<TInterface>(Func<TInterface> implementationFactory)
            where TInterface : class
        {
            _services.AddTransient(c => implementationFactory());
        }

        public void RegisterType(Type serviceType, Func<object> implementationFactory)
        {
            _services.AddTransient(serviceType, c => implementationFactory());
        }

        public void RegisterType(Type serviceType, Type implementationType)
        {
            _services.AddTransient(serviceType, implementationType);
        }

        public void RegisterSingleton<TInterface>() where TInterface : class
        {
            RegisterSingleton<TInterface, TInterface>();
        }

        public void RegisterSingleton<TFrom, TTo>()
            where TFrom : class
            where TTo : class, TFrom
        {
            _services.AddSingleton<TFrom, TTo>();
        }

        public void RegisterSingleton<TInterface>(TInterface implementationInstance)
            where TInterface : class
        {
            _services.AddSingleton(implementationInstance);
        }

        public void RegisterSingleton(Type serviceType, object implementationInstance)
        {
            _services.AddSingleton(serviceType, implementationInstance);
        }

        public void RegisterSingleton<TInterface>(Func<TInterface> implementationFactory)
            where TInterface : class
        {
            _services.AddSingleton(c => implementationFactory());
        }

        public void RegisterSingleton(Type serviceType, Func<object> implementationFactory)
        {
            _services.AddSingleton(serviceType, c => implementationFactory());
        }
    }
}
