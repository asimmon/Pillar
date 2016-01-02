﻿using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Interfaces
{
    /// <summary>
    /// Page abstraction that allow us to always get the current singular Page
    /// (not MasterDetailPage or NavigationPage).
    /// </summary>
    /// <seealso cref="IDialogProvider" />
    public interface IPage : IDialogProvider
    {
        INavigation Navigation { get; }
    }
}
