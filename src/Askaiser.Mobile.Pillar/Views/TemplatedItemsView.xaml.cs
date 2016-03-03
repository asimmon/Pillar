using System;
using System.Collections;
using System.Linq;
using System.Windows.Input;
using Askaiser.Mobile.Pillar.Interfaces;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Views
{
    public partial class TemplatedItemsView : ScrollView
    {
        private readonly ICommand _selectedCommand;

        public TemplatedItemsView()
        {
            InitializeComponent();

            _selectedCommand = new Command<object>(item => SelectedItem = item);
            PropertyChanged += (sender, e) =>
            {
                if (e.PropertyName == "Orientation")
                {
                    Container.Orientation = (this.Orientation == ScrollOrientation.Horizontal)
                        ? StackOrientation.Horizontal
                        : StackOrientation.Vertical;
                }
            };
        }

        public event EventHandler SelectedItemChanged;

        public static readonly BindableProperty ItemsSourceProperty =
            BindableProperty.Create<TemplatedItemsView, IList>(p => p.ItemsSource, default(IList), BindingMode.TwoWay, null, ItemsSourceChanged);

        public IList ItemsSource
        {
            get { return (IList)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public static readonly BindableProperty SelectedItemProperty =
            BindableProperty.Create<TemplatedItemsView, object>(p => p.SelectedItem, default(object), BindingMode.TwoWay, null, OnSelectedItemChanged);

        public object SelectedItem
        {
            get { return GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly BindableProperty TemplateSelectorProperty =
            BindableProperty.Create<TemplatedItemsView, TemplateSelector>(p => p.TemplateSelector, default(TemplateSelector), BindingMode.Default, null, TemplateSelectorChanged);

        public TemplateSelector TemplateSelector
        {
            get { return (TemplateSelector)GetValue(TemplateSelectorProperty); }
            set { SetValue(TemplateSelectorProperty, value); }
        }

        private static void TemplateSelectorChanged(BindableObject bindable, TemplateSelector oldValue, TemplateSelector newValue)
        {
            var view = (TemplatedItemsView)bindable;
            view.SetItems();
        }

        private static void ItemsSourceChanged(BindableObject bindable, IList oldValue, IList newValue)
        {
            var itemsLayout = (TemplatedItemsView)bindable;
            itemsLayout.SetItems();
        }

        private void SetItems()
        {
            Container.Children.Clear();

            if (ItemsSource == null)
                return;

            foreach (var item in ItemsSource)
                Container.Children.Add(GetItemView(item));

            SelectedItem = ItemsSource.Cast<object>().FirstOrDefault();
        }

        public void OnSelected(object sender, EventArgs e)
        {
            SetSelectedItems(sender);
        }

        private View GetItemView(object item)
        {
            var view = TemplateSelector.ViewFor(item);

            view.BindingContext = item;

            var gesture = new TapGestureRecognizer
            {
                Command = _selectedCommand,
                CommandParameter = item
            };

            AddGesture(view, gesture);

            return view;
        }

        private static void AddGesture(View view, IGestureRecognizer gesture)
        {
            view.GestureRecognizers.Add(gesture);

            var layout = view as Layout<View>;

            if (layout == null)
                return;

            foreach (var child in layout.Children)
                AddGesture(child, gesture);
        }

        private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var itemsView = (TemplatedItemsView)bindable;

            if (newValue == oldValue)
                return;

            itemsView.SetSelectedItems(newValue);
        }

        private void SetSelectedItems(object selectedItem)
        {
            var items = ItemsSource.OfType<ISelectable>();

            foreach (var item in items)
                item.IsSelected = item == selectedItem;

            var handler = SelectedItemChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }
    }
}

