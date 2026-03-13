using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class FromChildToParentIteratorTests
{
    [Fact]
    public void ShouldYieldOnlyLeavingStates()
    {
        FromChildToParentIterator iterator = new(TensorApi.Parse("a+b"));

        List<string> visited = [];
        while (iterator.Next() is { } current)
        {
            visited.Add(current.ToString(OutputFormat.Redberry));
        }

        Assert.Equal(["a", "b", "a+b"], visited);
        Assert.Equal("a+b", iterator.Result().ToString(OutputFormat.Redberry));
    }
}
