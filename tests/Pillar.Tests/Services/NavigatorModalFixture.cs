using System;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using Pillar.Tests.Mocks;
using Xamarin.Forms;
using Xunit;

namespace Pillar.Tests.Services
{
    public class NavigatorModalFixture
    {
        private readonly Navigator _navigator;
        private readonly NavigationPage _navigationPage;

        private readonly ViewModelA _vmA;
        private readonly ViewModelB _vmB;
        private readonly ViewModelC _vmC;

        private readonly ViewA _vA;
        private readonly ViewB _vB;
        private readonly ViewC _vC;

        private class ViewModelA : PillarViewModelBase
        { }

        private class ViewModelB : PillarViewModelBase
        { }

        private class ViewModelC : PillarViewModelBase
        { }

        private class ViewA : ContentPage { }

        private class ViewB : ContentPage { }

        private class ViewC : ContentPage { }

        public NavigatorModalFixture()
        {
            _vmA = new ViewModelA();
            _vmB = new ViewModelB();
            _vmC = new ViewModelC();

            _vA = new ViewA { BindingContext = _vmA, Title = "a" };
            _vB = new ViewB { BindingContext = _vmB, Title = "b" };
            _vC = new ViewC { BindingContext = _vmC, Title = "c" };

            _navigationPage = new NavigationPage(_vA);

            var pageMock = new Mock<IPage>();
            pageMock.Setup(p => p.Navigation).Returns(() => _navigationPage.CurrentPage.Navigation);

            var viewFactoryMock = new Mock<IViewFactory>();
            viewFactoryMock.Setup(vf => vf.Resolve(It.IsAny<Action<ViewModelA>>())).Returns(() => _vA);
            viewFactoryMock.Setup(vf => vf.Resolve(It.IsAny<Action<ViewModelB>>())).Returns(() => _vB);
            viewFactoryMock.Setup(vf => vf.Resolve(It.IsAny<Action<ViewModelC>>())).Returns(() => _vC);

            ViewModelA unusedA;
            ViewModelB unusedB;
            ViewModelC unusedC;
            viewFactoryMock.Setup(vf => vf.Resolve(out unusedA, It.IsAny<Action<ViewModelA>>()));

            viewFactoryMock.Setup(vf => vf.Resolve(out unusedB, It.IsAny<Action<ViewModelB>>()))
                .OutCallback((out ViewModelB vm, Action<ViewModelB> setStateAction) => vm = _vmB)
                .Returns(() => _vB);

            viewFactoryMock.Setup(vf => vf.Resolve(out unusedC, It.IsAny<Action<ViewModelC>>()))
                .OutCallback((out ViewModelC vm, Action<ViewModelC> setStateAction) => vm = _vmC)
                .Returns(() => _vC);

            viewFactoryMock.Setup(vf => vf.Resolve(_vmA)).Returns(_vA);
            viewFactoryMock.Setup(vf => vf.Resolve(_vmB)).Returns(_vB);
            viewFactoryMock.Setup(vf => vf.Resolve(_vmC)).Returns(_vC);

            _navigator = new Navigator(pageMock.Object, viewFactoryMock.Object);
        }

        [Fact]
        public async Task PushModalAsyncGenericsReturnsPushedViewModel()
        {
            var viewModel = await _navigator.PushModalAsync<ViewModelB>();

            Assert.Same(_vmB, viewModel);
        }

        [Fact]
        public async Task PushModalAsyncGenerics()
        {
            Assert.Same(_vA, _navigationPage.CurrentPage);
            Assert.Same(_vmA, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(1, _navigationPage.Navigation.NavigationStack.Count);

            await _navigator.PushModalAsync<ViewModelB>();

            Assert.Same(_vA, _navigationPage.CurrentPage);
            Assert.Same(_vmA, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(1, _navigationPage.Navigation.NavigationStack.Count);

            Assert.Equal(1, _navigationPage.Navigation.ModalStack.Count);
            Assert.Same(_vB, _navigationPage.Navigation.ModalStack.Last());
            Assert.Same(_vmB, _navigationPage.Navigation.ModalStack.Last().BindingContext);

            await _navigator.PushModalAsync<ViewModelC>();

            Assert.Equal(2, _navigationPage.Navigation.ModalStack.Count);
            Assert.Same(_vC, _navigationPage.Navigation.ModalStack.Last());
            Assert.Same(_vmC, _navigationPage.Navigation.ModalStack.Last().BindingContext);
        }

        [Fact]
        public async Task PushModalAsyncInstance()
        {
            Assert.Same(_vA, _navigationPage.CurrentPage);
            Assert.Same(_vmA, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(1, _navigationPage.Navigation.NavigationStack.Count);

            await _navigator.PushModalAsync(_vmB);

            Assert.Same(_vA, _navigationPage.CurrentPage);
            Assert.Same(_vmA, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(1, _navigationPage.Navigation.NavigationStack.Count);

            Assert.Equal(1, _navigationPage.Navigation.ModalStack.Count);
            Assert.Same(_vB, _navigationPage.Navigation.ModalStack.Last());
            Assert.Same(_vmB, _navigationPage.Navigation.ModalStack.Last().BindingContext);

            await _navigator.PushModalAsync(_vmC);

            Assert.Equal(2, _navigationPage.Navigation.ModalStack.Count);
            Assert.Same(_vC, _navigationPage.Navigation.ModalStack.Last());
            Assert.Same(_vmC, _navigationPage.Navigation.ModalStack.Last().BindingContext);
        }

        [Fact]
        public async Task PushThenPopAsyncGenerics()
        {
            await _navigator.PushModalAsync<ViewModelB>();

            Assert.Equal(1, _navigationPage.Navigation.ModalStack.Count);
            Assert.Same(_vB, _navigationPage.Navigation.ModalStack.Last());
            Assert.Same(_vmB, _navigationPage.Navigation.ModalStack.Last().BindingContext);

            var viewModel = await _navigator.PopModalAsync();

            Assert.Same(_vmB, viewModel);
            Assert.Equal(0, _navigationPage.Navigation.ModalStack.Count);
        }

        [Fact]
        public async Task PushThenPopAsyncInstance()
        {
            await _navigator.PushModalAsync(_vmB);

            Assert.Equal(1, _navigationPage.Navigation.ModalStack.Count);
            Assert.Same(_vB, _navigationPage.Navigation.ModalStack.Last());
            Assert.Same(_vmB, _navigationPage.Navigation.ModalStack.Last().BindingContext);

            await _navigator.PopModalAsync();

            Assert.Equal(0, _navigationPage.Navigation.ModalStack.Count);
        }
    }
}
