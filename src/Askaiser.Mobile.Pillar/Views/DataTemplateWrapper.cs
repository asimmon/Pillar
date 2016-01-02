using System;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Views
{
    /// <summary>
    /// Wrapper for a DataTemplate.
    /// Unfortunately the default constructor for DataTemplate is internal
    /// so I had to wrap the DataTemplate instead of inheriting it.
    /// </summary>
    /// <typeparam name="T">The object type that this DataTemplateWrapper matches</typeparam>
    public class DataTemplateWrapper<T> : BindableObject, IDataTemplateWrapper
    {
        public static readonly BindableProperty WrappedTemplateProperty = BindableProperty.Create<DataTemplateWrapper<T>, DataTemplate>(x => x.WrappedTemplate, null);
        public static readonly BindableProperty IsDefaultProperty = BindableProperty.Create<DataTemplateWrapper<T>, bool>(x => x.IsDefault, false);

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
