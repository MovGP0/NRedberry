using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Gb;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ReductionSeqTests
{
    [Fact]
    public void ShouldReducePolynomialToNormalForm()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        GenPolynomial<BigRational> x = ring.Univariate(0);
        GenPolynomial<BigRational> dividend = x.Multiply(x).Sum(x);
        ReductionSeq<BigRational> reduction = new();

        GenPolynomial<BigRational> normalform = reduction.Normalform([x], dividend);

        Assert.True(normalform.IsZero());
    }
}
