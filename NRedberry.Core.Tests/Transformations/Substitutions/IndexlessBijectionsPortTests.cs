using Shouldly;
using NRedberry.Transformations.Substitutions;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Transformations.Substitutions;

public sealed class IndexlessBijectionsPortTests
{
    [Fact]
    public void ShouldEnumerateDistinctBijectionsForMatchingIndexlessFactors()
    {
        IndexlessBijectionsPort port = new(
            [TensorApi.Parse("a"), TensorApi.Parse("a")],
            [TensorApi.Parse("a"), TensorApi.Parse("a"), TensorApi.Parse("a")]);

        List<string> bijections = [];
        int[]? reference;
        while ((reference = port.Take()) is not null)
        {
            bijections.Add(string.Join(",", (int[])reference.Clone()));
        }

        bijections.ShouldBe(["0,1", "0,2", "1,0", "1,2", "2,0", "2,1"]);
    }

    [Fact]
    public void ShouldReturnNullWhenSourceHasMoreFactorsThanTarget()
    {
        IndexlessBijectionsPort port = new(
            [TensorApi.Parse("a"), TensorApi.Parse("a")],
            [TensorApi.Parse("a")]);

        port.Take().ShouldBeNull();
    }
}
