using System;
using Askaiser.Mobile.Pillar.Bootstrapping;
using Askaiser.Mobile.Pillar.Factories;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Tests.Mocks
{
    public class MockBootstrapper : PillarBootstrapper
    {
        public IViewFactory ViewFactory { get; set; }

        public IServiceProvider Container { get; set; }

        protected override void ConfigureContainer(IServiceCollection builder)
        {
            base.ConfigureContainer(builder);
            builder.AddTransient<MockViewModel>();
            builder.AddTransient<MockView>();
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            ViewFactory = viewFactory;
            ViewFactory.Register<MockViewModel, MockView>();
        }

        protected override void ConfigureApplication(IServiceProvider container)
        {
            Container = container;
        }
    }
}
