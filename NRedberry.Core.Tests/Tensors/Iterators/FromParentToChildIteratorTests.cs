using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class FromParentToChildIteratorTests
{
    [Fact]
    public void ShouldYieldOnlyEnteringStates()
    {
        var sum = TensorApi.Parse("a+b");
        FromParentToChildIterator iterator = new(sum);

        List<string> visited = [];
        while (iterator.Next() is { } current)
        {
            visited.Add(current.ToString(OutputFormat.Redberry));
        }

        "result".ShouldSatisfyAllConditions(
            () => visited.ShouldBe([sum.ToString(OutputFormat.Redberry), sum[0].ToString(OutputFormat.Redberry), sum[1].ToString(OutputFormat.Redberry)]),
            () => iterator.Result().ToString(OutputFormat.Redberry).ShouldBe(sum.ToString(OutputFormat.Redberry)));
    }
}
