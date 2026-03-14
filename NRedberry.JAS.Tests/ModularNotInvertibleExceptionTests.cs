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

        exception.InnerException.ShouldBeSameAs(innerException);
        exception.F?.ToString().ShouldBe("6");
        exception.F1?.ToString().ShouldBe("3");
        exception.F2?.ToString().ShouldBe("2");
        exception.ToString().ShouldContain("f = 6");
    }
}
