﻿using System;
using System.Globalization;
using Askaiser.Mobile.Pillar.Behaviors;
using GalaSoft.MvvmLight.Command;
using Moq;
using NUnit.Framework;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Tests.Behaviors
{
    [TestFixture]
    public class EventToCommandBehaviorFixture
    {
        [Test]
        public void HandlesEventWithCommand()
        {
            bool textChanged = false;

            var behavior = new EventToCommandBehavior
            {
                EventName = "TextChanged",
                Command = new RelayCommand(() => textChanged = true)
            };

            var entry = new Entry();

            entry.Behaviors.Add(behavior);
            entry.Text = "foobar";
            entry.Behaviors.Remove(behavior);

            Assert.That(textChanged, Is.True);
        }

        [Test]
        public void HandlesEventWithCommandAndEventArgsConverter()
        {
            var converter = new Mock<IValueConverter>();
            converter.Setup(c => c.Convert(It.IsAny<object>(), It.IsAny<Type>(), It.IsAny<object>(), It.IsAny<CultureInfo>()))
                .Returns<object, Type, object, CultureInfo>((v, t, p, c) => ((TextChangedEventArgs) v).NewTextValue.Length);

            int textLength = 0;

            var behavior = new EventToCommandBehavior
            {
                EventName = "TextChanged",
                Command = new RelayCommand<int>(len => textLength = len),
                EventArgsConverter = converter.Object
            };

            var entry = new Entry();

            entry.Behaviors.Add(behavior);
            entry.Text = "foobar";
            entry.Behaviors.Remove(behavior);

            Assert.That(textLength, Is.GreaterThan(0));
        }

        [Test]
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
                Command = new RelayCommand<int>(len => textLength = len),
                EventArgsConverter = converter.Object,
                EventArgsConverterParameter = foobar
            };

            var entry = new Entry();

            entry.Behaviors.Add(behavior);
            entry.Text = foobar;
            entry.Behaviors.Remove(behavior);

            Assert.That(textLength, Is.EqualTo(foobar.Length * 2));
        }
    }
}