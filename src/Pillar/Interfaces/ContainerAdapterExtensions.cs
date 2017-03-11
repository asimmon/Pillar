using System;

namespace Pillar
{
    /// <summary>
    /// Add generic functionnalities to the IOC container adapter
    /// </summary>
    public static class ContainerAdapterExtensions
    {
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