using NRedberry.Transformations.Symmetrization;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Symmetrization;

public sealed class ComplexConjugateTransformationTests
{
    [Fact]
    public void ShouldThrowForSingletonAccess()
    {
        Assert.Throws<NotImplementedException>(() => _ = ComplexConjugateTransformation.Instance);
    }
}
