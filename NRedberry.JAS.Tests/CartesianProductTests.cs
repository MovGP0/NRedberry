using System.Linq;
using NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;
using Xunit;

namespace NRedberry.JAS.Tests;

public sealed class CartesianProductTests
{
    [Fact]
    public void ShouldEnumerateLexicographicProduct()
    {
        CartesianProduct<int> product = new([
            new[] { 1, 2 },
            new[] { 10, 20 },
            new[] { 100 }
        ]);

        List<string> tuples = product.Select(tuple => string.Join(",", tuple)).ToList();

        Assert.Equal(
            ["1,10,100", "1,20,100", "2,10,100", "2,20,100"],
            tuples);
    }

    [Fact]
    public void ShouldYieldEmptyWhenAnyComponentIsEmpty()
    {
        CartesianProduct<int> product = new([
            new[] { 1, 2 },
            Array.Empty<int>()
        ]);

        Assert.Empty(product);
    }
}
