using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
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

        Assert.True(ring.IsField());
        Assert.True(ring.IsAssociative());
        Assert.True(ring.IsCommutative());
        Assert.Equal(SystemBigInteger.Zero, ring.Characteristic());
        Assert.Equal(2, ring.Generators().Count);
        Assert.True(ring.Generators().Last().IsImag());
        Assert.Contains("Complex", ring.ToString(), StringComparison.Ordinal);
    }
}
