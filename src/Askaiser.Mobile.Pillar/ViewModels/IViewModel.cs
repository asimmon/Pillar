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
    }
}

