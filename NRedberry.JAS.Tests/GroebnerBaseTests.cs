using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Gb;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class GroebnerBaseTests
{
    [Fact]
    public void ShouldClassifyCommonZerosForSimpleInputs()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        GenPolynomial<BigRational> x = ring.Univariate(0);
        GenPolynomial<BigRational> constant = ring.FromInteger(1);
        GroebnerBase<BigRational> groebnerBase = new();

        groebnerBase.CommonZeroTest([]).ShouldBe(1);
        groebnerBase.CommonZeroTest([x]).ShouldBe(0);
        groebnerBase.CommonZeroTest([constant]).ShouldBe(-1);
    }
}
