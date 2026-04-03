using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class PrimeListTests
{
    [Fact]
    public void ShouldExposeKnownPrimeSequences()
    {
        PrimeList small = new(PrimeList.Range.Small);

        small.Size().ShouldBe(10);
        small.Get(0).ToString().ShouldBe("2");
        small.Get(9).ToString().ShouldBe("29");
        small.Get(10).ToString().ShouldBe("31");
    }

    [Fact]
    public void ShouldComputeSpecificPrimeFamilies()
    {
        PrimeList.GetLongPrime(5, 1).ToString().ShouldBe("31");
        PrimeList.GetMersennePrime(5).ToString().ShouldBe("31");
    }

    [Fact]
    public void ShouldEnumeratePrimesLazily()
    {
        PrimeList primes = new(PrimeList.Range.Small);

        primes.Take(5).Select(x => x.ToString()).ToArray().ShouldBe(["2", "3", "5", "7", "11"]);
    }
}
