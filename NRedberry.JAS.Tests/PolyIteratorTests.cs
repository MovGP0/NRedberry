using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class PolyIteratorTests
{
    [Fact]
    public void ShouldIteratePolynomialTermsAsMonomials()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        GenPolynomial<BigRational> polynomial = ring.Univariate(0).Sum(new BigRational(2));
        PolyIterator<BigRational> iterator = new(polynomial.GetMap());

        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.Exponent().GetVal().ShouldBe([1L]);
        iterator.MoveNext().ShouldBeTrue();
        iterator.Current.Exponent().GetVal().ShouldBe([0L]);
        iterator.MoveNext().ShouldBeFalse();
    }
}
