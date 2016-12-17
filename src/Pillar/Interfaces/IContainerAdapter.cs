using System;

namespace Pillar.Interfaces
{
    /// <summary>
    /// Depedency injection registration and resolving abstraction
    /// </summary>
    public interface IContainerAdapter
    {
        object Resolve(Type serviceType);

        void RegisterType(Type serviceType, Type implementationType);

        void RegisterType(Type serviceType, Func<object> implementationFactory);

        void RegisterSingleton(Type serviceType, Type implementationType);

        void RegisterSingleton(Type serviceType, object implementationInstance);

        void RegisterSingleton(Type serviceType, Func<object> implementationFactory);
    }
}
