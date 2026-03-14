using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Shouldly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class AlgebraicNumberTests
{
    [Fact]
    public void ShouldReduceGeneratorByDefiningPolynomial()
    {
        AlgebraicNumberRing<BigRational> ring = CreateRing();
        AlgebraicNumber<BigRational> generator = ring.GetGenerator();

        generator.Multiply(generator).Sum(ring.One).IsZero().ShouldBeTrue();
    }

    [Fact]
    public void ShouldSupportInverseAndArithmetic()
    {
        AlgebraicNumberRing<BigRational> ring = CreateRing();
        AlgebraicNumber<BigRational> value = ring.FromInteger(3);

        value.IsUnit().ShouldBeTrue();
        value.Sum(ring.FromInteger(2)).ToString().ShouldBe("5");
        value.Multiply(value).ToString().ShouldBe("9");
    }

    private static AlgebraicNumberRing<BigRational> CreateRing()
    {
        ComplexRing<BigRational> complexRing = new(new BigRational());
        return complexRing.AlgebraicRing();
    }
}
