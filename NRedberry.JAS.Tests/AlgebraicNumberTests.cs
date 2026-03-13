using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class AlgebraicNumberTests
{
    [Fact]
    public void ShouldReduceGeneratorByDefiningPolynomial()
    {
        AlgebraicNumberRing<BigRational> ring = CreateRing();
        AlgebraicNumber<BigRational> generator = ring.GetGenerator();

        Assert.True(generator.Multiply(generator).Sum(ring.One).IsZero());
    }

    [Fact]
    public void ShouldSupportInverseAndArithmetic()
    {
        AlgebraicNumberRing<BigRational> ring = CreateRing();
        AlgebraicNumber<BigRational> value = ring.FromInteger(3);

        Assert.True(value.IsUnit());
        Assert.Equal("5", value.Sum(ring.FromInteger(2)).ToString());
        Assert.Equal("9", value.Multiply(value).ToString());
    }

    private static AlgebraicNumberRing<BigRational> CreateRing()
    {
        ComplexRing<BigRational> complexRing = new(new BigRational());
        return complexRing.AlgebraicRing();
    }
}
