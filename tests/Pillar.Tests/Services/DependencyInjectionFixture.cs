using System;
using Pillar.Interfaces;
using Xamarin.Forms;
using Xunit;

namespace Pillar.Tests.Services
{
    public class DependencyInjectionFixture
    {
        private IContainerAdapter _container;

        private interface IFoo
        { }

        private interface IBar
        {
            IFoo InnerFoo { get; }
        }

        private class Foo : IFoo
        { }

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
            _container = new PillarDefaultIoc();

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

        [Fact]
        public void ResolveUnregisteredDependency()
        {
            Assert.Throws<InvalidOperationException>(() => _container.Resolve<ContentPage>());
        }

        [Fact]
        public void ResolveNullType()
        {
            Assert.Throws<ArgumentNullException>(() => _container.Resolve(null));
        }

        [Fact]
        public void CannotRegisterDependencyAfterResolve()
        {
            _container.Resolve<IFoo>();

            Assert.Throws<InvalidOperationException>(() => _container.RegisterType<ContentPage>());
        }

        [Fact]
        public void RegisterTypeFactory()
        {
            _container = new PillarDefaultIoc();

            _container.RegisterType<IFoo>(() => new Foo());

            var foo1 = _container.Resolve<IFoo>();
            var foo2 = _container.Resolve<IFoo>();

            Assert.NotSame(foo1, foo2);
        }

        [Fact]
        public void RegisterSingletonFactory()
        {
            _container = new PillarDefaultIoc();

            _container.RegisterSingleton<IFoo>(() => new Foo());

            var foo1 = _container.Resolve<IFoo>();
            var foo2 = _container.Resolve<IFoo>();

            Assert.Same(foo1, foo2);
        }
    }
}