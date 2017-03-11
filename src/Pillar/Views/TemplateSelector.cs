using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Pillar
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Xamarin.Forms.BindableObject" />
    [ContentProperty("Templates")]
    public class TemplateSelector : BindableObject
    {
        public static BindableProperty TemplatesProperty = BindableProperty.Create("Templates", typeof(DataTemplateCollection), typeof(TemplateSelector), default(DataTemplateCollection), BindingMode.OneWay, null, TemplatesChanged);

        public static BindableProperty SelectorFunctionProperty = BindableProperty.Create("SelectorFunction", typeof(Func<Type, DataTemplate>), typeof(TemplateSelector));

        public static BindableProperty ExceptionOnNoMatchProperty = BindableProperty.Create("ExceptionOnNoMatch", typeof(bool), typeof(DataTemplate), true);

        /// <summary>
        /// Initialize the TemplateCollections so that each 
        /// instance gets it's own collection
        /// </summary>
        public TemplateSelector()
        {
            Templates = new DataTemplateCollection();
        }

        /// <summary>
        ///  Clears the cache when the set of templates change
        /// </summary>
        /// <param name="bo"></param>
        /// <param name="oldval"></param>
        /// <param name="newval"></param>
        public static void TemplatesChanged(BindableObject bo, object oldval, object newval)
        {
            var ts = bo as TemplateSelector;
            var oldCollection = oldval as DataTemplateCollection;
            var newCollection = newval as DataTemplateCollection;

            if (ts == null) return;
            if (oldCollection != null) oldCollection.CollectionChanged -= ts.TemplateSetChanged;
            if (newCollection != null) newCollection.CollectionChanged += ts.TemplateSetChanged;
            ts.Cache = null;
        }

        /// <summary>
        /// Clear the cache on any template set change
        /// If needed this could be optimized to care about the specific
        /// change but I doubt it would be worthwhile.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TemplateSetChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Cache = null;
        }

        /// <summary>
        /// Private cache of matched types with datatemplates
        /// The cache is reset on any change to <see cref="Templates"/>
        /// </summary>
        private Dictionary<Type, DataTemplate> Cache { get; set; }

        /// <summary>
        /// Bindable property that allows the user to 
        /// determine if a <see cref="NoDataTemplateMatchException"/> is thrown when 
        /// there is no matching template found
        /// </summary>
        public bool ExceptionOnNoMatch
        {
            get { return (bool)GetValue(ExceptionOnNoMatchProperty); }
            set { SetValue(ExceptionOnNoMatchProperty, value); }
        }

        /// <summary>
        /// The collection of DataTemplates
        /// </summary>
        public DataTemplateCollection Templates
        {
            get { return (DataTemplateCollection)GetValue(TemplatesProperty); }
            set { SetValue(TemplatesProperty, value); }
        }

        /// <summary>
        /// A user supplied function of type
        /// <code>Func<typeparamname name="Type"></typeparamname>,<typeparamname name="DataTemplate"></typeparamname></code>
        /// If this function has been supplied it is always called first in the match 
        /// process.
        /// </summary>
        public Func<Type, DataTemplate> SelectorFunction
        {
            get { return (Func<Type, DataTemplate>)GetValue(SelectorFunctionProperty); }
            set { SetValue(SelectorFunctionProperty, value); }
        }


        /// <summary>
        /// Matches a type with a datatemplate
        /// Order of matching=>
        ///     SelectorFunction, 
        ///     Cache, 
        ///     SpecificTypeMatch,
        ///     InterfaceMatch,
        ///     BaseTypeMatch 
        ///     DefaultTempalte
        /// </summary>
        /// <param name="type">Type object type that needs a datatemplate</param>
        /// <returns>The DataTemplate from the WrappedDataTemplates Collection that closest matches 
        /// the type paramater.</returns>
        /// <exception cref="NoDataTemplateMatchException"></exception>Thrown if there is no datatemplate that matches the supplied type
        public DataTemplate TemplateFor(Type type)
        {
            var typesExamined = new List<Type>();
            var template = TemplateForImpl(type, typesExamined);
            if (template == null && ExceptionOnNoMatch)
                throw new NoDataTemplateMatchException(type, typesExamined);
            return template;
        }

        /// <summary>
        /// Interal implementation of <see cref="TemplateFor"/>.
        /// </summary>
        /// <param name="type">The type to match on</param>
        /// <param name="examined">A list of all types examined during the matching process</param>
        /// <returns>A DataTemplate or null</returns>
        private DataTemplate TemplateForImpl(Type type, List<Type> examined)
        {
            if (type == null) return null;//This can happen when we recusively check base types (object.BaseType==null)
            examined.Add(type);
            Contract.Assert(Templates != null, "Templates cannot be null");

            Cache = Cache ?? new Dictionary<Type, DataTemplate>();
            DataTemplate retTemplate = null;

            //Prefer the selector function if present
            //This has been moved before the cache check so that
            //the user supplied function has an opportunity to 
            //Make a decision with more information than simply
            //the requested type (perhaps the Ux or Network states...)
            if (SelectorFunction != null) retTemplate = SelectorFunction(type);

            // The selector function is supreme, if it has
            // returned a template use it.
            if (retTemplate != null) return retTemplate;

            //Happy case we already have the type in our cache
            if (Cache.ContainsKey(type)) return Cache[type];


            //check our list
            retTemplate = Templates.Where(x => x.Type == type).Select(x => x.WrappedTemplate).FirstOrDefault();
            //Check for interfaces
            retTemplate = retTemplate ?? type.GetTypeInfo().ImplementedInterfaces.Select(x => TemplateForImpl(x, examined)).FirstOrDefault();
            //look at base types
            retTemplate = retTemplate ?? TemplateForImpl(type.GetTypeInfo().BaseType, examined);
            //If all else fails try to find a Default Template
            retTemplate = retTemplate ?? Templates.Where(x => x.IsDefault).Select(x => x.WrappedTemplate).FirstOrDefault();

            Cache[type] = retTemplate;
            return retTemplate;
        }

        /// <summary>
        /// Finds a template for the type of the passed in item (<code>item.GetType()</code>)
        /// and creates the content and sets the Binding context of the View
        /// Currently the root of the DataTemplate must be a ViewCell.
        /// </summary>
        /// <param name="item">The item to instantiate a DataTemplate for</param>
        /// <returns>a View with it's binding context set</returns>
        /// <exception cref="InvalidVisualObjectException"></exception>Thrown when the matched datatemplate inflates to an object not derived from either 
        /// <see cref="Xamarin.Forms.View"/> or <see cref="Xamarin.Forms.ViewCell"/>
        public View ViewFor(object item)
        {
            var template = TemplateFor(item.GetType());
            var content = template.CreateContent();
            if (!(content is View) && !(content is ViewCell))
                throw new InvalidVisualObjectException(content.GetType());

            var view = (content is View) ? content as View : ((ViewCell)content).View;
            view.BindingContext = item;
            return view;
        }
    }
}

