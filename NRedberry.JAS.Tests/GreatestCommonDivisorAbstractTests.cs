using System.Collections.Generic;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ufd;
using Xunit;
using JasBigInteger = NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith.BigInteger;

namespace NRedberry.JAS.Tests;

public sealed class GreatestCommonDivisorAbstractTests
{
    [Fact]
    public void ShouldComputeBaseContentAndPrimitivePart()
    {
        StubGreatestCommonDivisorAbstractInteger gcd = new();
        GenPolynomialRing<JasBigInteger> ring = CreateIntegerRing();
        GenPolynomial<JasBigInteger> polynomial = ring.Univariate(0)
            .Sum(ring.FromInteger(1))
            .Multiply(new JasBigInteger(2));

        JasBigInteger content = gcd.BaseContent(polynomial);
        GenPolynomial<JasBigInteger> primitivePart = gcd.BasePrimitivePart(polynomial);

        content.ShouldBe(new JasBigInteger(2));
        primitivePart.ShouldBe(ring.Univariate(0).Sum(ring.FromInteger(1)));
    }

    [Fact]
    public void ShouldComputeListGcdLcmAndUseConcreteTypeName()
    {
        StubGreatestCommonDivisorAbstract gcd = new();
        GenPolynomialRing<BigRational> ring = CreateRing();
        GenPolynomial<BigRational> first = ring.Univariate(0, 2L).Subtract(ring.FromInteger(1));
        GenPolynomial<BigRational> second = ring.Univariate(0).Subtract(ring.FromInteger(1));

        GenPolynomial<BigRational> listGcd = gcd.Gcd([first, second]);
        GenPolynomial<BigRational> lcm = gcd.Lcm(first, second);

        listGcd.ShouldBe(second);
        lcm.ShouldBe(first);
        gcd.ToString().ShouldContain(nameof(StubGreatestCommonDivisorAbstract));
    }

    [Fact]
    public void ShouldThrowForUnsupportedBaseResultant()
    {
        StubGreatestCommonDivisorAbstract gcd = new();
        GenPolynomialRing<BigRational> ring = CreateRing();

        Should.Throw<NotSupportedException>(() => gcd.BaseResultant(ring.Univariate(0), ring.FromInteger(1)));
    }

    private static GenPolynomialRing<BigRational> CreateRing()
    {
        return new GenPolynomialRing<BigRational>(
            new BigRational(),
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

file sealed class StubGreatestCommonDivisorAbstract : GreatestCommonDivisorAbstract<BigRational>
{
    private readonly GreatestCommonDivisorSimple<BigRational> _delegate = new();

    public override GenPolynomial<BigRational> BaseGcd(GenPolynomial<BigRational> first, GenPolynomial<BigRational> second)
    {
        return _delegate.BaseGcd(first, second);
    }

    public override GenPolynomial<GenPolynomial<BigRational>> RecursiveUnivariateGcd(
        GenPolynomial<GenPolynomial<BigRational>> first,
        GenPolynomial<GenPolynomial<BigRational>> second)
    {
        return _delegate.RecursiveUnivariateGcd(first, second);
    }
}

file sealed class StubGreatestCommonDivisorAbstractInteger : GreatestCommonDivisorAbstract<JasBigInteger>
{
    private readonly GreatestCommonDivisorSimple<JasBigInteger> _delegate = new();

    public override GenPolynomial<JasBigInteger> BaseGcd(GenPolynomial<JasBigInteger> first, GenPolynomial<JasBigInteger> second)
    {
        return _delegate.BaseGcd(first, second);
    }

    public override GenPolynomial<GenPolynomial<JasBigInteger>> RecursiveUnivariateGcd(
        GenPolynomial<GenPolynomial<JasBigInteger>> first,
        GenPolynomial<GenPolynomial<JasBigInteger>> second)
    {
        return _delegate.RecursiveUnivariateGcd(first, second);
    }
}
