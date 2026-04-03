using NRedberry.Core.Tests.Extensions;
using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class FromChildToParentIteratorTests
{
    [Fact]
    public void ShouldYieldOnlyLeavingStates()
    {
        var sum = TensorApi.Parse("a+b");
        FromChildToParentIterator iterator = new(sum);

        List<string> visited = [];
        while (iterator.Next() is { } current)
        {
            visited.Add(current.ToString(OutputFormat.Redberry));
        }

        Should.SatisfyAllConditions(
            () => visited.ShouldBe(
            [
                sum[0].ToString(OutputFormat.Redberry),
                sum[1].ToString(OutputFormat.Redberry),
                sum.ToString(OutputFormat.Redberry)
            ]),
            () => iterator.Result().ToString(OutputFormat.Redberry).ShouldBe(sum.ToString(OutputFormat.Redberry)));
    }
}
