using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class TreeIteratorAbstractTests
{
    [Fact]
    public void ShouldFilterTraversalByRequestedState()
    {
        TestTreeIterator enteringIterator = new(TensorApi.Parse("a+b"), TraverseState.Entering);
        TestTreeIterator leavingIterator = new(TensorApi.Parse("a+b"), TraverseState.Leaving);

        Assert.Equal(["a+b", "a", "b"], Drain(enteringIterator));
        Assert.Equal(["a", "b", "a+b"], Drain(leavingIterator));
    }

    [Fact]
    public void ShouldForwardSetResultAndDepth()
    {
        TestTreeIterator iterator = new(TensorApi.Parse("a+b"), TraverseState.Entering);

        Assert.Equal("a+b", iterator.Next()!.ToString(OutputFormat.Redberry));
        Assert.Equal(0, iterator.Depth);
        Assert.Equal("a", iterator.Next()!.ToString(OutputFormat.Redberry));
        Assert.Equal(1, iterator.Depth);

        iterator.Set(NRedberry.Numbers.Complex.Zero);

        while (iterator.Next() is not null)
        {
        }

        Assert.Equal("b", iterator.Result().ToString(OutputFormat.Redberry));
    }

    private static List<string> Drain(ITreeIterator iterator)
    {
        List<string> visited = [];
        while (iterator.Next() is { } current)
        {
            visited.Add(current.ToString(OutputFormat.Redberry));
        }

        return visited;
    }

    private sealed class TestTreeIterator(NRedberry.Tensors.Tensor tensor, TraverseState state)
        : TreeIteratorAbstract(tensor, state);
}
