using Shouldly;
using Xunit;

namespace NRedberry.Core.Tests;

public sealed class ICloneableTests
{
    [Fact]
    public void ShouldReturnTypedClone()
    {
        Sample original = new(42);

        Sample clone = original.Clone();

        clone.ShouldNotBeSameAs(original);
        clone.Value.ShouldBe(42);
    }

    [Fact]
    public void ShouldSupportNonGenericClone()
    {
        Sample original = new(7);

        var clone = (Sample)((ICloneable)original).Clone();

        clone.ShouldNotBeSameAs(original);
        clone.Value.ShouldBe(7);
    }

    private sealed class Sample(int value) : ICloneable<Sample>
    {
        public int Value { get; } = value;

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
