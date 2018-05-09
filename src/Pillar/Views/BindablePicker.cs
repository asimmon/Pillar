using System;
using System.Collections;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using Xamarin.Forms;

namespace Pillar
{
    /// <summary>
    /// An extension of the Picker with new features:
    /// - ItemsSource, which can be an observable collection
    /// - DisplayMemberPath to control which property will be shown to the user
    /// </summary>
    public class BindablePicker : Picker
    {
        public static readonly new BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(IEnumerable), typeof(BindablePicker), default(IEnumerable), BindingMode.OneWay, null, ItemsSourcePropertyChanged);

        public static readonly new BindableProperty SelectedItemProperty = BindableProperty.Create("SelectedItem", typeof(object), typeof(BindablePicker), default(object), BindingMode.OneWay, null, SelectedItemPropertyChanged);

        private string _displayMemberPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="BindablePicker"/> class.
        /// </summary>
        public BindablePicker()
        {
            SelectedIndexChanged += OnSelectedIndexChanged;
        }

        /// <summary>
        /// Gets or sets the items source.
        /// </summary>
        public new IEnumerable ItemsSource
        {
            get { return (IEnumerable)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the selected item.
        /// </summary>
        public new object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets the display member path.
        /// The value of the property for each item of the item source should be unique.
        /// If not set, the ToString value of each item will be used.
        /// When the DisplayMemberPath is modified, the UI is refreshed.
        /// </summary>
        public string DisplayMemberPath
        {
            get { return _displayMemberPath; }
            set
            {
                if (!string.Equals(_displayMemberPath, value))
                {
                    _displayMemberPath = value;
                    RefreshItemsValuesAfterDisplayMemberPathChanged();
                }
            }
        }

        /// <summary>
        /// A new collection is bound
        /// </summary>
        /// <param name="bindable">The BindablePicker</param>
        /// <param name="oldValues">The old collection</param>
        /// <param name="newValues">The new collection</param>
        private static void ItemsSourcePropertyChanged(BindableObject bindable, object oldValues, object newValues)
        {
            var picker = bindable as BindablePicker;
            var oldEnumerable = oldValues as IEnumerable;
            var newEnumerable = newValues as IEnumerable;

            if (picker != null)
            {
                picker.ItemsSourceChanged(oldEnumerable, newEnumerable);
            }
        }

        /// <summary>
        /// Handler that assign a new collection to the picker.
        /// If the collection is observable, we attach (or detach) an event handler on CollectionChanged.
        /// </summary>
        /// <param name="oldValues">The old collection</param>
        /// <param name="newValues">The new collection</param>
        private void ItemsSourceChanged(IEnumerable oldValues, IEnumerable newValues)
        {
            var oldValuesObservable = oldValues as INotifyCollectionChanged;
            if (oldValuesObservable != null)
            {
                oldValuesObservable.CollectionChanged -= NewValueObservableCollectionChanged;
            }

            Items.Clear();
            AddItems(newValues);

            var newValuesObservable = newValues as INotifyCollectionChanged;
            if (newValuesObservable != null)
            {
                newValuesObservable.CollectionChanged += NewValueObservableCollectionChanged;
            }
        }

        /// <summary>
        /// Handle the modification of items of the bounded collection
        /// (addition, retrieval, move, reset, replace)
        /// </summary>
        private void NewValueObservableCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            // TODO taking care of NewStartingIndex and OldStartingIndex when adding, removing, replacing and moving values
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddItems(e.NewItems);
                    break;

                case NotifyCollectionChangedAction.Remove:
                    RemoveItems(e.OldItems);
                    break;

                case NotifyCollectionChangedAction.Reset:
                    Items.Clear();
                    break;

                case NotifyCollectionChangedAction.Replace:
                    RemoveItems(e.OldItems);
                    AddItems(e.NewItems, e.NewStartingIndex);
                    break;
            }
        }

        /// <summary>
        /// Add the new items to the picker items
        /// </summary>
        private void AddItems(IEnumerable items, int startIndex = -1)
        {
            if (items == null)
                return;

            int currentStartIndex = startIndex;
            foreach (var item in items)
            {
                var value = GetPropertyValueString(item, DisplayMemberPath);

                if (startIndex < 0)
                    Items.Add(value);
                else
                    Items.Insert(currentStartIndex, value);

                currentStartIndex++;
            }
        }

        /// <summary>
        /// Remove items from the picker items
        /// </summary>
        private void RemoveItems(IEnumerable items)
        {
            if (items == null)
                return;

            foreach (var item in items)
            {
                var value = GetPropertyValueString(item, DisplayMemberPath);
                Items.Remove(value);
            }
        }

        /// <summary>
        /// The selected index has changed, change the selected item accordingly
        /// </summary>
        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = Items[SelectedIndex];
            }
        }

        /// <summary>
        /// The selected item has changed, change the selected index accordingly
        /// </summary>
        /// <param name="bindable">The picker</param>
        /// <param name="oldValue">The old selected item</param>
        /// <param name="newValue">The new selected item</param>
        private static void SelectedItemPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var picker = bindable as BindablePicker;
            if (picker != null && newValue != null)
            {
                picker.SelectedIndex = picker.Items.IndexOf(newValue.ToString());
            }
        }

        /// <summary>
        /// Gets the string property value of an object using the property name.
        /// If the property name is missing, it will return ToString.
        /// </summary>
        private static string GetPropertyValueString(object item, string propertyName)
        {
            if (item == null)
                return "";

            if (propertyName == null)
                return item.ToString();

            var itemType = item.GetType();

            var property = itemType.GetRuntimeProperties().SingleOrDefault(p => p.Name == propertyName);
            if (property == null)
                throw new ArgumentException($"The property {propertyName} of the type {itemType.FullName} does not exists");

            // TODO cache the property for faster retrieval

            var value = property.GetValue(item, null);
            if (value == null)
                return "";

            return value.ToString();
        }

        /// <summary>
        /// Refresh the display values of the picker accordingly to the DisplayMemberPath
        /// </summary>
        private void RefreshItemsValuesAfterDisplayMemberPathChanged()
        {
            if (ItemsSource == null)
                return;

            int i = 0;

            foreach (var item in ItemsSource)
            {
                var displayValue = GetPropertyValueString(item, DisplayMemberPath);

                Items[i] = displayValue;

                i++;
            }
        }
    }
}
