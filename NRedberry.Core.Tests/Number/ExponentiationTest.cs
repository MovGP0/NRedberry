using System.Numerics;
using NRedberry.Numbers;
using NRedberry.Numbers.Parser;
using NumberComplex = NRedberry.Numbers.Complex;

namespace NRedberry.Core.Tests.Number;

public sealed class ExponentiationTest
{
    [Fact]
    public void ShouldFindIntegerRoots()
    {
        Exponentiation.FindIntegerRoot(new BigInteger(28), new BigInteger(3)).ShouldBeNull();
        Exponentiation.FindIntegerRoot(new BigInteger(22), new BigInteger(3)).ShouldBeNull();
        Exponentiation.FindIntegerRoot(new BigInteger(27), new BigInteger(3)).ShouldBe(new BigInteger(3));
        Exponentiation.FindIntegerRoot(new BigInteger(49), new BigInteger(2)).ShouldBe(new BigInteger(7));
        Exponentiation.FindIntegerRoot(new BigInteger(129140163), new BigInteger(17)).ShouldBe(new BigInteger(3));
        Exponentiation.FindIntegerRoot(new BigInteger(129140162), new BigInteger(17)).ShouldBeNull();
        Exponentiation.FindIntegerRoot(new BigInteger(129140164), new BigInteger(17)).ShouldBeNull();
        Exponentiation.FindIntegerRoot(new BigInteger(19073486328125L), new BigInteger(19)).ShouldBe(new BigInteger(5));
        Exponentiation.FindIntegerRoot(new BigInteger(19073486328123L), new BigInteger(19)).ShouldBeNull();
        Exponentiation.FindIntegerRoot(new BigInteger(19073486328128L), new BigInteger(19)).ShouldBeNull();
    }

    [Fact]
    public void ShouldExponentiateWhenPossible()
    {
        Real first = NumberParser<Real>.RealParser.Parse("25/144");
        Real firstPower = NumberParser<Real>.RealParser.Parse("-1/2");
        Real second = NumberParser<Real>.RealParser.Parse("27/343");
        Real secondPower = NumberParser<Real>.RealParser.Parse("2/3");
        Real thirdPower = NumberParser<Real>.RealParser.Parse("2/4");
        Real fourthPower = NumberParser<Real>.RealParser.Parse("0.5");

        Exponentiation.ExponentiateIfPossible(first, firstPower).ShouldBe(new Rational(12, 5));
        Exponentiation.ExponentiateIfPossible(second, secondPower).ShouldBe(new Rational(9, 49));
        Exponentiation.ExponentiateIfPossible(second, thirdPower).ShouldBeNull();

        Numeric numeric = Exponentiation.ExponentiateIfPossible(second, fourthPower).ShouldBeOfType<Numeric>();
        numeric.DoubleValue().ShouldBe(0.28056585887484736, 1e-15);
    }

    [Fact]
    public void ShouldFindIntegerRootOfComplex()
    {
        NumberComplex value = NumberParser<NumberComplex>.ComplexParser.Parse("256/129140163+256/129140163*I");
        NumberComplex expected = NumberParser<NumberComplex>.ComplexParser.Parse("1/3+1/3*I");

        Exponentiation.FindIntegerRoot(value, new BigInteger(17)).ShouldBe(expected);
    }
}
