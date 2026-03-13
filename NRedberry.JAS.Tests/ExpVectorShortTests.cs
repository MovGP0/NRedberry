using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ExpVectorShortTests
{
    [Fact]
    public void ShouldSupportSubtractionAndDependencies()
    {
        ExpVectorShort vector = new([1L, 0L, 2L]);
        ExpVector difference = vector.Subtract(new ExpVectorShort([1L, 0L, 1L]));

        Assert.Equal([0L, 0L, 1L], difference.GetVal());
        Assert.Equal([0, 2], vector.DependencyOnVariables());
    }
}
