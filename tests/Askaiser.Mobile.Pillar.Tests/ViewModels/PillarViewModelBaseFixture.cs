using Askaiser.Mobile.Pillar.Tests.Mocks;
using Xunit;

namespace Askaiser.Mobile.Pillar.Tests.ViewModels
{
    public class PillarViewModelBaseFixture
    {
        [Fact]
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

            Assert.Equal(true, propertyChanged);
        }
    }
}
