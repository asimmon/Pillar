using Xunit;

namespace Pillar.Tests.Services
{
    public class MessengerFixture
    {
        private Messenger _messenger;

        public MessengerFixture()
        {
            _messenger = new Messenger();
        }

        [Fact]
        public void SingleSubscriber()
        {
            string sentMessage = null;
            _messenger.Subscribe<MessengerFixture, string>(this, "SimpleTest", (sender, args) => sentMessage = args);

            _messenger.Send(this, "SimpleTest", "My Message");

            Assert.Equal("My Message", sentMessage);

            _messenger.Unsubscribe<MessengerFixture, string>(this, "SimpleTest");
        }

        [Fact]
        public void Filter()
        {
            string sentMessage = null;
            _messenger.Subscribe<MessengerFixture, string>(this, "SimpleTest", (sender, args) => sentMessage = args, this);

            _messenger.Send(new MessengerFixture(), "SimpleTest", "My Message");

            Assert.Null(sentMessage);

            _messenger.Send(this, "SimpleTest", "My Message");

            Assert.Equal("My Message", sentMessage);

            _messenger.Unsubscribe<MessengerFixture, string>(this, "SimpleTest");
        }

        [Fact]
        public void MultiSubscriber()
        {
            var sub1 = new object();
            var sub2 = new object();
            string sentMessage1 = null;
            string sentMessage2 = null;
            _messenger.Subscribe<MessengerFixture, string>(sub1, "SimpleTest", (sender, args) => sentMessage1 = args);
            _messenger.Subscribe<MessengerFixture, string>(sub2, "SimpleTest", (sender, args) => sentMessage2 = args);

            _messenger.Send(this, "SimpleTest", "My Message");

            Assert.Equal("My Message", sentMessage1);
            Assert.Equal("My Message", sentMessage2);

            _messenger.Unsubscribe<MessengerFixture, string>(sub1, "SimpleTest");
            _messenger.Unsubscribe<MessengerFixture, string>(sub2, "SimpleTest");
        }

        [Fact]
        public void Unsubscribe()
        {
            string sentMessage = null;
            _messenger.Subscribe<MessengerFixture, string>(this, "SimpleTest", (sender, args) => sentMessage = args);
            _messenger.Unsubscribe<MessengerFixture, string>(this, "SimpleTest");

            _messenger.Send(this, "SimpleTest", "My Message");

            Assert.Null(sentMessage);
        }

        [Fact]
        public void SendWithoutSubscribers()
        {
            _messenger.Send(this, "SimpleTest", "My Message");
        }

        [Fact]
        public void NoArgSingleSubscriber()
        {
            bool sentMessage = false;
            _messenger.Subscribe<MessengerFixture>(this, "SimpleTest", sender => sentMessage = true);

            _messenger.Send(this, "SimpleTest");

            Assert.True(sentMessage);

            _messenger.Unsubscribe<MessengerFixture>(this, "SimpleTest");
        }

        [Fact]
        public void UnsubscribeInCallback()
        {
            int messageCount = 0;

            var subscriber1 = new object();
            var subscriber2 = new object();

            _messenger.Subscribe<MessengerFixture>(subscriber1, "SimpleTest", sender =>
            {
                messageCount++;
                _messenger.Unsubscribe<MessengerFixture>(subscriber2, "SimpleTest");
            });

            _messenger.Subscribe<MessengerFixture>(subscriber2, "SimpleTest", sender =>
            {
                messageCount++;
                _messenger.Unsubscribe<MessengerFixture>(subscriber1, "SimpleTest");
            });

            _messenger.Send(this, "SimpleTest");

            Assert.Equal(messageCount, 1);
        }
    }
}
