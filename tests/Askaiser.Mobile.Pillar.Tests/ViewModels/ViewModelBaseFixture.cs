using System.ComponentModel;
using Askaiser.Mobile.Pillar.Tests.Mocks;
using NUnit.Framework;

namespace Askaiser.Mobile.Pillar.Tests.ViewModels
{
    [TestFixture]
    public class ViewModelBaseFixture
    {
        private MockViewModel _viewModel;

        [SetUp]
        public void RunBeforeAnyTests()
        {
            _viewModel = new MockViewModel();
        }

        [Test]
        public void SetPropertyByLambda()
        {
            var handler = new PropertyChangedEventHandler((sender, args) =>
            {
                Assert.That(sender, Is.EqualTo(_viewModel));
                Assert.That(args, Is.Not.Null);

                Assert.That(args.PropertyName, Is.EqualTo("Foo"));
            });

            try
            {
                _viewModel.PropertyChanged += handler;
                _viewModel.Foo = "Bar";
            }
            finally
            {
                _viewModel.PropertyChanged -= handler;
            }
        }
    }
}
