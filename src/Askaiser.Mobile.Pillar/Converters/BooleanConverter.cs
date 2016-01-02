using System;
using System.Collections.Generic;
using System.Globalization;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Converters
{
    /// <summary>
    /// Provides an abstract type converter to convert
    /// boolean objects to and from a generic type.
    /// </summary>
    /// <typeparam name="T">Type of the items to convert to and from the boolean objects.</typeparam>
    public abstract class BooleanConverter<T> : IValueConverter
    {
        /// <summary>
        /// Generic object to return when the value to convert is true.
        /// </summary>
        public T TrueObject { get; set; }

        /// <summary>
        /// Generic object to return when the value to convert is false.
        /// </summary>
        public T FalseObject { get; set; }

        protected BooleanConverter(T trueObjectValue, T falseObjectValue)
        {
            TrueObject = trueObjectValue;
            FalseObject = falseObjectValue;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool && ((bool)value) ? TrueObject : FalseObject;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is T && EqualityComparer<T>.Default.Equals((T)value, TrueObject);
        }
    }
}
