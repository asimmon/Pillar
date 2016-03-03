using Askaiser.Mobile.Pillar.Behaviors;
using NUnit.Framework;
using Xamarin.Forms;

namespace Askaiser.Mobile.Pillar.Tests.Behaviors
{
    [TestFixture]
    public class BindableBehaviorFixture
    {
        [Test]
        public void HasSameExistingBindingContextThanItsAsociatedObject()
        {
            var behavior = new BindableBehavior<Entry>();

            var bindingContext = new object();

            var entry = new Entry
            {
                BindingContext = bindingContext
            };

            entry.Behaviors.Add(behavior);

            Assert.AreSame(bindingContext, behavior.BindingContext);
        }

        [Test]
        public void GetSameBindingContextWhenAssociatedBindingContextChange()
        {
            var behavior = new BindableBehavior<Entry>();

            var oldBindingContext = new object();
            var newBindingContext = new object();

            var entry = new Entry
            {
                BindingContext = oldBindingContext
            };

            entry.Behaviors.Add(behavior);

            entry.BindingContext = newBindingContext;

            Assert.AreSame(newBindingContext, behavior.BindingContext);
        }
    }
}
