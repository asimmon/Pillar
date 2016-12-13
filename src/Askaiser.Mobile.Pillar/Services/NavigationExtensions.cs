using System.Linq;
using Askaiser.Mobile.Pillar.ViewModels;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Services
{
    /// <summary>
    /// Extensions methods to access current or previous view in the navigation stack
    /// </summary>
    public static class NavigationExtensions
    {
        /// <summary>
        /// Get the currently displayed view (page)
        /// </summary>
        public static Page GetCurrentView(this INavigation navigation)
        {
            return navigation?.NavigationStack?.LastOrDefault();
        }

        /// <summary>
        /// Get the currently displayed view model, if it is an IViewModel
        /// </summary>
        public static IViewModel GetCurrentViewModel(this INavigation navigation)
        {
            var currentView = GetCurrentView(navigation);
            return currentView?.BindingContext as IViewModel;
        }

        /// <summary>
        /// Get the view that was displayed before the current one, if it exists
        /// </summary>
        public static Page GetPreviousView(this INavigation navigation)
        {
            if (navigation != null && navigation.NavigationStack != null && navigation.NavigationStack.Count > 1)
            {
                return navigation.NavigationStack[navigation.NavigationStack.Count - 2];
            }

            return null;
        }

        /// <summary>
        /// Get the view model that was displayed before the current one, if it exists and is an IViewModel
        /// </summary>
        public static IViewModel GetPreviousViewModel(this INavigation navigation)
        {
            var previousView = GetPreviousView(navigation);
            return previousView?.BindingContext as IViewModel;
        }

        public static Page GetFirstView(this INavigation navigation)
        {
            return navigation?.NavigationStack?.FirstOrDefault();
        }

        public static IViewModel GetFirstViewModel(this INavigation navigation)
        {
            var firstView = GetFirstView(navigation);
            return firstView?.BindingContext as IViewModel;
        }
    }
}