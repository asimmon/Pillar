using System;
using System.ComponentModel;
using Pillar.Tests.Mocks;
using Xunit;

namespace Pillar.Tests.ViewModels
{
    public class ViewModelBaseFixture
    {
        private MockViewModel _viewModel;

        public ViewModelBaseFixture()
        {
            _viewModel = new MockViewModel();
        }

        [Fact]
        public void SetPropertyWithLambdaPropertyName()
        {
            var handler = new PropertyChangedEventHandler((sender, args) =>
            {
                Assert.Equal(_viewModel, sender);
                Assert.NotNull(args);

                Assert.Equal("LambdaStringProperty", args.PropertyName);
            });

            _viewModel.PropertyChanged += handler;
            _viewModel.LambdaStringProperty = "Bar";

            Assert.Equal("Bar", _viewModel.LambdaStringProperty);

            _viewModel.PropertyChanged -= handler;
        }

        [Fact]
        public void SetPropertyWitoutLambdaPropertyName()
        {
            var handler = new PropertyChangedEventHandler((sender, args) =>
            {
                Assert.Equal(_viewModel, sender);
                Assert.NotNull(args);

                Assert.Equal("WithoutLambdaStringProperty", args.PropertyName);
            });

            _viewModel.PropertyChanged += handler;
            _viewModel.WithoutLambdaStringProperty = "Bar";

            Assert.Equal("Bar", _viewModel.WithoutLambdaStringProperty);

            _viewModel.PropertyChanged -= handler;
        }

        [Fact]
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

            Assert.Equal(1, changes);

            _viewModel.PropertyChanged -= handler;
        }

        [Fact]
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

            Assert.Equal(1, changes);

            _viewModel.PropertyChanged -= handler;
        }

        [Fact]
        public void TestThrowsArgumentExceptionWhenLambdaPropertyEmpty()
        {
            var viewModel = new MockViewModel();

            var emptyHandler = new PropertyChangedEventHandler((sender, args) => { });
            viewModel.PropertyChanged += emptyHandler;

            Assert.Throws<ArgumentException>(() => viewModel.EmptyLambdaStringProperty = "Bar");

            viewModel.PropertyChanged -= emptyHandler;
        }

        [Fact]
        public void TestThrowsArgumentNullExceptionWhenLambdaPropertyNull()
        {
            var viewModel = new MockViewModel();

            var emptyHandler = new PropertyChangedEventHandler((sender, args) => { });
            viewModel.PropertyChanged += emptyHandler;

            Assert.Throws<ArgumentNullException>(() => viewModel.NullLambdaStringProperty = "Bar");

            viewModel.PropertyChanged -= emptyHandler;
        }

        [Fact]
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
