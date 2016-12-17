using Xamarin.Forms;

namespace Pillar.Interfaces
{
    /// <summary>
    /// Page abstraction that allow us to always get the current singular Page
    /// (not MasterDetailPage or NavigationPage).
    /// </summary>
    /// <seealso cref="IDialogProvider" />
    public interface IPage : IDialogProvider
    {
        /// <summary>
        /// The current navigation object
        /// </summary>
        INavigation Navigation { get; }
    }
}

