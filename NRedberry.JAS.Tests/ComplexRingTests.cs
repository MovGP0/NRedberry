using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Shouldly;
using Xunit;
using PolyComplexRing = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly.ComplexRing<NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigRational>;
using SystemBigInteger = System.Numerics.BigInteger;

namespace NRedberry.JAS.Tests;

public sealed class ComplexRingTests
{
    [Fact]
    public void ShouldExposeFactoryPropertiesAndGenerators()
    {
        PolyComplexRing ring = new(new BigRational());

        ring.IsField().ShouldBeTrue();
        ring.IsAssociative().ShouldBeTrue();
        ring.IsCommutative().ShouldBeTrue();
        ring.Characteristic().ShouldBe(SystemBigInteger.Zero);
        ring.Generators().Count.ShouldBe(2);
        ring.Generators().Last().IsImag().ShouldBeTrue();
        ring.ToString().ShouldContain("Complex", StringComparison.Ordinal);
    }
}
