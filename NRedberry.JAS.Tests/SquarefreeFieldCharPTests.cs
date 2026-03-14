using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class SquarefreeFieldCharPTests
{
    [Fact]
    public void ShouldComputeBaseSquarefreePartAndCoefficientFactorsInPositiveCharacteristic()
    {
        ModLongRing coefficientRing = new(2, true);
        TestSquarefreeFieldCharP squarefree = new(coefficientRing);
        GenPolynomialRing<ModLong> ring = CreateRing(coefficientRing);
        GenPolynomial<ModLong> polynomial = ring.Univariate(0, 2L).Sum(ring.FromInteger(1));

        GenPolynomial<ModLong> part = squarefree.BaseSquarefreePart(polynomial);
        SortedDictionary<ModLong, long> coefficientFactors = squarefree.SquarefreeFactors(coefficientRing.FromInteger(1));

        Assert.Equal(ring.Univariate(0).Sum(ring.FromInteger(1)), part);
        Assert.Empty(coefficientFactors);
    }

    private static GenPolynomialRing<ModLong> CreateRing(ModLongRing coefficientRing)
    {
        return new GenPolynomialRing<ModLong>(
            coefficientRing,
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
    }
}

file sealed class TestSquarefreeFieldCharP(ModLongRing fac) : SquarefreeFieldCharP<ModLong>(fac)
{
    private readonly SquarefreeFiniteFieldCharP<ModLong> _delegate = new(fac);

    public override GenPolynomial<ModLong>? BaseSquarefreePRoot(GenPolynomial<ModLong> polynomial)
    {
        return _delegate.BaseSquarefreePRoot(polynomial);
    }

    public override GenPolynomial<GenPolynomial<ModLong>>? RecursiveUnivariateRootCharacteristic(
        GenPolynomial<GenPolynomial<ModLong>> polynomial)
    {
        return _delegate.RecursiveUnivariateRootCharacteristic(polynomial);
    }
}
