using Askaiser.Mobile.Pillar.Bootstrapping;
using Askaiser.Mobile.Pillar.Factories;
using Autofac;
using PillarDemo.ViewModels;
using PillarDemo.Views;
using Xamarin.Forms;

namespace PillarDemo
{
    public class DemoBootstrapper : PillarBootstrapper
    {
        private readonly Application _app;

        public DemoBootstrapper(Application app)
        {
            _app = app;
        }

        protected override void ConfigureContainer(ContainerBuilder builder)
        {
            base.ConfigureContainer(builder);

            builder.RegisterModule<DemoModule>();
        }

        protected override void RegisterViews(IViewFactory viewFactory)
        {
            viewFactory.Register<LoginViewModel, LoginView>();
            viewFactory.Register<HomeViewModel, HomeView>();
            viewFactory.Register<EventToCommandViewModel, EventToCommandView>();
            viewFactory.Register<TemplateSelectorViewModel, TemplateSelectorView>();
            viewFactory.Register<MessengerViewModel, MessengerView>();
            viewFactory.Register<DialogViewModel, DialogView>();
        }

        protected override void ConfigureApplication(IContainer container)
        {
            var viewFactory = container.Resolve<IViewFactory>();
            var page = viewFactory.Resolve<LoginViewModel>();

            _app.MainPage = new NavigationPage(page);
        }
    }
}
