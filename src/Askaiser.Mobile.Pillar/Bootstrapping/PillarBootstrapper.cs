using Askaiser.Mobile.Pillar.Factories;
using Askaiser.Mobile.Pillar.Interfaces;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Bootstrapping
{
    /// <summary>
    /// This is where you configure, register and bind your ViewModels and Views.
    /// </summary>
    public abstract class PillarBootstrapper
    {
        public IContainerAdapter Container { get; protected set; }

        public Application App { get; private set; }

        protected PillarBootstrapper(Application app)
        {
            App = app;
        }

        public void Run()
        {
            Container = GetContainer();

            ConfigureContainerInternal(Container);

            var viewFactory = Container.Resolve<IViewFactory>();

            RegisterViews(viewFactory);

            ConfigureApplication(Container);
        }

        /// <summary>
        /// Creates the dependendy injection container used in the whole application.
        /// Override this method to create your own container decorator.
        /// </summary>
        /// <returns></returns>
        protected virtual IContainerAdapter GetContainer()
        {
            return new AspNetDependencyInjectionAdapter();
        }

        /// <summary>
        /// You can register your dependencies by overriding this method.
        /// Don't forget to call base.ConfigureContainer(builder) to register
        /// the dependencies of this library.
        /// </summary>
        /// <param name="container">Used to register dependencies</param>
        private void ConfigureContainerInternal(IContainerAdapter container)
        {
            var pillarModule = new PillarModule();
            pillarModule.Load(container);

            ConfigureContainer(container);
        }

        protected abstract void ConfigureContainer(IContainerAdapter container);

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
        private void ConfigureApplication(IContainerAdapter container)
        {
            var viewFactory = container.Resolve<IViewFactory>();

            var page = GetFirstPage(viewFactory);

            if (App != null) // for unit testing purpose only
            {
                App.MainPage = new NavigationPage(page);
            }
        }

        protected abstract Page GetFirstPage(IViewFactory viewFactory);
    }
}

