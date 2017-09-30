using Xamarin.Forms;
using Pillar;

namespace PillarDemo
{
    public class YourAppBootstrapper : PillarBootstrapper
    {
        // Instantiate this class in your Application class, after InitializeComponent()
        // new YourAppBootstrapper(this).Run();
        public YourAppBootstrapper(Application app)
            : base(app)
        { }

        protected override void RegisterDependencies(IContainerAdapter container)
        {
            // TODO Register your dependencies in the built-in IoC container

            // "New instance" per resolve and "single instance" registration examples
            // container.RegisterType<Bar, IBar>(); 
            // container.RegisterSingleton<Foo, IFoo>();

            // Registration without interfaces
            // container.RegisterType<HomeViewModel>(); 
            // container.RegisterType<HomePage>();
        }

        protected override void BindViewModelsToViews(IViewFactory viewFactory)
        {
            // TODO Bind your view models to pages

            // viewFactory.Register<HomeViewModel, HomePage>();
        }

        protected override Page GetFirstPage(IViewFactory viewFactory)
        {
            // TODO Return a Xamarin Forms Page resolved by the view factory

            // return viewFactory.Resolve<HomeViewModel>();
            return null;
        }
    }
}
