using System;
using Xamarin.Forms;

namespace Pillar
{
    /// <summary>
    /// This is where you configure, register and bind your ViewModels and Views.
    /// </summary>
    public abstract class PillarBootstrapper
    {
        public IContainerAdapter Container { get; protected set; }

        public Application App { get; private set; }

        /// <summary>
        /// Initialize an instance of the class <see cref="PillarBootstrapper"/>.
        /// </summary>
        /// <param name="app">The Xamarin.Forms application</param>
        protected PillarBootstrapper(Application app)
        {
            App = app;
        }

        /// <summary>
        /// Call this method to start your application.
        /// Show the first page configured with the <see cref="GetFirstPage"/> method.
        /// </summary>
        public void Run()
        {
            Container = GetContainer();

            RegisterAllDependencies(Container);

            var viewFactory = Container.Resolve<IViewFactory>();

            BindViewModelsToViews(viewFactory);

            ConfigureApplication(Container);
        }

        /// <summary>
        /// Creates the dependendy injection container used in the whole application.
        /// Override this method to create your own container decorator.
        /// </summary>
        /// <returns></returns>
        protected virtual IContainerAdapter GetContainer()
        {
            return new PillarDefaultIoc();
        }

        /// <summary>
        /// You can register your dependencies by overriding this method.
        /// Don't forget to call base.ConfigureContainer(builder) to register
        /// the dependencies of this library.
        /// </summary>
        /// <param name="container">Used to register dependencies</param>
        private void RegisterAllDependencies(IContainerAdapter container)
        {
            container.RegisterPillarDependencies();

            RegisterDependencies(container);
        }

        /// <summary>
        /// Implement this method to register your dependencies.
        /// </summary>
        /// <param name="container">The dependency container</param>
        protected abstract void RegisterDependencies(IContainerAdapter container);

        /// <summary>
        /// Implement this method to bind each View type to a ViewModel type.
        /// </summary>
        /// <param name="viewFactory">The View factory</param>
        protected abstract void BindViewModelsToViews(IViewFactory viewFactory);

        /// <summary>
        /// Implement this method to configure your application when it starts.
        /// For exemple, you can retrieve the view factory from the container,
        /// and then retrieve a page and set it as MainPage
        /// (you will need a reference to your Application instance).
        /// </summary>
        /// <param name="container">The dependency container</param>
        private void ConfigureApplication(IContainerAdapter container)
        {
            var viewFactory = container.Resolve<IViewFactory>();
            var navigator = container.Resolve<INavigator>();

            // Get the first page to show to the user
            var detailPage = GetFirstPage(viewFactory);
            if (detailPage == null)
                throw new ArgumentException($"An instance of a Page must be returned in {GetType().FullName}.GetFirstPage()");

            var detailViewModel = detailPage.BindingContext as IViewModel;

            // We might need to use a MasterDetailsPage if this method returns something
            var masterPage = GetMasterPage(viewFactory);
            IViewModel masterViewModel = null;

            if (masterPage != null)
            {
                masterViewModel = masterPage.BindingContext as IViewModel;
            }

            // The unit tests cannot go through this block as we don't have an application instance to test
            if (App != null)
            {
                // Navigation events, entering
                if (masterViewModel != null) masterViewModel.ViewEntering();
                if (detailViewModel != null) detailViewModel.ViewEntering();

                Page navPage = new PillarNavigationPage(detailPage, navigator);
                App.MainPage = masterPage == null
                    ? navPage
                    : new MasterDetailPage { Master = masterPage, Detail = navPage };

                // Navigation events, entered
                if (masterViewModel != null) masterViewModel.ViewEntered();
                if (detailViewModel != null) detailViewModel.ViewEntered();
            }
        }

        /// <summary>
        /// Implement this method to return the first page that will be shown to the user
        /// </summary>
        protected abstract Page GetFirstPage(IViewFactory viewFactory);

        /// <summary>
        /// Override this method if you want to use a MasterDetailsPage wrapper in your application
        /// </summary>
        protected virtual Page GetMasterPage(IViewFactory viewFactory)
        {
            return null;
        }
    }
}