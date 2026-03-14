using System.Collections.Generic;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;
using JasBigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.JAS.Tests;

public sealed class HenselUtilTests
{
    [Fact]
    public void ShouldVerifyDiophantLiftsForBinaryAndListForms()
    {
        GenPolynomialRing<ModLong> ring = CreateModularRing();
        GenPolynomial<ModLong> a = ring.Univariate(0).Sum(ring.FromInteger(1));
        GenPolynomial<ModLong> b = ring.Univariate(0).Sum(ring.FromInteger(2));
        GenPolynomial<ModLong> one = ring.FromInteger(1);
        List<GenPolynomial<ModLong>> factors = [a, b];
        List<GenPolynomial<ModLong>> coefficients = [ring.Zero, one];

        HenselUtil.IsDiophantLift(a, b, one, ring.Zero, a).ShouldBeTrue();
        HenselUtil.IsDiophantLift(factors, coefficients, a).ShouldBeTrue();
    }

    [Fact]
    public void ShouldLiftExtendedEuclideanAndDiophantRelations()
    {
        GenPolynomialRing<ModLong> ring = CreateModularRing();
        GenPolynomial<ModLong> a = ring.Univariate(0).Sum(ring.FromInteger(1));
        GenPolynomial<ModLong> b = ring.Univariate(0).Sum(ring.FromInteger(2));
        GenPolynomial<ModLong> c = ring.Univariate(0);
        GenPolynomial<ModLong>[] bezout = HenselUtil.LiftExtendedEuclidean(a, b, 1);
        List<GenPolynomial<ModLong>> liftedList = HenselUtil.LiftExtendedEuclidean([a, b], 1);
        List<GenPolynomial<ModLong>> diophant = HenselUtil.LiftDiophant(a, b, c, 1);
        List<GenPolynomial<ModLong>> diophantList = HenselUtil.LiftDiophant([a, b], c, 1);

        bezout.Length.ShouldBe(2);
        liftedList.Count.ShouldBe(2);
        diophant.Count.ShouldBe(2);
        diophantList.Count.ShouldBe(2);
        Should.Throw<ArgumentException>(() => HenselUtil.LiftExtendedEuclidean(ring.Zero, b, 1));
    }

    [Fact]
    public void ShouldLiftSingleMonicFactorAndReturnTrivialQuadraticApproximationForZero()
    {
        GenPolynomialRing<JasBigInteger> integerRing = CreateIntegerRing();
        GenPolynomialRing<ModLong> modularRing = CreateModularRing();
        GenPolynomial<JasBigInteger> integerFactor = integerRing.Univariate(0).Sum(integerRing.FromInteger(1));
        GenPolynomial<ModLong> modularFactor = modularRing.Univariate(0).Sum(modularRing.FromInteger(1));
        List<GenPolynomial<ModLong>> lifted = HenselUtil.LiftHenselMonic(integerFactor, [modularFactor], 2);
        HenselApprox<ModLong> directApprox = HenselUtil.LiftHenselQuadratic(
            integerRing.Zero,
            new JasBigInteger(2),
            modularFactor,
            modularRing.Univariate(0).Sum(modularRing.FromInteger(2)),
            modularRing.FromInteger(1),
            modularRing.Zero);
        HenselApprox<ModLong> inferredApprox = HenselUtil.LiftHenselQuadratic(
            integerRing.Zero,
            new JasBigInteger(2),
            modularFactor,
            modularRing.Univariate(0).Sum(modularRing.FromInteger(2)));

        lifted.ShouldHaveSingleItem();
        lifted[0].ShouldBe(modularFactor);
        directApprox.A.IsZero().ShouldBeTrue();
        directApprox.B.IsZero().ShouldBeTrue();
        inferredApprox.A.IsZero().ShouldBeTrue();
        inferredApprox.B.IsZero().ShouldBeTrue();
    }

    private static GenPolynomialRing<ModLong> CreateModularRing()
    {
        return new GenPolynomialRing<ModLong>(
            new ModLongRing(5, true),
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
    }

    private static GenPolynomialRing<JasBigInteger> CreateIntegerRing()
    {
        return new GenPolynomialRing<JasBigInteger>(
            new JasBigInteger(),
            1,
            new TermOrder(TermOrder.INVLEX),
            ["x"]);
    }
}
