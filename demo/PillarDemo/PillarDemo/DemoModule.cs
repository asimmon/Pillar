using Pillar.ViewModels;
using Autofac;
using Autofac.Core;
using GalaSoft.MvvmLight.Messaging;
using PillarDemo.ViewModels;
using PillarDemo.Views;

namespace PillarDemo
{
    public class DemoModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ViewModelLocator>().SingleInstance();

            builder.RegisterType<HelpViewModel>().SingleInstance();

            builder.RegisterType<LoginViewModel>().SingleInstance();
            builder.RegisterType<LoginView>().SingleInstance();

            builder.RegisterType<HomeViewModel>().SingleInstance();
            builder.RegisterType<HomeView>().SingleInstance();

            builder.RegisterType<EventToCommandViewModel>();
            builder.RegisterType<EventToCommandView>();

            builder.RegisterType<DialogViewModel>();
            builder.RegisterType<DialogView>();

            builder.RegisterType<TemplateSelectorViewModel>();
            builder.RegisterType<TemplateSelectorView>();

            builder.RegisterType<MessengerViewModel>();
            builder.RegisterType<MessengerView>();

            builder.Register(ctx => Messenger.Default)
                .As<IMessenger>()
                .SingleInstance();
        }

        protected override void AttachToComponentRegistration(IComponentRegistry componentRegistry, IComponentRegistration registration)
        {
            registration.Activated += RegistrationActivated;
        }

        private static void RegistrationActivated(object sender, ActivatedEventArgs<object> e)
        {
            var vm = e.Instance as IViewModel;
            if (vm != null)
            {
                vm.Locator = e.Context.Resolve<ViewModelLocator>();
            }
        }
    }
}
