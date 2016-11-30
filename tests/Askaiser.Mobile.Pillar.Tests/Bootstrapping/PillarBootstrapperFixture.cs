using Askaiser.Mobile.Pillar.Tests.Mocks;
using Xamarin.Forms;
using Xunit;

namespace Askaiser.Mobile.Pillar.Tests.Bootstrapping
{
    public class PillarBootstrapperFixture
    {
        [Fact]
        public void ConfiguresWhenRun()
        {
            var bootstrapper = new MockBootstrapper();
            bootstrapper.Run();

            Assert.NotNull(bootstrapper.ViewFactory);

            var view = bootstrapper.ViewFactory.Resolve<MockViewModel>();

            Assert.NotNull(view);

            Assert.NotNull(bootstrapper.Container);
        }
    }
}
