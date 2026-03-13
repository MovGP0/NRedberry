using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class CoefficientsTests
{
    [Fact]
    public void ShouldCacheGeneratedCoefficients()
    {
        CountingCoefficients coefficients = new();

        BigRational first = coefficients.Get(3);
        BigRational second = coefficients.Get(3);

        Assert.Equal("4", first.ToString());
        Assert.Equal(first, second);
        Assert.Equal(1, coefficients.GenerateCount);
        Assert.True(coefficients.CoeffCache.ContainsKey(3));
    }

    [Fact]
    public void ShouldUsePrefilledCacheWithoutGenerating()
    {
        CountingCoefficients coefficients = new(new Dictionary<int, BigRational>
        {
            [2] = new BigRational(7)
        });

        BigRational value = coefficients.Get(2);

        Assert.Equal("7", value.ToString());
        Assert.Equal(0, coefficients.GenerateCount);
    }
}

file sealed class CountingCoefficients : Coefficients<BigRational>
{
    public int GenerateCount { get; private set; }

    public CountingCoefficients()
    {
    }

    public CountingCoefficients(Dictionary<int, BigRational> cache)
        : base(cache)
    {
    }

    protected override BigRational Generate(int index)
    {
        GenerateCount++;
        return new BigRational(index + 1);
    }
}
