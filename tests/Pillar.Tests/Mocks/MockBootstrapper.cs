using Pillar.Bootstrapping;
using Pillar.Factories;
using Pillar.Interfaces;
using Xamarin.Forms;

namespace Pillar.Tests.Mocks
{
    public class MockBootstrapper : PillarBootstrapper
    {
        public MockBootstrapper(Application app)
            : base(app)
        { }

        public MockBootstrapper()
            : base(null)
        { }

        public IViewFactory ViewFactory { get; set; }

        protected override void RegisterDependencies(IContainerAdapter container)
        {
            container.RegisterType<MockViewModel>();
            container.RegisterType<MockView>();
        }

        protected override void BindViewModelsToViews(IViewFactory viewFactory)
        {
            ViewFactory = viewFactory;
            ViewFactory.Register<MockViewModel, MockView>();
        }

        protected override Page GetFirstPage(IViewFactory viewFactory)
        {
            return viewFactory.Resolve<MockViewModel>();
        }
    }
}
