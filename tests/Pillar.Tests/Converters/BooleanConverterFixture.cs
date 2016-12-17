using Pillar.Converters;
using Pillar.Tests.Mocks;
using Xunit;

namespace Pillar.Tests.Converters
{
    public class BooleanConverterFixture
    {
        private object _trueObject;
        private object _falseObject;
        private BooleanConverter<object> _converter;

        public BooleanConverterFixture()
        {
            _trueObject = "true object";
            _falseObject = "false object";

            _converter = new MockBooleanConverter(_trueObject, _falseObject);
        }

        [Fact]
        public void TestConvertTrueObject()
        {
            var convertedObject = _converter.Convert(true, null, null, null);
            Assert.Same(convertedObject, _trueObject);
        }

        [Fact]
        public void TestConvertFalseObject()
        {
            var convertedObject = _converter.Convert(false, null, null, null);
            Assert.Same(convertedObject, _falseObject);
        }

        [Fact]
        public void TestConvertBackTrueObject()
        {
            var convertedBoolean = _converter.ConvertBack(_trueObject, null, null, null);
            Assert.Equal(true, convertedBoolean);
        }

        [Fact]
        public void TestConvertBackFalseObject()
        {
            var convertedObject = _converter.ConvertBack(_falseObject, null, null, null);
            Assert.Equal(false, convertedObject);
        }
    }
}
