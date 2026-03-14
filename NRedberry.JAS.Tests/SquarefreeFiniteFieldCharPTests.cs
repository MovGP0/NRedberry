using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class SquarefreeFiniteFieldCharPTests
{
    [Fact]
    public void ShouldComputeCharacteristicRootsForCoefficientsAndPolynomials()
    {
        ModLongRing coefficientRing = new(2, true);
        SquarefreeFiniteFieldCharP<ModLong> squarefree = new(coefficientRing);
        GenPolynomialRing<ModLong> ring = new(
            coefficientRing,
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
        GenPolynomial<ModLong> polynomial = ring.Univariate(0, 2L).Sum(ring.FromInteger(1));

        GenPolynomial<ModLong>? root = squarefree.BaseSquarefreePRoot(polynomial);

        squarefree.CoeffRootCharacteristic(coefficientRing.FromInteger(1)).ShouldBe(coefficientRing.FromInteger(1));
        squarefree.RootCharacteristic(polynomial).ShouldNotBeNull();
        root.ShouldBe(ring.Univariate(0).Sum(ring.FromInteger(1)));
    }
}
