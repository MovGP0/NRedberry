using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Xunit;
using PolyComplexRing = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly.ComplexRing<NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigRational>;
using SystemBigInteger = System.Numerics.BigInteger;

namespace NRedberry.JAS.Tests;

public sealed class AlgebraicNumberRingTests
{
    [Fact]
    public void ShouldExposeExpectedExtensionRingProperties()
    {
        PolyComplexRing complexRing = new(new BigRational());
        var ring = complexRing.AlgebraicRing();
        var generator = ring.GetGenerator();

        Assert.True(ring.IsField());
        Assert.Equal(1, ring.GetField());
        Assert.Equal(SystemBigInteger.Zero, ring.Characteristic());
        Assert.Equal(1, ring.Depth());
        Assert.Equal(2L, ring.TotalExtensionDegree());
        Assert.True(ring.Zero.IsZero());
        Assert.True(ring.One.IsOne());
        Assert.True(generator.Multiply(generator).Sum(ring.FromInteger(1)).IsZero());
    }

    [Fact]
    public void ShouldCreateAlgebraicNumbersFromIntegersAndSupportInversion()
    {
        var ring = new PolyComplexRing(new BigRational()).AlgebraicRing();
        var generator = ring.GetGenerator();
        var embedded = ring.FillFromInteger(5);

        Assert.Equal(ring.FromInteger(5), embedded);
        Assert.True(generator.IsUnit());
        Assert.Equal(generator.ToString(), generator.Clone().ToString());
    }
}
