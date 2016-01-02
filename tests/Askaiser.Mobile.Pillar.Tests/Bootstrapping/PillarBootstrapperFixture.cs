using Askaiser.Mobile.Pillar.Tests.Mocks;
using NUnit.Framework;

namespace Askaiser.Mobile.Pillar.Tests.Bootstrapping
{
    [TestFixture]
    public class PillarBootstrapperFixture
    {
        [Test]
        public void ConfiguresWhenRun()
        {
            var bootstrapper = new MockBootstrapper();
            bootstrapper.Run();

            Assert.That(bootstrapper.ViewFactory, Is.Not.Null);

            var view = bootstrapper.ViewFactory.Resolve<MockViewModel>();

            Assert.That(view, Is.Not.Null);

            Assert.That(bootstrapper.Container, Is.Not.Null);
        }
    }
}
