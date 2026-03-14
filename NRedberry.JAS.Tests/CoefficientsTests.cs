using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Ps;
using Shouldly;
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

        first.ToString().ShouldBe("4");
        second.ShouldBe(first);
        coefficients.GenerateCount.ShouldBe(1);
        coefficients.CoeffCache.ContainsKey(3).ShouldBeTrue();
    }

    [Fact]
    public void ShouldUsePrefilledCacheWithoutGenerating()
    {
        CountingCoefficients coefficients = new(new Dictionary<int, BigRational>
        {
            [2] = new BigRational(7)
        });

        BigRational value = coefficients.Get(2);

        value.ToString().ShouldBe("7");
        coefficients.GenerateCount.ShouldBe(0);
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
