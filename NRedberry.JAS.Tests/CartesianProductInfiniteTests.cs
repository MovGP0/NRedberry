using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;
using Shouldly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class CartesianProductInfiniteTests
{
    [Fact]
    public void ShouldEnumerateByIncreasingIndexLevel()
    {
        CartesianProductInfinite<int> product = new([
            [0, 1, 2],
            [10, 11, 12]
        ]);

        List<string> tuples = product.Take(6).Select(tuple => string.Join(",", tuple)).ToList();

        tuples.ShouldBe(["0,10", "0,11", "1,10", "0,12", "1,11", "2,10"]);
    }

    [Fact]
    public void ShouldRejectEmptyComponentList()
    {
        Should.Throw<ArgumentException>(() => new CartesianProductInfinite<int>([]));
    }
}
