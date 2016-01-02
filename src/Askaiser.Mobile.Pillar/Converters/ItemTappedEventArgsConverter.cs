using System;
using System.Globalization;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Converters
{
    /// <summary>
    /// Converts a ItemTappedEventArgs event to its Item.
    /// Generally, the Item is the BindingContext of the tapped item.
    /// </summary>
    public class ItemTappedEventArgsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var eventArgs = value as ItemTappedEventArgs;
            if (eventArgs == null)
                throw new ArgumentException("Expected TappedEventArgs as value", "value");

            return eventArgs.Item;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
