using System;
using Askaiser.Mobile.Pillar.Factories;
using Askaiser.Mobile.Pillar.Ioc;
using Askaiser.Mobile.Pillar.Ioc.Abstractions;

namespace Askaiser.Mobile.Pillar.Bootstrapping
{
    /// <summary>
    /// This is where you configure, register and bind your ViewModels and Views.
    /// </summary>
    public abstract class PillarBootstrapper
    {
        public void Run()
        {
            var builder = new ServiceCollection();

            ConfigureContainer(builder);

            var container = builder.BuildServiceProvider();
            var viewFactory = container.GetService<IViewFactory>();

            RegisterViews(viewFactory);

            ConfigureApplication(container);
        }

        /// <summary>
        /// You can register your dependencies by overriding this method.
        /// Don't forget to call base.ConfigureContainer(builder) to register
        /// the dependencies of this library.
        /// </summary>
        /// <param name="builder">Used to register dependencies</param>
        protected virtual void ConfigureContainer(IServiceCollection builder)
        {
            var pillarModule = new PillarModule();
            pillarModule.Load(builder);
        }

        /// <summary>
        /// Implement this method to bind each View type to a ViewModel type.
        /// </summary>
        /// <param name="viewFactory">The View factory</param>
        protected abstract void RegisterViews(IViewFactory viewFactory);

        /// <summary>
        /// Implement this method to configure your application when it starts.
        /// For exemple, you can retrieve the view factory from the container,
        /// and then retrieve a page and set it as MainPage
        /// (you will need a reference to your Application instance).
        /// </summary>
        /// <param name="container">The Autofac dependencies container</param>
        protected abstract void ConfigureApplication(IServiceProvider container);
    }
}

