using Pillar.Behaviors;
using Xamarin.Forms;
using Xunit;

namespace Pillar.Tests.Behaviors
{
    public class BindableBehaviorFixture
    {
        [Fact]
        public void HasSameExistingBindingContextThanItsAsociatedObject()
        {
            var behavior = new BindableBehavior<Entry>();

            var bindingContext = new object();

            var entry = new Entry
            {
                BindingContext = bindingContext
            };

            entry.Behaviors.Add(behavior);

            Assert.Same(behavior.BindingContext, bindingContext);
        }

        [Fact]
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

            Assert.Same(behavior.BindingContext, newBindingContext);
        }
    }
}
