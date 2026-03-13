using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Arith;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class PrimeListTests
{
    [Fact]
    public void ShouldExposeKnownPrimeSequences()
    {
        PrimeList small = new(PrimeList.Range.Small);

        Assert.Equal(10, small.Size());
        Assert.Equal("2", small.Get(0).ToString());
        Assert.Equal("29", small.Get(9).ToString());
        Assert.Equal("31", small.Get(10).ToString());
    }

    [Fact]
    public void ShouldComputeSpecificPrimeFamilies()
    {
        Assert.Equal("31", PrimeList.GetLongPrime(5, 1).ToString());
        Assert.Equal("31", PrimeList.GetMersennePrime(5).ToString());
    }

    [Fact]
    public void ShouldEnumeratePrimesLazily()
    {
        PrimeList primes = new(PrimeList.Range.Small);

        Assert.Equal(["2", "3", "5", "7", "11"], primes.Take(5).Select(x => x.ToString()).ToArray());
    }
}
