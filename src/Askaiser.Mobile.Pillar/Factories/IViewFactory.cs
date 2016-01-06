using System;
using Askaiser.Mobile.Pillar.ViewModels;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Factories
{
    public interface IViewFactory
    {
        /// <summary>
        /// Bind a class type that implement <see cref="IViewModel"/> to a <see cref="Page"/> type for later retrieval.
        /// </summary>
        /// <typeparam name="TViewModel">The type of class that implement IViewModel</typeparam>
        /// <typeparam name="TView">The page bound to the view model</typeparam>
        void Register<TViewModel, TView>() 
            where TViewModel : class, IViewModel 
            where TView : Page;

        /// <summary>
        /// Retrieve the page bound to a specific class type that implement <see cref="IViewModel"/>.
        /// Also set the resolved view model as the binding context of the resolved page.
        /// </summary>
        /// <typeparam name="TViewModel">The type of class that implement IViewModel</typeparam>
        /// <param name="setStateAction">An action that will be executed on the resolved view model</param>
        /// <returns>The resolved page</returns>
        Page Resolve<TViewModel>(Action<TViewModel> setStateAction = null) 
            where TViewModel : class, IViewModel;

        /// <summary>
        /// Retrieve the page bound to a specific class type that implement <see cref="IViewModel"/>
        /// and return also the resolved instance of the view model as an out argument.
        /// Also set the resolved view model as the binding context of the resolved page.
        /// </summary>
        /// <typeparam name="TViewModel">The type of class that implement IViewModel</typeparam>
        /// <param name="viewModel">The resolved view model</param>
        /// <param name="setStateAction">An action that will be executed on the resolved view model</param>
        /// <returns>The resolved page</returns>
        Page Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null) 
            where TViewModel : class, IViewModel;

        /// <summary>
        /// Retrieve the page bound to the class type of the passed <see cref="IViewModel"/> instance.
        /// Also set the view model instance as the binding context of the resolved page.
        /// </summary>
        /// <typeparam name="TViewModel">The type of class that implement IViewModel</typeparam>
        /// <param name="viewModel">The view model instance</param>
        /// <returns>The resolved page</returns>
        Page Resolve<TViewModel>(TViewModel viewModel) 
            where TViewModel : class, IViewModel;
    }
}

