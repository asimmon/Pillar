using System.Threading.Tasks;
using Askaiser.Mobile.Pillar.Services;
using Askaiser.Mobile.Pillar.ViewModels;
using Xamarin.Forms;
using Xunit;

namespace Askaiser.Mobile.Pillar.Tests.Services
{
    public class NavigationExtensionsFixture
    {
        private class ViewModel : PillarViewModelBase { }

        private class View : ContentPage { }

        private readonly ViewModel _vmA;
        private readonly ViewModel _vmB;

        private readonly View _vA;
        private readonly View _vB;

        private readonly INavigation _navigation;

        public NavigationExtensionsFixture()
        {
            _vmA = new ViewModel();
            _vmB = new ViewModel();

            _vA = new View { BindingContext = _vmA };
            _vB = new View { BindingContext = _vmB };

            _navigation = new NavigationPage(_vA).Navigation;
        }

        [Fact]
        public void SimpleCurrentViewAndViewModelWithoutNavigation()
        {
            Assert.Same(_vA, _navigation.GetCurrentView());
            Assert.Same(_vmA, _navigation.GetCurrentViewModel());
        }

        [Fact]
        public async Task PreviousViewAndViewModelAfterNavigation()
        {
            await _navigation.PushAsync(_vB);

            Assert.Same(_vA, _navigation.GetPreviousView());
            Assert.Same(_vmA, _navigation.GetPreviousViewModel());
        }

        [Fact]
        public async Task CurrentViewAndViewModelAfterNavigation()
        {
            await _navigation.PushAsync(_vB);

            Assert.Same(_vB, _navigation.GetCurrentView());
            Assert.Same(_vmB, _navigation.GetCurrentViewModel());
        }

        [Fact]
        public async Task CurrentViewAndViewModelAfterNavigationWhenNotIViewModel()
        {
            var viewModel = new object();
            var view = new ContentPage
            {
                BindingContext = viewModel
            };

            await _navigation.PushAsync(view);

            Assert.Same(view, _navigation.GetCurrentView());
            Assert.Same(null, _navigation.GetCurrentViewModel()); // cannot cast to IViewModel
        }

        [Fact]
        public async Task FirstViewAndViewModelAfterNavigation()
        {
            await _navigation.PushAsync(_vB);

            Assert.Same(_vA, _navigation.GetFirstView());
            Assert.Same(_vmA, _navigation.GetFirstViewModel());
        }

        [Fact]
        public async Task FirstViewAndViewModelAfterNavigationPushAndPop()
        {
            await _navigation.PushAsync(_vB);

            Assert.Same(_vA, _navigation.GetFirstView());
            Assert.Same(_vmA, _navigation.GetFirstViewModel());
        }
    }
}
