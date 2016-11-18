using Askaiser.Mobile.Pillar.Factories;
using Askaiser.Mobile.Pillar.Interfaces;
using Askaiser.Mobile.Pillar.Tests.Mocks;
using Xunit;

namespace Askaiser.Mobile.Pillar.Tests.Factories
{
    public class ViewFactoryFixture
    {
        [Fact]
        public void ResolvesViewFromViewModelWhenViewModelIsRegisteredWithViewType()
        {
            var container = new AspNetDependencyInjectionAdapter();
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
    }
}
