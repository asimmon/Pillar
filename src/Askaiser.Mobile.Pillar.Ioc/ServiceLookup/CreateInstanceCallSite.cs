using System;
using System.Linq.Expressions;
using System.Runtime.ExceptionServices;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Ioc.ServiceLookup
{
    internal class CreateInstanceCallSite : IServiceCallSite
    {
        private readonly ServiceDescriptor _descriptor;

        public CreateInstanceCallSite(ServiceDescriptor descriptor)
        {
            _descriptor = descriptor;
        }

        public object Invoke(ServiceProvider provider)
        {
            try
            {
                return Activator.CreateInstance(_descriptor.ImplementationType);
            }
            catch (Exception ex) when (ex.InnerException != null)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException).Throw();
                // The above line will always throw, but the compiler requires we throw explicitly.
                throw;
            }
        }

        public Expression Build(Expression provider)
        {
            return Expression.New(_descriptor.ImplementationType);
        }
    }
}
