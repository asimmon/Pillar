using Askaiser.Mobile.Pillar.Factories;
using Askaiser.Mobile.Pillar.Ioc;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;
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
            var builder = new ServiceCollection();
            builder.AddSingleton<MockViewModel>();
            builder.AddSingleton<MockView>();
            var container = builder.BuildServiceProvider();

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
