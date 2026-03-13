using NRedberry;
using NRedberry.Transformations.Reverse;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Reverse;

public sealed class SingleReverseTests
{
    [Fact]
    public void ShouldThrowWhileSingleReverseIsUnimplemented()
    {
        Assert.Throws<NotImplementedException>(() => new SingleReverse(IndexType.LatinLower));
    }
}
