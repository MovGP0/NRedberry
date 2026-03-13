using NRedberry.Core.Utils;
using Xunit;

namespace NRedberry.Core.Tests.Utils;

public sealed class TensorWrapperWithEqualsTests
{
    [Fact]
    public void ShouldCompareWrappedValuesByEquality()
    {
        TensorWrapperWithEquals left = new("value");
        TensorWrapperWithEquals right = new(string.Concat("va", "lue"));
        TensorWrapperWithEquals other = new("other");

        Assert.Equal(left, right);
        Assert.NotEqual(left, other);
        Assert.Equal("value", left.GetTensor());
    }
}
