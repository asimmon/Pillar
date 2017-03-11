using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Pillar
{
    public class ViewFactory : IViewFactory
    {
        private readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();
        private readonly IContainerAdapter _componentContext;

        public ViewFactory(IContainerAdapter componentContext)
        {
            _componentContext = componentContext;
        }

        /// <summary>
        /// Bind a class type that implement <see cref="IViewModel" /> to a <see cref="Page" /> type for later retrieval.
        /// </summary>
        /// <typeparam name="TViewModel">The type of class that implement IViewModel</typeparam>
        /// <typeparam name="TView">The page bound to the view model</typeparam>
        public void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Page
        {
            _map[typeof(TViewModel)] = typeof(TView);
        }

        /// <summary>
        /// Retrieve the page bound to a specific class type that implement <see cref="IViewModel" />.
        /// Also set the resolved view model as the binding context of the resolved page.
        /// </summary>
        /// <typeparam name="TViewModel">The type of class that implement IViewModel</typeparam>
        /// <param name="setStateAction">An action that will be executed on the resolved view model</param>
        /// <returns>
        /// The resolved page
        /// </returns>
        public Page Resolve<TViewModel>(Action<TViewModel> setStateAction = null) where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            return Resolve(out viewModel, setStateAction);
        }

        /// <summary>
        /// Retrieve the page bound to a specific class type that implement <see cref="IViewModel" />
        /// and return also the resolved instance of the view model as an out argument.
        /// Also set the resolved view model as the binding context of the resolved page.
        /// </summary>
        /// <typeparam name="TViewModel">The type of class that implement IViewModel</typeparam>
        /// <param name="viewModel">The resolved view model</param>
        /// <param name="setStateAction">An action that will be executed on the resolved view model</param>
        /// <returns>
        /// The resolved page
        /// </returns>
        public Page Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            viewModel = _componentContext.Resolve<TViewModel>();

            var viewModelType = typeof(TViewModel);

            Type viewType;
            if (!_map.TryGetValue(viewModelType, out viewType))
            {
                throw new InvalidOperationException($"Could not find a view type mapped to the view model type {viewModelType.FullName}");
            }

            var view = _componentContext.Resolve(viewType) as Page;

            if (setStateAction != null)
                setStateAction(viewModel);

            view.BindingContext = viewModel;
            return view;
        }

        /// <summary>
        /// Retrieve the page bound to the class type of the passed <see cref="IViewModel" /> instance.
        /// Also set the view model instance as the binding context of the resolved page.
        /// </summary>
        /// <typeparam name="TViewModel">The type of class that implement IViewModel</typeparam>
        /// <param name="viewModel">The view model instance</param>
        /// <returns>
        /// The resolved page
        /// </returns>
        public Page Resolve<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var viewModelType = viewModel.GetType();

            Type viewType;
            if (!_map.TryGetValue(viewModelType, out viewType))
            {
                throw new InvalidOperationException($"Could not a view type mapped to the view model type {viewModelType.FullName}");
            }

            var view = _componentContext.Resolve(viewType) as Page;
            view.BindingContext = viewModel;
            return view;
        }
    }
}

