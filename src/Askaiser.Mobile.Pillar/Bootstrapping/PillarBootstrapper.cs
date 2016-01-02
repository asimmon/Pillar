using Askaiser.Mobile.Pillar.Factories;
using Autofac;

namespace Askaiser.Mobile.Pillar.Bootstrapping
{
    /// <summary>
    /// This is where you configure, register and bind your ViewModels and Views.
    /// </summary>
    public abstract class PillarBootstrapper
    {
        public void Run()
        {
            var builder = new ContainerBuilder();

            ConfigureContainer(builder);

            var container = builder.Build();
            var viewFactory = container.Resolve<IViewFactory>();

            RegisterViews(viewFactory);

            ConfigureApplication(container);
        }

        /// <summary>
        /// You can register your dependencies by overriding this method.
        /// Don't forget to call base.ConfigureContainer(builder) to register
        /// the dependencies of this library.
        /// </summary>
        /// <param name="builder">Used to register dependencies</param>
        protected virtual void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<PillarModule>();
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
        protected abstract void ConfigureApplication(IContainer container);
    }
}

