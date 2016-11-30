using System;
using Askaiser.Mobile.Pillar.Ioc;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Interfaces
{
    public interface IContainerAdapter
    {
        object Resolve(Type serviceType);

        void RegisterType(Type serviceType, Type implementationType);

        void RegisterType(Type serviceType, Func<object> implementationFactory);

        void RegisterSingleton(Type serviceType, Type implementationType);

        void RegisterSingleton(Type serviceType, object implementationInstance);

        void RegisterSingleton(Type serviceType, Func<object> implementationFactory);
    }

    public class AspNetDependencyInjectionAdapter : IContainerAdapter
    {
        private readonly IServiceCollection _services;
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
            _services.AddTransient(serviceType, c => implementationFactory());
        }

        public void RegisterType(Type serviceType, Type implementationType)
        {
            _services.AddTransient(serviceType, implementationType);
        }

        public void RegisterSingleton(Type serviceType, Type implementationType)
        {
            _services.AddSingleton(serviceType, implementationType);
        }

        public void RegisterSingleton(Type serviceType, object implementationInstance)
        {
            _services.AddSingleton(serviceType, implementationInstance);
        }

        public void RegisterSingleton(Type serviceType, Func<object> implementationFactory)
        {
            _services.AddSingleton(serviceType, c => implementationFactory());
        }
    }

    /// <summary>
    /// Generic extension methods
    /// </summary>
    public static class ContainerAdapterExtensions
    {
        // resolve

        public static T Resolve<T>(this IContainerAdapter adapter)
            where T : class
        {
            return (T)adapter.Resolve(typeof(T));
        }

        // transient generics

        public static void RegisterType<TFrom, TTo>(this IContainerAdapter adapter)
            where TFrom : class
            where TTo : class, TFrom
        {
            adapter.RegisterType(typeof(TFrom), typeof(TTo));
        }

        public static void RegisterType<TInterface>(this IContainerAdapter adapter)
            where TInterface : class
        {
            adapter.RegisterType(typeof(TInterface), typeof(TInterface));
        }

        public static void RegisterType<TInterface>(this IContainerAdapter adapter, Func<TInterface> implementationFactory)
            where TInterface : class
        {
            adapter.RegisterType(typeof(TInterface), implementationFactory);
        }

        // singletons generics

        public static void RegisterSingleton<TInterface>(this IContainerAdapter adapter)
            where TInterface : class
        {
            adapter.RegisterSingleton<TInterface, TInterface>();
        }

        public static void RegisterSingleton<TFrom, TTo>(this IContainerAdapter adapter)
            where TFrom : class
            where TTo : class, TFrom
        {
            adapter.RegisterSingleton(typeof(TFrom), typeof(TTo));
        }

        public static void RegisterSingleton<TInterface>(this IContainerAdapter adapter, TInterface implementationInstance)
            where TInterface : class
        {
            adapter.RegisterSingleton(typeof(TInterface), implementationInstance);
        }

        public static void RegisterSingleton<TInterface>(this IContainerAdapter adapter, Func<TInterface> implementationFactory)
            where TInterface : class
        {
            adapter.RegisterSingleton(typeof(TInterface), implementationFactory);
        }
    }
}
