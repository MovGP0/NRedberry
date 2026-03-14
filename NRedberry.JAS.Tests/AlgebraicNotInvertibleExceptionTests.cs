using System;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Shouldly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class AlgebraicNotInvertibleExceptionTests
{
    [Fact]
    public void ShouldPreserveFactorPayloadsInStateAndText()
    {
        InvalidOperationException inner = new("boom");
        AlgebraicNotInvertibleException exception = new("not invertible", inner, "f", "f1", "f2");

        exception.F.ShouldBe("f");
        exception.F1.ShouldBe("f1");
        exception.F2.ShouldBe("f2");
        exception.InnerException.ShouldBeSameAs(inner);
        exception.ToString().ShouldContain("f = f", StringComparison.Ordinal);
        exception.ToString().ShouldContain("f1 = f1", StringComparison.Ordinal);
        exception.ToString().ShouldContain("f2 = f2", StringComparison.Ordinal);
    }
}
