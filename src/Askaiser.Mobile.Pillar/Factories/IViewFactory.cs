using System;
using Askaiser.Mobile.Pillar.ViewModels;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Factories
{
    public interface IViewFactory
    {
        void Register<TViewModel, TView>() 
            where TViewModel : class, IViewModel 
            where TView : Page;

        Page Resolve<TViewModel>(Action<TViewModel> setStateAction = null) 
            where TViewModel : class, IViewModel;

        Page Resolve<TViewModel>(out TViewModel viewModel, Action<TViewModel> setStateAction = null) 
            where TViewModel : class, IViewModel;

        Page Resolve<TViewModel>(TViewModel viewModel) 
            where TViewModel : class, IViewModel;
    }
}

