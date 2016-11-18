using Askaiser.Mobile.Pillar.Interfaces;
using Xunit;

namespace Askaiser.Mobile.Pillar.Tests.Services
{
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

        public DependencyInjectionFixture()
        {
            _container = new AspNetDependencyInjectionAdapter();

            _container.RegisterType<IFoo, Foo>();
            _container.RegisterType<IBar, Bar>();
        }

        [Fact]
        public void SimpleTransients()
        {
            var foo = _container.Resolve<IFoo>();
            var bar = _container.Resolve<IBar>();

            Assert.NotNull(foo);
            Assert.NotNull(bar);
            Assert.NotNull(bar.InnerFoo);
        }

        [Fact]
        public void MultipleTransientsAreNotTheSame()
        {
            var foo1 = _container.Resolve<IFoo>();
            var foo2 = _container.Resolve<IFoo>();

            Assert.NotSame(foo2, foo1);
        }
    }

}
