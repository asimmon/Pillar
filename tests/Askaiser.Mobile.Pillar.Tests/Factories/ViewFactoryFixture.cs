using Askaiser.Mobile.Pillar.Factories;
using Askaiser.Mobile.Pillar.Interfaces;
using Askaiser.Mobile.Pillar.Tests.Mocks;
using NUnit.Framework;

namespace Askaiser.Mobile.Pillar.Tests.Factories
{
    [TestFixture]
    public class ViewFactoryFixture
    {
        [Test]
        public void ResolvesViewFromViewModelWhenViewModelIsRegisteredWithViewType()
        {
            var container = new AspNetDependencyInjectionAdapter();
            container.RegisterSingleton<MockViewModel>();
            container.RegisterSingleton<MockView>();

            var viewFactory = new ViewFactory(container);

            viewFactory.Register<MockViewModel, MockView>();

            MockViewModel viewModel;

            var view = viewFactory.Resolve(out viewModel, x => x.Title = "Test Title");

            Assert.That(view, Is.Not.Null);
            Assert.That(view, Is.TypeOf<MockView>());
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Title, Is.EqualTo("Test Title"));

            view = viewFactory.Resolve<MockViewModel>(x => x.Title = "Test Title 2");

            Assert.That(view, Is.Not.Null);
            Assert.That(viewModel.Title, Is.EqualTo("Test Title 2"));

            view = viewFactory.Resolve(viewModel);

            Assert.That(view, Is.Not.Null);
            Assert.That(view, Is.TypeOf<MockView>());
        }
    }
}
