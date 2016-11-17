using System;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Ioc
{
    public static class ServiceCollectionContainerBuilderExtensions
    {
        public static IServiceProvider BuildServiceProvider(this IServiceCollection services)
        {
            return new ServiceProvider(services);
        }
    }
}
