using System;
using System.Globalization;
using Askaiser.Mobile.Pillar.Behaviors;
using Moq;
using Xunit;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Tests.Behaviors
{
    public class EventToCommandBehaviorFixture
    {
        [Fact]
        public void EnsureThatEventNameIsMandatory()
        {
            var behavior = new EventToCommandBehavior
            {
                EventName = null
            };

            var entry = new Entry();

            var ex = Assert.Throws<ArgumentException>(() => entry.Behaviors.Add(behavior));
            
            Assert.Equal("EventToCommand: EventName must be specified", ex.Message);
        }

        [Fact]
        public void EnsureThatEventExists()
        {
            var behavior = new EventToCommandBehavior
            {
                EventName = "EventThatDoesNotExists"
            };

            var entry = new Entry();

            var ex = Assert.Throws<ArgumentException>(() => entry.Behaviors.Add(behavior));

            Assert.Equal("EventToCommand: Cannot find any event named 'EventThatDoesNotExists' on attached type", ex.Message);
        }

        [Fact]
        public void EnsureThatDoNothingWhenCommandIsNull()
        {
            var behavior = new EventToCommandBehavior
            {
                EventName = "TextChanged",
                Command = null
            };

            var entry = new Entry();
            entry.Behaviors.Add(behavior);
            entry.Text = "foobar";
        }

        [Fact]
        public void HandlesEventWithCommand()
        {
            bool textChanged = false;

            var behavior = new EventToCommandBehavior
            {
                EventName = "TextChanged",
                Command = new Command(() => textChanged = true)
            };

            var entry = new Entry();

            entry.Behaviors.Add(behavior);
            entry.Text = "foobar";
            entry.Behaviors.Remove(behavior);

            Assert.True(textChanged);
        }

        [Fact]
        public void HandlesEventWithCommandAndEventArgsConverter()
        {
            var converter = new Mock<IValueConverter>();
            converter.Setup(c => c.Convert(It.IsAny<object>(), It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<CultureInfo>()))
                .Returns<object, Type, object, CultureInfo>((v, t, p, c) => ((TextChangedEventArgs) v).NewTextValue.Length);

            int textLength = 0;

            var behavior = new EventToCommandBehavior
            {
                EventName = "TextChanged",
                Command = new Command<int>(len => textLength = len),
                EventArgsConverter = converter.Object
            };

            var entry = new Entry();

            entry.Behaviors.Add(behavior);
            entry.Text = "foobar";
            entry.Behaviors.Remove(behavior);

            Assert.True(textLength > 0);
        }

        [Fact]
        public void HandlesEventWithCommandAndEventArgsConverterAndEventArgsConverterParameter()
        {
            const string foobar = "foobar";

            var converter = new Mock<IValueConverter>();
            converter.Setup(c => c.Convert(It.IsAny<object>(), It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<CultureInfo>()))
                .Returns<object, Type, object, CultureInfo>((v, t, p, c) =>
                {
                    return ((TextChangedEventArgs)v).NewTextValue.Length + p.ToString().Length;
                });

            int textLength = 0;

            var behavior = new EventToCommandBehavior
            {
                EventName = "TextChanged",
                Command = new Command<int>(len => textLength = len),
                EventArgsConverter = converter.Object,
                EventArgsConverterParameter = foobar
            };

            var entry = new Entry();

            entry.Behaviors.Add(behavior);
            entry.Text = foobar;
            entry.Behaviors.Remove(behavior);

            Assert.Equal(foobar.Length * 2, textLength);
        }

        [Fact]
        public void SpecifyCommandParameterManuallyOverridesEventArgs()
        {
            object result = null;

            var behavior = new EventToCommandBehavior
            {
                EventName = "TextChanged",
                Command = new Command<object>(parameter => result = parameter),
                CommandParameter = "dummy value that will be set to the return variable"
            };

            var entry = new Entry();

            entry.Behaviors.Add(behavior);
            entry.Text = "foobar";

            Assert.Same(result, behavior.CommandParameter);
        }
    }
}
