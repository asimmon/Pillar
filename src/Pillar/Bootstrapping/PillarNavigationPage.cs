using System;
using Xamarin.Forms;

namespace Pillar
{
    /// <summary>
    /// Custom navigation page that call Pillar's navigator to pop back when
    /// the back button is pressed. Navigation events will be triggered.
    /// </summary>
    /// <seealso cref="INavigationAware"/>
    internal sealed class PillarNavigationPage : NavigationPage
    {
        private readonly INavigator _navigator;

        public PillarNavigationPage(Page root, INavigator navigator)
            : base(root)
        {
            if (navigator == null)
                throw new ArgumentNullException(nameof(navigator));

            _navigator = navigator;
        }

        protected override bool OnBackButtonPressed()
        {
            // remove modal page from stack if exists
            if (CurrentPage.SendBackButtonPressed())
                return true;

            _navigator.PopAsync().ContinueWith(task =>
            {
                if (task.IsFaulted)
                    throw task.Exception;
            });

            return true;
        }
    }
}