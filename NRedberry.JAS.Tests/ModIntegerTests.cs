using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ModIntegerTests
{
    [Fact]
    public void ShouldNormalizeValuesAndInvertUnits()
    {
        ModIntegerRing ring = new(new BigInteger(7));
        ModInteger value = new(ring, 10);

        value.ToString().ShouldBe("3");
        value.Inverse().ToString().ShouldBe("5");
        value.Multiply(value.Inverse()).ToString().ShouldBe("1");
    }

    [Fact]
    public void ShouldThrowDetailedExceptionForNonUnits()
    {
        ModIntegerRing ring = new(new BigInteger(6));
        ModInteger value = new(ring, 3);

        ModularNotInvertibleException exception = Should.Throw<ModularNotInvertibleException>(() => value.Inverse());

        exception.F?.ToString().ShouldBe("6");
        exception.F1?.ToString().ShouldBe("3");
        exception.F2?.ToString().ShouldBe("2");
    }
}
