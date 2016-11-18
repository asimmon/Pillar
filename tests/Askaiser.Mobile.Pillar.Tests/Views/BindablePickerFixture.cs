using System;
using System.Collections.ObjectModel;
using Askaiser.Mobile.Pillar.Views;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace Askaiser.Mobile.Pillar.Tests.Views
{
    public class BindablePickerFixture
    {
        private BindablePicker _picker;

        private List<NamedDummyObject> _classicList;

        private ObservableCollection<NamedDummyObject> _observableItems;

        public BindablePickerFixture()
        {
            _picker = new BindablePicker();

            _classicList = new List<NamedDummyObject>
            {
                new NamedDummyObject(0, "Foo"),
                new NamedDummyObject(1, "Bar"),
                new NamedDummyObject(2, "Baz"),
                new NamedDummyObject(3, "Qux")
            };

            _observableItems = new ObservableCollection<NamedDummyObject>(_classicList);
        }

        [Fact]
        public void EnsureThatItemsAreAssignedFromNewItemsSource()
        {
            _picker.ItemsSource = _classicList;

            Assert.Equal(_picker.Items, _classicList.Select(i => i.Name));
        }

        [Fact]
        public void EnsureThatItemsAreAddedWhenObservableItemsSourceChanges()
        {
            _picker.ItemsSource = _observableItems;

            _observableItems.Add(new NamedDummyObject(5, "New item"));

            Assert.Equal(_picker.Items, _observableItems.Select(i => i.Name));
        }

        [Fact]
        public void EnsureThatItemsAreRemovedWhenObservableItemsSourceChanges()
        {
            _picker.ItemsSource = _observableItems;

            _observableItems.RemoveAt(1);

            Assert.Equal(_picker.Items, _observableItems.Select(i => i.Name));
        }

        [Fact]
        public void EnsureThatNewNullItemsSourceRemoveAllItems()
        {
            _picker.ItemsSource = _observableItems;
            _picker.ItemsSource = null;

            Assert.Equal(0, _picker.Items.Count);
        }

        [Fact]
        public void EnsureThatClearingObservableItemsSourceRemoveAllItems()
        {
            _picker.ItemsSource = _observableItems;

            _observableItems.Clear();

            Assert.Equal(0, _picker.Items.Count);
        }

        [Fact]
        public void EnsureItemsAreUpdatedWhenReplacingObjectsInObservableItemsSource()
        {
            _picker.ItemsSource = _observableItems;

            _observableItems[2] = new NamedDummyObject(2, "Lol");

            Assert.Equal(_picker.Items, _observableItems.Select(i => i.Name));
        }

        [Fact]
        public void UpdateSelectedIndexUpdatesSelectedItem()
        {
            _picker.ItemsSource = _classicList;

            _picker.SelectedIndex = 2;

            Assert.Equal(_classicList[2].Name, _picker.SelectedItem);
        }

        [Fact]
        public void AddNullItemToObservableCollectionAddsItemWithEmptyText()
        {
            _picker.ItemsSource = _observableItems;

            _observableItems.Add(null);

            Assert.Equal("", _picker.Items.LastOrDefault());
        }

        [Fact]
        public void UseDisplayMemberPathInsteadOfDefaultToString()
        {
            _picker.DisplayMemberPath = "Id";
            _picker.ItemsSource = _classicList;

            Assert.Equal(_picker.Items, _classicList.Select(i => Convert.ToString(i.Id)));
        }

        [Fact]
        public void ThrowsExceptionWhenDisplayMemberPathDoesNotExists()
        {
            _picker.DisplayMemberPath = "DummyMissingProperty";

            var ex = Assert.Throws<ArgumentException>(() => _picker.ItemsSource = _classicList);

            Assert.Equal("The property DummyMissingProperty of the type Askaiser.Mobile.Pillar.Tests.Views.NamedDummyObject does not exists", ex.Message);
        }

        [Fact]
        public void DisplayMemberPathWithNullPropertiesAddsEmptyTextItems()
        {
            _classicList = new List<NamedDummyObject>
            {
                new NamedDummyObject(0, null),
                new NamedDummyObject(1, null)
            };

            _picker.DisplayMemberPath = "Name";

            _picker.ItemsSource = _classicList;

            Assert.Equal(_picker.Items, _classicList.Select(i => ""));
        }

        [Fact]
        public void ChangingDisplayMemberPathRefreshItems()
        {
            _picker.DisplayMemberPath = "Id";

            _picker.ItemsSource = _classicList;

            _picker.DisplayMemberPath = "Name";

            Assert.Equal(_picker.Items, _classicList.Select(i => i.Name));
        }

        [Fact]
        public void UpdateSelectedItemUpdatesSelectedIndex()
        {
            _picker.ItemsSource = _observableItems;

            _picker.SelectedItem = _observableItems[2];

            Assert.Equal(2, _picker.SelectedIndex);
        } 
    }
}
