using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Gb;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ReductionTests
{
    [Fact]
    public void ShouldExposeReductionContractThroughSequentialImplementation()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        GenPolynomial<BigRational> x = ring.Univariate(0);
        Reduction<BigRational> reduction = new ReductionSeq<BigRational>();

        GenPolynomial<BigRational> normalform = reduction.Normalform([x], x);

        Assert.True(normalform.IsZero());
    }
}
