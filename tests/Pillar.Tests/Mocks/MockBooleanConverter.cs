namespace Pillar.Tests.Mocks
{
    public class MockBooleanConverter : BooleanConverter<object>
    {
        public MockBooleanConverter(object trueObjectValue, object falseObjectValue)
            : base(trueObjectValue, falseObjectValue)
        { }
    }
}
