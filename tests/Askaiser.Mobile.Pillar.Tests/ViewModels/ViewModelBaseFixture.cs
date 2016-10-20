using System;
using System.ComponentModel;
using Askaiser.Mobile.Pillar.Tests.Mocks;
using NUnit.Framework;

namespace Askaiser.Mobile.Pillar.Tests.ViewModels
{
    [TestFixture]
    public class ViewModelBaseFixture
    {
        private MockViewModel _viewModel;

        [SetUp]
        public void RunBeforeAnyTests()
        {
            _viewModel = new MockViewModel();
        }

        [Test]
        public void SetPropertyWithLambdaPropertyName()
        {
            var handler = new PropertyChangedEventHandler((sender, args) =>
            {
                Assert.That(sender, Is.EqualTo(_viewModel));
                Assert.That(args, Is.Not.Null);

                Assert.That(args.PropertyName, Is.EqualTo("LambdaStringProperty"));
            });

            _viewModel.PropertyChanged += handler;
            _viewModel.LambdaStringProperty = "Bar";

            Assert.That(_viewModel.LambdaStringProperty, Is.EqualTo("Bar"));

            _viewModel.PropertyChanged -= handler;
        }

        [Test]
        public void SetPropertyWitoutLambdaPropertyName()
        {
            var handler = new PropertyChangedEventHandler((sender, args) =>
            {
                Assert.That(sender, Is.EqualTo(_viewModel));
                Assert.That(args, Is.Not.Null);

                Assert.That(args.PropertyName, Is.EqualTo("WithoutLambdaStringProperty"));
            });

            _viewModel.PropertyChanged += handler;
            _viewModel.WithoutLambdaStringProperty = "Bar";

            Assert.That(_viewModel.WithoutLambdaStringProperty, Is.EqualTo("Bar"));

            _viewModel.PropertyChanged -= handler;
        }

        [Test]
        public void SetPropertyWitoutLambdaPropertyNameSameValueTwiceChangeNothing()
        {
            var changes = 0;

            var handler = new PropertyChangedEventHandler((sender, args) =>
            {
                changes++;
            });

            _viewModel.PropertyChanged += handler;
            _viewModel.WithoutLambdaStringProperty = "Bar";
            _viewModel.WithoutLambdaStringProperty = "Bar";

            Assert.That(changes, Is.EqualTo(1));

            _viewModel.PropertyChanged -= handler;
        }

        [Test]
        public void SetPropertyByLambdaSameValueTwiceChangeNothing()
        {
            var changes = 0;

            var handler = new PropertyChangedEventHandler((sender, args) =>
            {
                changes++;
            });

            _viewModel.PropertyChanged += handler;
            _viewModel.LambdaStringProperty = "Bar";
            _viewModel.LambdaStringProperty = "Bar"; // same value again

            Assert.That(changes, Is.EqualTo(1));

            _viewModel.PropertyChanged -= handler;
        }

        [Test]
        public void TestThrowsArgumentExceptionWhenLambdaPropertyEmpty()
        {
            var viewModel = new MockViewModel();

            var emptyHandler = new PropertyChangedEventHandler((sender, args) => { });
            viewModel.PropertyChanged += emptyHandler;

            Assert.Throws<ArgumentException>(() => viewModel.EmptyLambdaStringProperty = "Bar");

            viewModel.PropertyChanged -= emptyHandler;
        }

        [Test]
        public void TestThrowsArgumentNullExceptionWhenLambdaPropertyNull()
        {
            var viewModel = new MockViewModel();

            var emptyHandler = new PropertyChangedEventHandler((sender, args) => { });
            viewModel.PropertyChanged += emptyHandler;

            Assert.Throws<ArgumentNullException>(() => viewModel.NullLambdaStringProperty = "Bar");

            viewModel.PropertyChanged -= emptyHandler;
        }

        [Test]
        public void TestThrowsArgumentNullExceptionWhenLambdaPropertyNotAProperty()
        {
            var viewModel = new MockViewModel();

            var emptyHandler = new PropertyChangedEventHandler((sender, args) => { });
            viewModel.PropertyChanged += emptyHandler;

            Assert.Throws<ArgumentException>(() => viewModel.NotALambdaProperty = "Bar");

            viewModel.PropertyChanged -= emptyHandler;
        }
    }
}
