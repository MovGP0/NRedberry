using Xunit;

namespace NRedberry.Core.Tests;

public sealed class ICloneableTests
{
    [Fact]
    public void ShouldReturnTypedClone()
    {
        Sample original = new(42);

        Sample clone = original.Clone();

        Assert.NotSame(original, clone);
        Assert.Equal(42, clone.Value);
    }

    [Fact]
    public void ShouldSupportNonGenericClone()
    {
        Sample original = new(7);

        var clone = (Sample)((ICloneable)original).Clone();

        Assert.NotSame(original, clone);
        Assert.Equal(7, clone.Value);
    }

    private sealed class Sample : ICloneable<Sample>
    {
        public int Value { get; }

        public Sample(int value)
        {
            Value = value;
        }

        public Sample Clone()
        {
            return new Sample(Value);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
