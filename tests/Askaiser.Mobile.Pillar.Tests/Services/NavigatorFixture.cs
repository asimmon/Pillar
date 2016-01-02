using System;
using Askaiser.Mobile.Pillar.Factories;
using Askaiser.Mobile.Pillar.Interfaces;
using Askaiser.Mobile.Pillar.Services;
using Askaiser.Mobile.Pillar.Tests.Mocks;
using Moq;
using NUnit.Framework;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Tests.Services
{
    [TestFixture]
    public class NavigatorFixture
    {
        private MockViewModel _viewModel;
        private Navigator _navigator;
        private Action<MockViewModel> _action;

        [SetUp]
        public void SetUp()
        {
            _action = x => x.Title = "Test";
            _viewModel = new MockViewModel();
            var navigation = new Mock<INavigation>();
            var page = new Mock<IPage>();

            page.Setup(x => x.Navigation).Returns(navigation.Object);

            navigation.Setup(x => x.PopAsync()).ReturnsAsync(new Page { BindingContext = new MockViewModel() });
            navigation.Setup(x => x.PopModalAsync()).ReturnsAsync(new Page { BindingContext = new MockViewModel() });
            navigation.Setup(x => x.PopToRootAsync());

            var viewFactory = new Mock<IViewFactory>();
            viewFactory.Setup(x => x.Resolve(out _viewModel, _action)).Returns(new MockView());

            _navigator = new Navigator(page.Object, viewFactory.Object);
        }

        [Test]
        public async void NavigateToView()
        {
            var viewModel = await _navigator.PushAsync(_action);
            Assert.That(viewModel, Is.EqualTo(_viewModel));
        }

        [Test]
        public async void NavigateToModalView()
        {
            var viewModel = await _navigator.PushModalAsync(_action);
            Assert.That(viewModel, Is.EqualTo(_viewModel));
        }

        [Test]
        public async void NavigateFromView()
        {
            var viewModel = await _navigator.PopAsync();
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel, Is.TypeOf<MockViewModel>());
        }

        [Test]
        public async void NavigateFromModalView()
        {
            var viewModel = await _navigator.PopModalAsync();
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel, Is.TypeOf<MockViewModel>());
        }

        [Test]
        public async void NavigateToRoot()
        {
            var viewModel = await _navigator.PopModalAsync();
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel, Is.TypeOf<MockViewModel>());
        }
    }
}
