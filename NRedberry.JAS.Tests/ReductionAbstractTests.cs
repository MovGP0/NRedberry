using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Gb;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class ReductionAbstractTests
{
    [Fact]
    public void ShouldProduceMonicIrreducibleSet()
    {
        GenPolynomialRing<BigRational> ring = new(new BigRational(), 1, ["x"]);
        ReductionAbstract<BigRational> reduction = new ReductionSeq<BigRational>();
        GenPolynomial<BigRational> polynomial = ring.Univariate(0).Multiply(new BigRational(2));

        var irreducible = reduction.IrreducibleSet([polynomial]);

        irreducible.ShouldHaveSingleItem();
        irreducible[0].LeadingBaseCoefficient().ToString().ShouldBe("1");
        irreducible[0].LeadingExpVector()!.GetVal().ShouldBe([1L]);
    }
}
