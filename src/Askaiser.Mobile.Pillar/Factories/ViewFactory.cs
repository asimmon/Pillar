using System;
using System.Collections.Generic;
using Askaiser.Mobile.Pillar.ViewModels;
using Autofac;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Factories
{
    public class ViewFactory : IViewFactory
    {
        private readonly IDictionary<Type, Type> _map = new Dictionary<Type, Type>();
        private readonly IComponentContext _componentContext;

        public ViewFactory(IComponentContext componentContext)
        {
            _componentContext = componentContext;
            Instance = this;
        }

        public static IViewFactory Instance { get; private set; }

        public void Register<TViewModel, TView>()
            where TViewModel : class, IViewModel
            where TView : Page
        {
            _map[typeof(TViewModel)] = typeof(TView);
        }

        public Page Resolve<TViewModel>(Action<TViewModel> setStateAction = null) where TViewModel : class, IViewModel
        {
            TViewModel viewModel;
            return Resolve(out viewModel, setStateAction);
        }

        public Page Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null)
            where TViewModel : class, IViewModel
        {
            viewModel = _componentContext.Resolve<TViewModel>();

            var viewType = _map[typeof(TViewModel)];
            var view = _componentContext.Resolve(viewType) as Page;

            if (setStateAction != null)
                setStateAction(viewModel);

            view.BindingContext = viewModel;
            return view;
        }

        public Page Resolve<TViewModel>(TViewModel viewModel)
            where TViewModel : class, IViewModel
        {
            var type = viewModel.GetType();
            var viewType = _map[type];
            var view = _componentContext.Resolve(viewType) as Page;
            view.BindingContext = viewModel;
            return view;
        }
    }
}

