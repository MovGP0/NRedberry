using System;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ModularNotInvertibleExceptionTests
{
    [Fact]
    public void ShouldPreserveInnerExceptionAndFactors()
    {
        InvalidOperationException innerException = new("boom");
        ModularNotInvertibleException exception = new(innerException, 6, 3, 2);

        Assert.Same(innerException, exception.InnerException);
        Assert.Equal("6", exception.F?.ToString());
        Assert.Equal("3", exception.F1?.ToString());
        Assert.Equal("2", exception.F2?.ToString());
        Assert.Contains("f = 6", exception.ToString(), StringComparison.Ordinal);
    }
}
