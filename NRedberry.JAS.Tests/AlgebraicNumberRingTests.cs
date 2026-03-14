using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Shouldly;
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

        ring.IsField().ShouldBeTrue();
        ring.GetField().ShouldBe(1);
        ring.Characteristic().ShouldBe(SystemBigInteger.Zero);
        ring.Depth().ShouldBe(1);
        ring.TotalExtensionDegree().ShouldBe(2L);
        ring.Zero.IsZero().ShouldBeTrue();
        ring.One.IsOne().ShouldBeTrue();
        generator.Multiply(generator).Sum(ring.FromInteger(1)).IsZero().ShouldBeTrue();
    }

    [Fact]
    public void ShouldCreateAlgebraicNumbersFromIntegersAndSupportInversion()
    {
        var ring = new PolyComplexRing(new BigRational()).AlgebraicRing();
        var generator = ring.GetGenerator();
        var embedded = ring.FillFromInteger(5);

        embedded.ShouldBe(ring.FromInteger(5));
        generator.IsUnit().ShouldBeTrue();
        generator.Clone().ToString().ShouldBe(generator.ToString());
    }
}
