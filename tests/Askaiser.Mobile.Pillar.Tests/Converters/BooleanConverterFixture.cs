using Askaiser.Mobile.Pillar.Converters;
using Askaiser.Mobile.Pillar.Tests.Mocks;
using NUnit.Framework;

namespace Askaiser.Mobile.Pillar.Tests.Converters
{
    [TestFixture]
    public class BooleanConverterFixture
    {
        private object _trueObject;
        private object _falseObject;
        private BooleanConverter<object> _converter;

        [SetUp]
        public void RunBeforeAnyTests()
        {
            _trueObject = "true object";
            _falseObject = "false object";

            _converter = new MockBooleanConverter(_trueObject, _falseObject);
        }

        [Test]
        public void TestConvertTrueObject()
        {
            var convertedObject = _converter.Convert(true, null, null, null);
            Assert.AreSame(_trueObject, convertedObject);
        }

        [Test]
        public void TestConvertFalseObject()
        {
            var convertedObject = _converter.Convert(false, null, null, null);
            Assert.AreSame(_falseObject, convertedObject);
        }

        [Test]
        public void TestConvertBackTrueObject()
        {
            var convertedBoolean = _converter.ConvertBack(_trueObject, null, null, null);
            Assert.AreEqual(true, convertedBoolean);
        }

        [Test]
        public void TestConvertBackFalseObject()
        {
            var convertedObject = _converter.ConvertBack(_falseObject, null, null, null);
            Assert.AreEqual(false, convertedObject);
        }
    }
}
