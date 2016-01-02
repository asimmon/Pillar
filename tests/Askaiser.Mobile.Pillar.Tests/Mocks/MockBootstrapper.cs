using Askaiser.Mobile.Pillar.Bootstrapping;
using Askaiser.Mobile.Pillar.Factories;
using Autofac;

namespace Askaiser.Mobile.Pillar.Tests.Mocks
{
    public class MockBootstrapper : PillarBootstrapper
    {
        public IViewFactory ViewFactory { get; set; }

        public IContainer Container { get; set; }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);
            builder.RegisterType<MockViewModel>();
            builder.RegisterType<MockView>();
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            ViewFactory = viewFactory;
            ViewFactory.Register<MockViewModel, MockView>();
        }

        protected override void ConfigureApplication(IContainer container)
        {
            Container = container;
        }
    }
}
