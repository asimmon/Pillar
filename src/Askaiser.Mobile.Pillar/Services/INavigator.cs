using System;
using System.Threading.Tasks;
using Askaiser.Mobile.Pillar.ViewModels;

namespace Askaiser.Mobile.Pillar.Services
{
    /// <summary>
    /// Provides a ViewModel-based navigation. You need to bind each
    /// ViewModel type to a View typein a AutofacBootstrapper based class.
    /// </summary>
    public interface INavigator
    {
        /// <summary>
        /// Asynchronously go back to the previous Page.
        /// </summary>
        /// <returns>The ViewModel bound to the previous Page</returns>
        Task<IViewModel> PopAsync();

        /// <summary>
        /// Asynchronously dismiss the current modal page.
        /// </summary>
        /// <returns>The ViewModel bound to the Page where the modal were push</returns>
        Task<IViewModel> PopModalAsync();

        /// <summary>
        /// Asynchronously go back to the first Page opened.
        /// </summary>
        /// <returns>The ViewModel associated to the first Page</returns>
        Task PopToRootAsync();

        /// <summary>
        /// Asynchronously go to the Page associated to a ViewModel type.
        /// The instance of the TViewModel type will be resolved with Autofac.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the ViewModel</typeparam>
        /// <param name="setStateAction">A callback to apply to the ViewModel. Can be used to pass data.</param>
        /// <returns>The instance of the resolved ViewModel</returns>
        Task<TViewModel> PushAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        /// <summary>
        /// Asynchronously go to a Page associated to a ViewModel type.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the ViewModel</typeparam>
        /// <param name="viewModel">The ViewModel instance</param>
        /// <returns>The same ViewModel instance</returns>
        Task<TViewModel> PushAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;

        /// <summary>
        /// Asynchronously open a modal Page associated to a ViewModel type.
        /// The instance of the TViewModel type will be resolved with Autofac.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the ViewModel</typeparam>
        /// <param name="setStateAction">A callback to apply to the ViewModel. Can be used to pass data.</param>
        /// <returns>The instance of the resolved ViewModel</returns>
        Task<TViewModel> PushModalAsync<TViewModel>(Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel;

        /// <summary>
        /// Asynchronously open a modal Page associated to a ViewModel type.
        /// </summary>
        /// <typeparam name="TViewModel">The type of the ViewModel</typeparam>
        /// <param name="viewModel">The view model instance</param>
        /// <returns></returns>
        Task<TViewModel> PushModalAsync<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel;
    }
}

