using System;
using Askaiser.Mobile.Pillar.Factories;
using Askaiser.Mobile.Pillar.Interfaces;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;
using Askaiser.Mobile.Pillar.Services;
using Askaiser.Mobile.Pillar.Views;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Bootstrapping
{
    /// <summary>
    /// Autofac Module that register all the dependencies needed in this library.
    /// </summary>
    public sealed class PillarModule
    {
        public void Load(IServiceCollection builder)
        {
            // service registration
            builder.AddSingleton<IDialogProvider, DialogService>();
            builder.AddSingleton<IViewFactory, ViewFactory>();
            builder.AddSingleton<INavigator, Navigator>();

            // current page resolver
            builder.AddTransient<Func<Page>>(provider => GetCurrentPage);

            // current PageProxy
            builder.AddSingleton<IPage, PageProxy>();
        }

        public Page GetCurrentPage()
        {
            return GetCurrentPage(Application.Current.MainPage);
        }

        public Page GetCurrentPage(Page rootPage)
        {
            var page = rootPage;
            bool hasMore;

            do
            {
                hasMore = true;

                if (page is MasterDetailPage)
                {
                    page = ((MasterDetailPage) page).Detail;
                }
                else if (page is IPageContainer<Page>)
                {
                    page = ((IPageContainer<Page>) page).CurrentPage;
                }
                else
                {
                    hasMore = false;
                }
            } while (hasMore);

            return page;
        }
    }
}

