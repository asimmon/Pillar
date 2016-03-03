using System;
using System.Collections.ObjectModel;
using Askaiser.Mobile.Pillar.Views;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Askaiser.Mobile.Pillar.Tests.Views
{
    [TestFixture]
    public class BindablePickerFixture
    {
        private BindablePicker _picker;

        private List<NamedDummyObject> _classicList;

        private ObservableCollection<NamedDummyObject> _observableItems;

        [SetUp]
        public void RunBeforeAnyTests()
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

        [Test]
        public void EnsureThatItemsAreAssignedFromNewItemsSource()
        {
            _picker.ItemsSource = _classicList;

            CollectionAssert.AreEqual(_classicList.Select(i => i.Name), _picker.Items);
        }

        [Test]
        public void EnsureThatItemsAreAddedWhenObservableItemsSourceChanges()
        {
            _picker.ItemsSource = _observableItems;

            _observableItems.Add(new NamedDummyObject(5, "New item"));

            CollectionAssert.AreEqual(_observableItems.Select(i => i.Name), _picker.Items);
        }

        [Test]
        public void EnsureThatItemsAreRemovedWhenObservableItemsSourceChanges()
        {
            _picker.ItemsSource = _observableItems;

            _observableItems.RemoveAt(1);

            CollectionAssert.AreEqual(_observableItems.Select(i => i.Name), _picker.Items);
        }

        [Test]
        public void EnsureThatNewNullItemsSourceRemoveAllItems()
        {
            _picker.ItemsSource = _observableItems;
            _picker.ItemsSource = null;

            Assert.That(_picker.Items.Count, Is.EqualTo(0));
        }

        [Test]
        public void EnsureThatClearingObservableItemsSourceRemoveAllItems()
        {
            _picker.ItemsSource = _observableItems;

            _observableItems.Clear();

            Assert.That(_picker.Items.Count, Is.EqualTo(0));
        }

        [Test]
        public void EnsureItemsAreUpdatedWhenReplacingObjectsInObservableItemsSource()
        {
            _picker.ItemsSource = _observableItems;

            _observableItems[2] = new NamedDummyObject(2, "Lol");

            CollectionAssert.AreEqual(_observableItems.Select(i => i.Name), _picker.Items);
        }

        [Test]
        public void UpdateSelectedIndexUpdatesSelectedItem()
        {
            _picker.ItemsSource = _classicList;

            _picker.SelectedIndex = 2;

            Assert.AreEqual(_classicList[2].Name, _picker.SelectedItem);
        }

        [Test]
        public void AddNullItemToObservableCollectionAddsItemWithEmptyText()
        {
            _picker.ItemsSource = _observableItems;

            _observableItems.Add(null);

            Assert.AreEqual("", _picker.Items.LastOrDefault());
        }

        [Test]
        public void UseDisplayMemberPathInsteadOfDefaultToString()
        {
            _picker.DisplayMemberPath = "Id";
            _picker.ItemsSource = _classicList;

            CollectionAssert.AreEqual(_classicList.Select(i => Convert.ToString(i.Id)), _picker.Items);
        }

        [Test]
        public void ThrowsExceptionWhenDisplayMemberPathDoesNotExists()
        {
            _picker.DisplayMemberPath = "DummyMissingProperty";

            var ex = Assert.Throws<ArgumentException>(() => _picker.ItemsSource = _classicList);

            Assert.That(ex.Message, Is.EqualTo("The property DummyMissingProperty of the type Askaiser.Mobile.Pillar.Tests.Views.NamedDummyObject does not exists"));
        }

        [Test]
        public void DisplayMemberPathWithNullPropertiesAddsEmptyTextItems()
        {
            _classicList = new List<NamedDummyObject>
            {
                new NamedDummyObject(0, null),
                new NamedDummyObject(1, null)
            };

            _picker.DisplayMemberPath = "Name";

            _picker.ItemsSource = _classicList;

            CollectionAssert.AreEqual(_classicList.Select(i => ""), _picker.Items);
        }

        [Test]
        public void ChangingDisplayMemberPathRefreshItems()
        {
            _picker.DisplayMemberPath = "Id";

            _picker.ItemsSource = _classicList;

            _picker.DisplayMemberPath = "Name";

            CollectionAssert.AreEqual(_classicList.Select(i => i.Name), _picker.Items);
        }

        [Test]
        public void UpdateSelectedItemUpdatesSelectedIndex()
        {
            _picker.ItemsSource = _observableItems;

            _picker.SelectedItem = _observableItems[2];

            Assert.AreEqual(2, _picker.SelectedIndex);
        } 
    }
}
