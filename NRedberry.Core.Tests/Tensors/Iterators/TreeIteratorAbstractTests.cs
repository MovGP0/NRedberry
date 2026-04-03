using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using TensorType = NRedberry.Tensors.Tensor;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class TreeIteratorAbstractTests
{
    [Fact]
    public void ShouldFilterTraversalByRequestedState()
    {
        TestTreeIterator enteringIterator = new(TensorApi.Parse("a+b"), TraverseState.Entering);
        TestTreeIterator leavingIterator = new(TensorApi.Parse("a+b"), TraverseState.Leaving);

        List<TensorType> enteringVisited = Drain(enteringIterator);
        List<TensorType> leavingVisited = Drain(leavingIterator);

        enteringVisited.ShouldSatisfyAllConditions(
            () => enteringVisited.Count.ShouldBe(3),
            () => TensorUtils.Equals(enteringVisited[0], TensorApi.Parse("a+b")).ShouldBeTrue(),
            () => GetSortedRedberryStrings(enteringVisited[1..]).ShouldBe(["a", "b"]));

        leavingVisited.ShouldSatisfyAllConditions(
            () => leavingVisited.Count.ShouldBe(3),
            () => GetSortedRedberryStrings(leavingVisited[..2]).ShouldBe(["a", "b"]),
            () => TensorUtils.Equals(leavingVisited[2], TensorApi.Parse("a+b")).ShouldBeTrue());
    }

    [Fact]
    public void ShouldForwardSetResultAndDepth()
    {
        TestTreeIterator iterator = new(TensorApi.Parse("a+b"), TraverseState.Entering);

        TensorType root = iterator.Next()!;
        TensorUtils.Equals(root, TensorApi.Parse("a+b")).ShouldBeTrue();
        iterator.Depth.ShouldBe(0);

        TensorType selectedLeaf = iterator.Next()!;
        string selectedLeafString = ToRedberryString(selectedLeaf);
        selectedLeafString.ShouldBeOneOf("a", "b");
        iterator.Depth.ShouldBe(1);

        iterator.Set(NRedberry.Numbers.Complex.Zero);

        while (iterator.Next() is not null)
        {
        }

        string expectedRemainingLeaf = selectedLeafString == "a" ? "b" : "a";
        iterator.Result().ToString(OutputFormat.Redberry).ShouldBe(expectedRemainingLeaf);
    }

    private static List<TensorType> Drain(ITreeIterator iterator)
    {
        List<TensorType> visited = [];
        while (iterator.Next() is { } current)
        {
            visited.Add(current);
        }

        return visited;
    }

    private static string ToRedberryString(TensorType tensor)
    {
        return tensor.ToString(OutputFormat.Redberry);
    }

    private static string[] GetSortedRedberryStrings(IEnumerable<TensorType> tensors)
    {
        string[] values = tensors.Select(ToRedberryString).ToArray();
        Array.Sort(values, StringComparer.Ordinal);
        return values;
    }

    private sealed class TestTreeIterator(NRedberry.Tensors.Tensor tensor, TraverseState state)
        : TreeIteratorAbstract(tensor, state);
}
