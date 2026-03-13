using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class CartesianProductInfiniteTests
{
    [Fact]
    public void ShouldEnumerateByIncreasingIndexLevel()
    {
        CartesianProductInfinite<int> product = new([
            new[] { 0, 1, 2 },
            new[] { 10, 11, 12 }
        ]);

        List<string> tuples = product.Take(6).Select(tuple => string.Join(",", tuple)).ToList();

        Assert.Equal(
            ["0,10", "0,11", "1,10", "0,12", "1,11", "2,10"],
            tuples);
    }

    [Fact]
    public void ShouldRejectEmptyComponentList()
    {
        Assert.Throws<ArgumentException>(() => new CartesianProductInfinite<int>([]));
    }
}
