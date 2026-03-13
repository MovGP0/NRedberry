using System.Collections.Generic;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class HenselMultUtilTests
{
    [Fact]
    public void ShouldDelegateUnivariateLiftDiophantToHenselUtil()
    {
        GenPolynomialRing<ModLong> ring = new(
            new ModLongRing(5, true),
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
        GenPolynomial<ModLong> a = ring.Univariate(0).Sum(ring.FromInteger(1));
        GenPolynomial<ModLong> b = ring.Univariate(0).Sum(ring.FromInteger(2));
        GenPolynomial<ModLong> c = ring.Univariate(0);
        List<GenPolynomial<ModLong>> expected = HenselUtil.LiftDiophant(a, b, c, 1);

        List<GenPolynomial<ModLong>> lifted = HenselMultUtil.LiftDiophant<ModLong>(
            a,
            b,
            c,
            [((ModLongRing)ring.CoFac).FromInteger(0)],
            1,
            1);

        Assert.Equal(expected[0], lifted[0]);
        Assert.Equal(expected[1], lifted[1]);
    }
}
