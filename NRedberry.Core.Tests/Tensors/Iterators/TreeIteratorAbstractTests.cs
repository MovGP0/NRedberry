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

        Drain(enteringIterator).ShouldBe(["a+b", "a", "b"]);
        Drain(leavingIterator).ShouldBe(["a", "b", "a+b"]);
    }

    [Fact]
    public void ShouldForwardSetResultAndDepth()
    {
        TestTreeIterator iterator = new(TensorApi.Parse("a+b"), TraverseState.Entering);

        iterator.Next()!.ToString(OutputFormat.Redberry).ShouldBe("a+b");
        iterator.Depth.ShouldBe(0);
        iterator.Next()!.ToString(OutputFormat.Redberry).ShouldBe("a");
        iterator.Depth.ShouldBe(1);

        iterator.Set(NRedberry.Numbers.Complex.Zero);

        while (iterator.Next() is not null)
        {
        }

        iterator.Result().ToString(OutputFormat.Redberry).ShouldBe("b");
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
