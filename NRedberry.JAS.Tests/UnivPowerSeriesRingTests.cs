using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Poly;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class UnivPowerSeriesRingTests
{
    [Fact]
    public void ShouldProvideIdentityElementsMetadataAndIntegerEmbedding()
    {
        UnivPowerSeriesRing<BigRational> ring = CreateRing();
        UnivPowerSeries<BigRational> three = ring.FromInteger(3);
        List<UnivPowerSeries<BigRational>> generators = ring.Generators();

        ring.ToString().ShouldBe("BigRational((t))");
        ring.GetZero().IsZero().ShouldBeTrue();
        ring.GetONE().IsOne().ShouldBeTrue();
        ring.IsFinite().ShouldBeFalse();
        ring.IsCommutative().ShouldBeTrue();
        ring.IsAssociative().ShouldBeTrue();
        ring.IsField().ShouldBeFalse();
        ring.Characteristic().ShouldBe(System.Numerics.BigInteger.Zero);
        ring.Truncate.ShouldBe(3);
        three.Coefficient(0).ToString().ShouldBe("3");
        three.Coefficient(1).ToString().ShouldBe("0");
        generators.ShouldNotBeEmpty();
    }

    [Fact]
    public void ShouldCreateTaylorSeriesAndRandomSamples()
    {
        UnivPowerSeriesRing<BigRational> ring = CreateRing();
        GenPolynomialRing<BigRational> polynomialRing = new(new BigRational(), 1, ["x"]);
        GenPolynomial<BigRational> polynomial = polynomialRing.Univariate(0, 2).Sum(new BigRational(1));
        PolynomialTaylorFunction<BigRational> function = new(polynomial);

        UnivPowerSeries<BigRational> series = ring.SeriesOfTaylor(function, new BigRational(1));
        UnivPowerSeries<BigRational> randomSeries = ring.Random(4, 1.0f, new Random(1234));
        UnivPowerSeries<BigRational> copied = ring.Copy(series);

        series.Coefficient(0).ToString().ShouldBe("2");
        series.Coefficient(1).ToString().ShouldBe("2");
        series.Coefficient(2).ToString().ShouldBe("1");
        series.Coefficient(3).ToString().ShouldBe("0");
        copied.ToString().ShouldBe(series.ToString());
        randomSeries.Coefficient(0).ToString().ShouldNotBe("0");
    }

    private static UnivPowerSeriesRing<BigRational> CreateRing()
    {
        return new UnivPowerSeriesRing<BigRational>(new BigRational(), 3, "t");
    }
}
