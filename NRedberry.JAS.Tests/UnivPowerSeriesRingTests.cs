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

        Assert.Equal("BigRational((t))", ring.ToString());
        Assert.True(ring.GetZero().IsZero());
        Assert.True(ring.GetONE().IsOne());
        Assert.False(ring.IsFinite());
        Assert.True(ring.IsCommutative());
        Assert.True(ring.IsAssociative());
        Assert.False(ring.IsField());
        Assert.Equal(System.Numerics.BigInteger.Zero, ring.Characteristic());
        Assert.Equal(3, ring.Truncate);
        Assert.Equal("3", three.Coefficient(0).ToString());
        Assert.Equal("0", three.Coefficient(1).ToString());
        Assert.NotEmpty(generators);
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

        Assert.Equal("2", series.Coefficient(0).ToString());
        Assert.Equal("2", series.Coefficient(1).ToString());
        Assert.Equal("1", series.Coefficient(2).ToString());
        Assert.Equal("0", series.Coefficient(3).ToString());
        Assert.Equal(series.ToString(), copied.ToString());
        Assert.NotEqual("0", randomSeries.Coefficient(0).ToString());
    }

    private static UnivPowerSeriesRing<BigRational> CreateRing()
    {
        return new UnivPowerSeriesRing<BigRational>(new BigRational(), 3, "t");
    }
}
