using Askaiser.Mobile.Pillar.Interfaces;
using NUnit.Framework;

namespace Askaiser.Mobile.Pillar.Tests.Services
{
    [TestFixture]
    public class DependencyInjectionFixture
    {
        private IContainerAdapter _container;

        private interface IFoo { }

        private interface IBar
        {
            IFoo InnerFoo { get; }
        }

        private class Foo : IFoo { }

        private class Bar : IBar
        {
            public IFoo InnerFoo { get; }

            public Bar(IFoo foo)
            {
                InnerFoo = foo;
            }
        }

        [SetUp]
        public void BeforeEachTest()
        {
            _container = new AspNetDependencyInjectionAdapter();

            _container.RegisterType<IFoo, Foo>();
            _container.RegisterType<IBar, Bar>();
        }

        [Test]
        public void SimpleTransients()
        {
            var foo = _container.Resolve<IFoo>();
            var bar = _container.Resolve<IBar>();

            Assert.NotNull(foo);
            Assert.NotNull(bar);
            Assert.NotNull(bar.InnerFoo);
        }

        [Test]
        public void MultipleTransientsAreNotTheSame()
        {
            var foo1 = _container.Resolve<IFoo>();
            var foo2 = _container.Resolve<IFoo>();

            Assert.AreNotSame(foo1, foo2);
        }
    }

}
