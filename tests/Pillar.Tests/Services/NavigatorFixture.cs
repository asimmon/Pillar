using System;
using System.IO;
using System.Threading.Tasks;
using Moq;
using Pillar.Tests.Mocks;
using Xamarin.Forms;
using Xunit;

namespace Pillar.Tests.Services
{
    public class NavigatorFixture
    {
        private readonly Navigator _navigator;
        private readonly NavigationPage _navigationPage;

        private readonly TextWriter _trace;

        private readonly ViewModelA _vmA;
        private readonly ViewModelB _vmB;
        private readonly ViewModelC _vmC;

        private readonly ViewA _vA;
        private readonly ViewB _vB;
        private readonly ViewC _vC;

        protected abstract class TraceableViewModel : PillarViewModelBase
        {
            private readonly string _id;
            private readonly TextWriter _trace;

            protected TraceableViewModel(string id, TextWriter trace)
            {
                _id = id;
                _trace = trace;
            }

            public override void ViewEntering() => _trace.Write($"eg{_id}-");
            public override void ViewEntered() => _trace.Write($"ed{_id}-");
            public override void ViewLeaving() => _trace.Write($"lg{_id}-");
            public override void ViewLeaved() => _trace.Write($"ld{_id}-");

            public override string ToString() => $"ViewModel {_id.ToUpper()}";
        }

        protected class ViewModelA : TraceableViewModel
        {
            public ViewModelA(TextWriter trace) : base("a", trace) { }
        }

        protected class ViewModelB : TraceableViewModel
        {
            public ViewModelB(TextWriter trace) : base("b", trace) { }
        }

        protected class ViewModelC : TraceableViewModel
        {
            public ViewModelC(TextWriter trace) : base("c", trace) { }
        }

        private class ViewA : ContentPage { }

        private class ViewB : ContentPage { }

        private class ViewC : ContentPage { }

        public NavigatorFixture()
        {
            _trace = new StringWriter();

            _vmA = new ViewModelA(_trace);
            _vmB = new ViewModelB(_trace);
            _vmC = new ViewModelC(_trace);

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
        public async Task PushAsyncBThenPushAsyncCWithInstances()
        {
            Assert.Same(_vA, _navigationPage.CurrentPage);
            Assert.Same(_vmA, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(1, _navigationPage.Navigation.NavigationStack.Count);

            await _navigator.PushAsync(_vmB);
            Assert.Same(_vB, _navigationPage.CurrentPage);
            Assert.Same(_vmB, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(2, _navigationPage.Navigation.NavigationStack.Count);

            await _navigator.PushAsync(_vmC);
            Assert.Same(_vC, _navigationPage.CurrentPage);
            Assert.Same(_vmC, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(3, _navigationPage.Navigation.NavigationStack.Count);

            // Read like this : entering B, leaving A, entered B, leaved A, entering C, etc.
            Assert.Equal("egb-lga-edb-lda-egc-lgb-edc-ldb-", _trace.ToString());
        }

        [Fact]
        public async Task PushAsyncBThenPushAsyncCWithenerics()
        {
            Assert.Same(_vA, _navigationPage.CurrentPage);
            Assert.Same(_vmA, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(1, _navigationPage.Navigation.NavigationStack.Count);

            await _navigator.PushAsync<ViewModelB>();
            Assert.Same(_vB, _navigationPage.CurrentPage);
            Assert.Same(_vmB, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(2, _navigationPage.Navigation.NavigationStack.Count);

            await _navigator.PushAsync<ViewModelC>();
            Assert.Same(_vC, _navigationPage.CurrentPage);
            Assert.Same(_vmC, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(3, _navigationPage.Navigation.NavigationStack.Count);

            Assert.Equal("egb-lga-edb-lda-egc-lgb-edc-ldb-", _trace.ToString());
        }

        [Fact]
        public async Task PushAsyncBThenPushAsyncCWhenAAndBHaveNoHistoryWithInstances()
        {
            _vmA.NoHistory = true;
            _vmB.NoHistory = true;

            await _navigator.PushAsync(_vmB);
            Assert.Same(_vB, _navigationPage.CurrentPage);
            Assert.Same(_vmB, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(1, _navigationPage.Navigation.NavigationStack.Count);

            await _navigator.PushAsync(_vmC);
            Assert.Same(_vC, _navigationPage.CurrentPage);
            Assert.Same(_vmC, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(1, _navigationPage.Navigation.NavigationStack.Count);

            Assert.Equal("egb-lga-edb-lda-egc-lgb-edc-ldb-", _trace.ToString());
        }

        [Fact]
        public async Task PushAsyncBThenPushAsyncCWhenAAndBHaveNoHistoryWithGenerics()
        {
            _vmA.NoHistory = true;
            _vmB.NoHistory = true;

            await _navigator.PushAsync<ViewModelB>();
            Assert.Same(_vB, _navigationPage.CurrentPage);
            Assert.Same(_vmB, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(1, _navigationPage.Navigation.NavigationStack.Count);

            await _navigator.PushAsync<ViewModelC>();
            Assert.Same(_vC, _navigationPage.CurrentPage);
            Assert.Same(_vmC, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(1, _navigationPage.Navigation.NavigationStack.Count);

            Assert.Equal("egb-lga-edb-lda-egc-lgb-edc-ldb-", _trace.ToString());
        }

        [Fact]
        public async Task PushAsyncBThenPushAsyncCWhenAHaveNoHistoryWithInstanceThenGenerics()
        {
            _vmA.NoHistory = true;
            _vmB.NoHistory = false;

            await _navigator.PushAsync(_vmB);
            Assert.Same(_vB, _navigationPage.CurrentPage);
            Assert.Same(_vmB, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(1, _navigationPage.Navigation.NavigationStack.Count);

            await _navigator.PushAsync<ViewModelC>();
            Assert.Same(_vC, _navigationPage.CurrentPage);
            Assert.Same(_vmC, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(2, _navigationPage.Navigation.NavigationStack.Count);

            Assert.Equal("egb-lga-edb-lda-egc-lgb-edc-ldb-", _trace.ToString());
        }

        [Fact]
        public async Task PushAsyncBThenPopAsyncWithInstance()
        {
            await _navigator.PushAsync(_vmB);
            Assert.Same(_vB, _navigationPage.CurrentPage);
            Assert.Same(_vmB, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(2, _navigationPage.Navigation.NavigationStack.Count);

            await _navigator.PopAsync();
            Assert.Same(_vA, _navigationPage.CurrentPage);
            Assert.Same(_vmA, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(1, _navigationPage.Navigation.NavigationStack.Count);

            Assert.Equal("egb-lga-edb-lda-ega-lgb-eda-ldb-", _trace.ToString());
        }

        [Fact]
        public async Task PopAsyncWithInstanceButOnlyOnePageInNavigationStack()
        {
            await _navigator.PopAsync();
            await _navigator.PopAsync();
            await _navigator.PopAsync();
            Assert.Same(_vA, _navigationPage.CurrentPage);
            Assert.Same(_vmA, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(1, _navigationPage.Navigation.NavigationStack.Count);
        }

        [Fact]
        public async Task PopToRootAsyncWithManyViewsInNavigationStack()
        {
            await _navigator.PushAsync(_vmB);
            await _navigator.PushAsync(_vmC);
            await _navigator.PopToRootAsync();

            Assert.Same(_vA, _navigationPage.CurrentPage);
            Assert.Same(_vmA, _navigationPage.CurrentPage.BindingContext);
            Assert.Equal(1, _navigationPage.Navigation.NavigationStack.Count);

            Assert.Equal("egb-lga-edb-lda-egc-lgb-edc-ldb-ega-lgc-eda-ldc-", _trace.ToString());
        }
    }
}
