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

        values.ShouldBe(["1,2", "1,3", "2,3"]);
    }

    [Fact]
    public void ShouldReturnSingleEmptySubsetForZeroK()
    {
        KsubSet<int> subsets = new([1, 2, 3], 0);

        List<List<int>> values = subsets.ToList();

        values.ShouldHaveSingleItem();
        values[0].ShouldBeEmpty();
    }
}
