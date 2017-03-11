using System;
using Xamarin.Forms;

namespace Pillar
{
    /// <summary>
    /// Wrapper for a DataTemplate.
    /// Unfortunately the default constructor for DataTemplate is internal
    /// so I had to wrap the DataTemplate instead of inheriting it.
    /// </summary>
    /// <typeparam name="T">The object type that this DataTemplateWrapper matches</typeparam>
    [ContentProperty("WrappedTemplate")]
    public class DataTemplateWrapper<T> : BindableObject, IDataTemplateWrapper
    {
        public static readonly BindableProperty WrappedTemplateProperty = BindableProperty.Create("WrappedTemplate", typeof(DataTemplate), typeof(DataTemplateWrapper<T>));
        public static readonly BindableProperty IsDefaultProperty = BindableProperty.Create("IsDefault", typeof(bool), typeof(DataTemplateWrapper<T>), false);

        public bool IsDefault
        {
            get { return (bool)GetValue(IsDefaultProperty); }
            set { SetValue(IsDefaultProperty, value); }
        }

        public DataTemplate WrappedTemplate
        {
            get { return (DataTemplate)GetValue(WrappedTemplateProperty); }
            set { SetValue(WrappedTemplateProperty, value); }
        }

        public Type Type
        {
            get { return typeof(T); }
        }
    }

}
