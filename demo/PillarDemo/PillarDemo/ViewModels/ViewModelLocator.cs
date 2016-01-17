using Autofac;

namespace PillarDemo.ViewModels
{
    public class ViewModelLocator
    {
        private readonly IComponentContext _dependencyResolver;

        public ViewModelLocator(IComponentContext dependencyResolver)
        {
            _dependencyResolver = dependencyResolver;
        }

        public HelpViewModel Help
        {
            get { return _dependencyResolver.Resolve<HelpViewModel>(); }
        }
    }
}
