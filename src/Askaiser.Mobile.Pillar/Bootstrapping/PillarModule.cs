using System;
using Askaiser.Mobile.Pillar.Factories;
using Askaiser.Mobile.Pillar.Interfaces;
using Askaiser.Mobile.Pillar.Services;
using Askaiser.Mobile.Pillar.Views;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Bootstrapping
{
    /// <summary>
    /// Class that help register all the dependencies needed in this library.
    /// </summary>
    public static class PillarModule
    {
        internal static void RegisterPillarDependencies(this IContainerAdapter container)
        {
            // service registration
            container.RegisterSingleton(container); // the container itself can be injected

            container.RegisterSingleton<IDialogProvider, DialogService>();
            container.RegisterSingleton<IViewFactory, ViewFactory>();
            container.RegisterSingleton<INavigator, Navigator>();
            container.RegisterSingleton<IMessenger, Messenger>();

            // current page resolver
            container.RegisterType<Func<Page>>(() => GetCurrentPage);

            // current PageProxy
            container.RegisterSingleton<IPage, PageProxy>();
        }

        private static Page GetCurrentPage()
        {
            return GetCurrentPage(Application.Current.MainPage);
        }

        public static Page GetCurrentPage(Page rootPage)
        {
            var page = rootPage;
            bool containsAnotherPage;

            do
            {
                containsAnotherPage = true;

                if (page is MasterDetailPage)
                {
                    page = ((MasterDetailPage)page).Detail;
                }
                else if (page is IPageContainer<Page>)
                {
                    page = ((IPageContainer<Page>)page).CurrentPage;
                }
                else
                {
                    containsAnotherPage = false;
                }
            } while (containsAnotherPage);

            return page;
        }
    }
}

