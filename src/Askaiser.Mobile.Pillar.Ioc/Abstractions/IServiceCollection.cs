using System.Collections.Generic;

namespace Askaiser.Mobile.Pillar.Ioc.Abstractions
{
    /// <summary>
    /// Specifies the contract for a collection of service descriptors.
    /// </summary>
    public interface IServiceCollection : IList<ServiceDescriptor>
    { }
}
