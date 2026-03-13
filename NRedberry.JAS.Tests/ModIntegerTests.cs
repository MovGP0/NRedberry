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

        Assert.Equal("3", value.ToString());
        Assert.Equal("5", value.Inverse().ToString());
        Assert.Equal("1", value.Multiply(value.Inverse()).ToString());
    }

    [Fact]
    public void ShouldThrowDetailedExceptionForNonUnits()
    {
        ModIntegerRing ring = new(new BigInteger(6));
        ModInteger value = new(ring, 3);

        ModularNotInvertibleException exception = Assert.Throws<ModularNotInvertibleException>(() => value.Inverse());

        Assert.Equal("6", exception.F?.ToString());
        Assert.Equal("3", exception.F1?.ToString());
        Assert.Equal("2", exception.F2?.ToString());
    }
}
