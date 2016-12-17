using System;
using Moq;
using Pillar.Factories;
using Pillar.Interfaces;
using Pillar.Tests.Mocks;
using Xunit;

namespace Pillar.Tests.Factories
{
    public class ViewFactoryFixture
    {
        [Fact]
        public void ResolvesViewFromViewModelWhenViewModelIsRegisteredWithViewType()
        {
            var container = new PillarDefaultIoc();
            container.RegisterSingleton<MockViewModel>();
            container.RegisterSingleton<MockView>();

            var viewFactory = new ViewFactory(container);

            viewFactory.Register<MockViewModel, MockView>();

            MockViewModel viewModel;

            var view = viewFactory.Resolve(out viewModel, x => x.Title = "Test Title");

            Assert.NotNull(view);
            Assert.IsType<MockView>(view);
            Assert.NotNull(viewModel);
            Assert.Equal("Test Title", viewModel.Title);

            view = viewFactory.Resolve<MockViewModel>(x => x.Title = "Test Title 2");

            Assert.NotNull(view);
            Assert.Equal("Test Title 2", viewModel.Title);

            view = viewFactory.Resolve(viewModel);

            Assert.NotNull(view);
            Assert.IsType<MockView>(view);
        }

        [Fact]
        public void ResolveWithOutParameterUnregisteredViewViewModelPairThrowsException()
        {
            var fakeContainer = new Mock<IContainerAdapter>().Object;
            var viewFactory = new ViewFactory(fakeContainer);

            MockViewModel viewModel;

            Assert.Throws<InvalidOperationException>(() => viewFactory.Resolve(out viewModel));
        }

        [Fact]
        public void ResolveUnregisteredViewViewModelPairThrowsException()
        {
            var fakeContainer = new Mock<IContainerAdapter>().Object;
            var viewFactory = new ViewFactory(fakeContainer);

            Assert.Throws<InvalidOperationException>(() => viewFactory.Resolve<MockViewModel>());
        }

        [Fact]
        public void ResolveInstanceUnregisteredViewViewModelPairThrowsException()
        {
            var fakeContainer = new Mock<IContainerAdapter>().Object;
            var viewFactory = new ViewFactory(fakeContainer);

            var viewModel = new MockViewModel();
            Assert.Throws<InvalidOperationException>(() => viewFactory.Resolve(viewModel));
        }
    }
}
