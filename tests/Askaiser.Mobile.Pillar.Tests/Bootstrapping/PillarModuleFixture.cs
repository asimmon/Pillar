using Askaiser.Mobile.Pillar.Bootstrapping;
using NUnit.Framework;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Tests.Bootstrapping
{
    [TestFixture]
    public class PillarModuleFixture
    {
        private PillarModule _module;

        [SetUp]
        public void RunBeforeAnyTests()
        {
            _module = new PillarModule();

        }

        [Test]
        public void GetCurrentPageForSimplePage()
        {
            var rootPage = new ContentPage();

            var currentPage = _module.GetCurrentPage(rootPage);

            Assert.AreSame(rootPage, currentPage);
        }

        [Test]
        public void GetCurrentPageForNavigationPage()
        {
            var page = new ContentPage();
            var rootPage = new NavigationPage(page);

            var currentPage = _module.GetCurrentPage(rootPage);

            Assert.AreSame(page, currentPage);
        }

        [Test]
        public async void GetCurrentPageForNavigationPageWithManyPages()
        {
            var firstPage = new ContentPage();
            var secondPage = new ContentPage();

            var rootPage = new NavigationPage(firstPage);

            await rootPage.PushAsync(secondPage);

            var currentPage = _module.GetCurrentPage(rootPage);

            Assert.AreSame(secondPage, currentPage);
        }

        [Test]
        public void GetCurrentPageForMasterDetailPage()
        {
            var masterPage = new ContentPage
            {
                Title = "mandatory title"
            };
            var detailPage = new ContentPage();

            var rootPage = new MasterDetailPage
            {
                Master = masterPage,
                Detail = detailPage
            };

            var currentPage = _module.GetCurrentPage(rootPage);

            Assert.AreSame(detailPage, currentPage);
        }

        [Test]
        public void GetCurrentPageForNavigationPageInsideMasterDetailPage()
        {
            var masterPage = new ContentPage
            {
                Title = "mandatory title"
            };

            var page = new ContentPage();

            var detailPage = new NavigationPage(page);

            var rootPage = new MasterDetailPage
            {
                Master = masterPage,
                Detail = detailPage
            };

            var currentPage = _module.GetCurrentPage(rootPage);

            Assert.AreSame(page, currentPage);
        }
    }
}
