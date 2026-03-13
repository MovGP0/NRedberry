using NRedberry;
using NRedberry.Transformations.Reverse;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Reverse;

public sealed class ReverseTransformationTests
{
    [Fact]
    public void ShouldThrowWhileReversePortIsUnimplemented()
    {
        Assert.Throws<NotImplementedException>(() => new ReverseTransformation(IndexType.LatinLower));
    }
}
