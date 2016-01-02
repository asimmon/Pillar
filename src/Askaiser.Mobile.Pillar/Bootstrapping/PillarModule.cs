using System;
using Askaiser.Mobile.Pillar.Factories;
using Askaiser.Mobile.Pillar.Interfaces;
using Askaiser.Mobile.Pillar.Services;
using Askaiser.Mobile.Pillar.Views;
using Autofac;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Bootstrapping
{
    /// <summary>
    /// Autofac Module that register all the dependencies needed in this library.
    /// </summary>
    public sealed class PillarModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // service registration
            builder.RegisterType<DialogService>()
                .As<IDialogProvider>()
                .SingleInstance();

            builder.RegisterType<ViewFactory>()
                .As<IViewFactory>()
                .SingleInstance();

            builder.RegisterType<Navigator>()
                .As<INavigator>()
                .SingleInstance();

            // default page resolver
            builder.RegisterInstance<Func<Page>>(() =>
            {
                // Check if we are using MasterDetailPage
                var masterDetailPage = Application.Current.MainPage as MasterDetailPage;

                var page = masterDetailPage != null
                    ? masterDetailPage.Detail
                    : Application.Current.MainPage;

                // Check if page is a NavigationPage
                var navigationPage = page as IPageContainer<Page>;

                return navigationPage != null
                    ? navigationPage.CurrentPage
                        : page;
            });

            // current PageProxy
            builder.RegisterType<PageProxy>()
                .As<IPage>()
                .SingleInstance();
        }
    }
}

