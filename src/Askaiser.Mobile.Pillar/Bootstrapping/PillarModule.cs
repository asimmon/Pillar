﻿using System;
using Askaiser.Mobile.Pillar.Factories;
using Askaiser.Mobile.Pillar.Interfaces;
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
        public void Load(IContainerAdapter container)
        {
            // service registration
            container.RegisterSingleton(container); // the container itself can be injected
            container.RegisterSingleton<IDialogProvider, DialogService>();
            container.RegisterSingleton<IViewFactory, ViewFactory>();
            container.RegisterSingleton<INavigator, Navigator>();

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
            bool hasMore;

            do
            {
                hasMore = true;

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
                    hasMore = false;
                }
            } while (hasMore);

            return page;
        }
    }
}

