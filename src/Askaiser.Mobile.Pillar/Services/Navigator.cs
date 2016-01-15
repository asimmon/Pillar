using System;
using System.Linq;
using System.Threading.Tasks;
using Askaiser.Mobile.Pillar.Factories;
using Askaiser.Mobile.Pillar.Interfaces;
using Askaiser.Mobile.Pillar.ViewModels;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Services
{
    /// <summary>
    /// The implementation of the ViewModel-based navigation.
    /// Uses the Xamarin Forms navigation system <see cref="INavigation"/>.
    /// Please inject the interface inside your ViewModel to navigate between pages.
    /// </summary>
    /// <seealso cref="INavigator" />
    public class Navigator : INavigator
    {
        /// <summary>
        /// We use a page abstraction to get the underlying navigation system
        /// </summary>
        private readonly IPage _page;

        /// <summary>
        /// Factory used to get any Page associated to a ViewModel
        /// </summary>
        private readonly IViewFactory _viewFactory;

        public Navigator(IPage page, IViewFactory viewFactory)
        {
            _page = page;
            _viewFactory = viewFactory;
        }

        private INavigation Navigation
        {
            get { return _page.Navigation; }
        }

        private IViewModel CurrentViewModel
        {
            get
            {
                if (Navigation == null || Navigation.NavigationStack == null)
                    return null;

                var firstPage = Navigation.NavigationStack.FirstOrDefault();
                if (firstPage == null)
                    return null;

                return firstPage.BindingContext as IViewModel;
            }
        }

        public async Task<IViewModel> PopAsync()
        {
            var view = await Navigation.PopAsync();
            var viewModel = view.BindingContext as IViewModel;

            if (viewModel != null)
                viewModel.NavigatedFrom();

            return viewModel;
        }

        public async Task<IViewModel> PopModalAsync()
        {
            var view = await Navigation.PopModalAsync();
            var viewModel = view.BindingContext as IViewModel;

            if (viewModel != null)
                viewModel.NavigatedFrom();

            return viewModel;
        }

        public async Task PopToRootAsync()
        {
            await Navigation.PopToRootAsync();
        }

        public async Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = _viewFactory.Resolve(out viewModel, setStateAction);

            var currentViewModel = CurrentViewModel;
            if (currentViewModel != null && currentViewModel.NoHistory)
            {
                Navigation.InsertPageBefore(view, Navigation.NavigationStack.FirstOrDefault());
                await Navigation.PopAsync().ConfigureAwait(false);
            }
            else await Navigation.PushAsync(view);

            viewModel.NavigatedTo();
            return viewModel;
        }

        public async Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);

            var currentViewModel = CurrentViewModel;
            if (currentViewModel != null && currentViewModel.NoHistory)
            {
                Navigation.InsertPageBefore(view, Navigation.NavigationStack.FirstOrDefault());
                await Navigation.PopAsync().ConfigureAwait(false);
            }
            else await Navigation.PushAsync(view);

            viewModel.NavigatedTo();
            return viewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = _viewFactory.Resolve(out viewModel, setStateAction);
            await Navigation.PushModalAsync(view);
            return viewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var view = _viewFactory.Resolve(viewModel);
            await Navigation.PushModalAsync(view);
            return viewModel;
        }

        public void ClearHistory()
        {
            var existingPages = Navigation.NavigationStack.ToList();
            foreach (var page in existingPages)
            {
                Navigation.RemovePage(page);
            }
        }
    }
}

