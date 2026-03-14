using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;
using Shouldly;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class CartesianProductTests
{
    [Fact]
    public void ShouldEnumerateLexicographicProduct()
    {
        CartesianProduct<int> product = new([
            [1, 2],
            [10, 20],
            [100]
        ]);

        List<string> tuples = product.Select(tuple => string.Join(",", tuple)).ToList();

        tuples.ShouldBe(["1,10,100", "1,20,100", "2,10,100", "2,20,100"]);
    }

    [Fact]
    public void ShouldYieldEmptyWhenAnyComponentIsEmpty()
    {
        CartesianProduct<int> product = new([
            [1, 2],
            []
        ]);

        product.ShouldBeEmpty();
    }
}
