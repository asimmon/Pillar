using System;
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

        public async Task<IViewModel> PopAsync()
        {
            var previousViewModel = Navigation.GetCurrentViewModel();
            var nextViewModel = Navigation.GetPreviousViewModel();

            if (nextViewModel != null)
                nextViewModel.ViewEntering();

            if (previousViewModel != null)
                previousViewModel.ViewLeaving();

            await Navigation.PopAsync().ConfigureAwait(false);

            if (nextViewModel != null)
                nextViewModel.ViewEntered();

            if (previousViewModel != null)
                previousViewModel.ViewLeaved();

            return nextViewModel;
        }

        public async Task<IViewModel> PopModalAsync()
        {
            var nextView = await Navigation.PopModalAsync().ConfigureAwait(false);
            var nextViewModel = nextView.BindingContext as IViewModel;

            return nextViewModel;
        }

        public async Task PopToRootAsync()
        {
            var nextViewModel = Navigation.GetFirstViewModel();
            var previousViewModel = Navigation.GetCurrentViewModel();

            if (nextViewModel != null)
                nextViewModel.ViewEntering();

            if (previousViewModel != null)
                previousViewModel.ViewLeaving();

            await Navigation.PopToRootAsync().ConfigureAwait(false);

            if (nextViewModel != null)
                nextViewModel.ViewEntered();

            if (previousViewModel != null)
                previousViewModel.ViewLeaved();
        }

        public async Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            var previousiew = Navigation.GetCurrentView();
            var previousViewModel = Navigation.GetCurrentViewModel();

            TViewModel nextViewModel;
            var nextView = _viewFactory.Resolve(out nextViewModel, setStateAction);

            // About to entering a new view and leaving the current one
            nextViewModel.ViewEntering();

            if (previousViewModel != null)
                previousViewModel.ViewLeaving();

            if (previousViewModel != null && previousViewModel.NoHistory)
            {
                Navigation.InsertPageBefore(nextView, previousiew);
                await Navigation.PopAsync().ConfigureAwait(false);
            }
            else await Navigation.PushAsync(nextView).ConfigureAwait(false);

            // Entered the new view and leaved the previous one
            nextViewModel.ViewEntered();

            if (previousViewModel != null)
                previousViewModel.ViewLeaved();

            return nextViewModel;
        }

        public async Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var nextViewModel = viewModel;
            var nextView = _viewFactory.Resolve(nextViewModel);

            var previousView = Navigation.GetCurrentView();
            var previousViewModel = Navigation.GetCurrentViewModel();

            // About to entering a new view and leaving the current one
            nextViewModel.ViewEntering();

            if (previousViewModel != null)
                previousViewModel.ViewLeaving();

            if (previousViewModel != null && previousViewModel.NoHistory)
            {
                Navigation.InsertPageBefore(nextView, previousView);
                await Navigation.PopAsync().ConfigureAwait(false);
            }
            else await Navigation.PushAsync(nextView);

            // Entered the new view and leaved the previous one
            nextViewModel.ViewEntered();

            if (previousViewModel != null)
                previousViewModel.ViewLeaved();

            return nextViewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            var view = _viewFactory.Resolve(out viewModel, setStateAction);

            await Navigation.PushModalAsync(view).ConfigureAwait(false);

            return viewModel;
        }

        public async Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var nextView = _viewFactory.Resolve(viewModel);

            await Navigation.PushModalAsync(nextView).ConfigureAwait(false);

            return viewModel;
        }
    }
}