using Pillar;
using PillarDemo.ViewModels;
using PillarDemo.Views;
using Xamarin.Forms;

namespace PillarDemo
{
    public class DemoBootstrapper : PillarBootstrapper
    {
        public DemoBootstrapper(Application app)
            : base(app)
        { }

        protected override void RegisterDependencies(IContainerAdapter container)
        {
            container.RegisterSingleton<LoginViewModel>();
            container.RegisterSingleton<LoginView>();

            container.RegisterSingleton<HomeViewModel>();
            container.RegisterSingleton<HomeView>();

            container.RegisterType<EventToCommandViewModel>();
            container.RegisterType<EventToCommandView>();

            container.RegisterType<DialogViewModel>();
            container.RegisterType<DialogView>();

            container.RegisterType<TemplateSelectorViewModel>();
            container.RegisterType<TemplateSelectorView>();

            container.RegisterType<MessengerViewModel>();
            container.RegisterType<MessengerView>();
        }

        protected override void BindViewModelsToViews(IViewFactory viewFactory)
        {
            viewFactory.Register<LoginViewModel, LoginView>();
            viewFactory.Register<HomeViewModel, HomeView>();
            viewFactory.Register<EventToCommandViewModel, EventToCommandView>();
            viewFactory.Register<TemplateSelectorViewModel, TemplateSelectorView>();
            viewFactory.Register<MessengerViewModel, MessengerView>();
            viewFactory.Register<DialogViewModel, DialogView>();
        }

        protected override Page GetFirstPage(IViewFactory viewFactory)
        {
            return viewFactory.Resolve<LoginViewModel>();
        }
    }
}
