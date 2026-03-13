using System;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class AlgebraicNotInvertibleExceptionTests
{
    [Fact]
    public void ShouldPreserveFactorPayloadsInStateAndText()
    {
        InvalidOperationException inner = new("boom");
        AlgebraicNotInvertibleException exception = new("not invertible", inner, "f", "f1", "f2");

        Assert.Equal("f", exception.F);
        Assert.Equal("f1", exception.F1);
        Assert.Equal("f2", exception.F2);
        Assert.Same(inner, exception.InnerException);
        Assert.Contains("f = f", exception.ToString(), StringComparison.Ordinal);
        Assert.Contains("f1 = f1", exception.ToString(), StringComparison.Ordinal);
        Assert.Contains("f2 = f2", exception.ToString(), StringComparison.Ordinal);
    }
}
