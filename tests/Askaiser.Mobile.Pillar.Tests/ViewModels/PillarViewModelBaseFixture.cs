using Askaiser.Mobile.Pillar.Tests.Mocks;
using NUnit.Framework;

namespace Askaiser.Mobile.Pillar.Tests.ViewModels
{
    [TestFixture]
    public class PillarViewModelBaseFixture
    {
        [Test]
        public void TestHandlesPropertyChanged()
        {
            bool propertyChanged = false;
            var viewModel = new MockViewModel();

            viewModel.PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Title" && viewModel.Title == "Test")
                    propertyChanged = true;
            };

            viewModel.Title = "Test";

            Assert.That(propertyChanged, Is.True);
        }
    }
}
