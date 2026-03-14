using NRedberry;
using NRedberry.Transformations.Reverse;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Reverse;

public sealed class ReverseTransformationTests
{
    [Fact]
    public void ShouldRejectMetricIndexTypes()
    {
        Assert.Throws<ArgumentException>(() => new ReverseTransformation(IndexType.LatinLower));
    }
}
