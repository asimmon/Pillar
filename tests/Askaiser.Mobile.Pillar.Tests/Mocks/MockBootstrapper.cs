using Askaiser.Mobile.Pillar.Bootstrapping;
using Askaiser.Mobile.Pillar.Factories;
using Askaiser.Mobile.Pillar.Interfaces;

namespace Askaiser.Mobile.Pillar.Tests.Mocks
{
    public class MockBootstrapper : PillarBootstrapper
    {
        public IViewFactory ViewFactory { get; set; }

        protected override void ConfigureContainer(IContainerAdapter container)
        {
            container.RegisterType<MockViewModel>();
            container.RegisterType<MockView>();
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            ViewFactory = viewFactory;
            ViewFactory.Register<MockViewModel, MockView>();
        }

        protected override void ConfigureApplication(IContainerAdapter container)
        {
            Container = container;
        }
    }
}
