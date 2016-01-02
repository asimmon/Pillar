using Askaiser.Mobile.Pillar.Interfaces;

namespace Askaiser.Mobile.Pillar.ViewModels
{
    /// <summary>
    /// A ViewModel abstraction implemented in <see cref="PillarViewModelBase"/>
    /// </summary>
    /// <seealso cref="INavigationAware" />
    public interface IViewModel : INavigationAware
    {
        /// <summary>
        /// Represents the page Title.
        /// It's up to you to bind this property to a Page title.
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// If true, the associated Page will not exists in the navigation stack.
        /// A good example is a login screen, once you are logged in and on a different page,
        /// if you press the back button you won't return to the login screen.
        /// </summary>
        bool NoHistory { get; set; }

        /// <summary>
        /// FOR XAML USE ONLY!
        /// Magical object which allow us to use the ViewModelLocator pattern without the limitations of a static context.
        /// It means that we can use dependency injection, that we can write testable code without static classed or methods.
        /// You will need to register a class with singleton lifecycle (the ViewModelLocator instance) 
        /// and add an RegistrationActivated event handler in Autofac.
        /// Every time that a IViewModel will be requested you will resolve and inject the the ViewModelLocator instance.
        /// See the online documentation for a concrete example.
        /// </summary>
        object Locator { get; set; }
    }
}

