using System.Threading.Tasks;

namespace Pillar
{
    /// <summary>
    /// Displays popups or actionsheet on any page.
    /// </summary>
    public interface IDialogProvider
    {
        /// <summary>
        /// Displays an alert popup with a cancel button
        /// </summary>
        /// <param name="title">The title of the popup</param>
        /// <param name="message">The message of the popup</param>
        /// <param name="cancel">The text of the cancel button</param>
        /// <returns>An awaitable Task that need to be awaited</returns>
        Task DisplayAlert(string title, string message, string cancel);

        /// <summary>
        /// Displays the alert with a OK button and a cancel button
        /// </summary>
        /// <param name="title">The title of the popup</param>
        /// <param name="message">The message of the popup</param>
        /// <param name="accept">The text of the OK button</param>
        /// <param name="cancel">The text of the cancel button</param>
        /// <returns>An awaitable Task that need to be awaited. True means that the OK button was pressed.</returns>
        Task<bool> DisplayAlert(string title, string message, string accept, string cancel);

        /// <summary>
        /// Displays a native platform action sheet, allowing the application user to choose from several buttons.
        /// </summary>
        /// <returns>An awaitable Task that displays an action sheet and returns the Text of the button pressed by the user.</returns>
        Task<string> DisplayActionSheet(string title, string cancel, string destruction, params string[] buttons);
    }
}

