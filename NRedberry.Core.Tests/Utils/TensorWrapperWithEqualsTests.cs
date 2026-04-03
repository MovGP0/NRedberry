using NRedberry.Core.Utils;

namespace NRedberry.Core.Tests.Utils;

public sealed class TensorWrapperWithEqualsTests
{
    [Fact]
    public void ShouldCompareWrappedValuesByEquality()
    {
        TensorWrapperWithEquals left = new("value");
        TensorWrapperWithEquals right = new(string.Concat("va", "lue"));
        TensorWrapperWithEquals other = new("other");

        right.ShouldBe(left);
        other.ShouldNotBe(left);
        left.GetTensor().ShouldBe("value");
    }
}
