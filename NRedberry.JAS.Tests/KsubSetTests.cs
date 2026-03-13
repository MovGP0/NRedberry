using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class KsubSetTests
{
    [Fact]
    public void ShouldEnumerateAllKElementSubsets()
    {
        KsubSet<int> subsets = new([1, 2, 3], 2);

        List<string> values = subsets.Select(tuple => string.Join(",", tuple)).ToList();

        Assert.Equal(["1,2", "1,3", "2,3"], values);
    }

    [Fact]
    public void ShouldReturnSingleEmptySubsetForZeroK()
    {
        KsubSet<int> subsets = new([1, 2, 3], 0);

        List<List<int>> values = subsets.ToList();

        Assert.Single(values);
        Assert.Empty(values[0]);
    }
}
