using System;
using Xamarin.Forms;

namespace Pillar
{
    /// <summary>
    /// Interface to enable DataTemplateCollection to hold
    /// typesafe instances of DataTemplateWrapper
    /// </summary>
    public interface IDataTemplateWrapper
    {
        bool IsDefault { get; set; }
        DataTemplate WrappedTemplate { get; set; }
        Type Type { get; }
    }
}
