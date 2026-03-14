using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Tensors.Iterators;

public sealed class TreeTraverseIteratorTests
{
    [Fact]
    public void ShouldTraverseTreeInEnteringAndLeavingOrder()
    {
        TreeTraverseIterator iterator = new(TensorApi.Parse("a+b"));

        List<(TraverseState State, string Tensor, int Depth)> visited = [];
        TraverseState? state;
        while ((state = iterator.Next()) is not null)
        {
            visited.Add((state.Value, iterator.Current().ToString(OutputFormat.Redberry), iterator.Depth));
        }

        visited.ShouldBe([
                (TraverseState.Entering, "a+b", 0),
                (TraverseState.Entering, "a", 1),
                (TraverseState.Leaving, "a", 1),
                (TraverseState.Entering, "b", 1),
                (TraverseState.Leaving, "b", 1),
                (TraverseState.Leaving, "a+b", 0),
            ]);
    }

    [Fact]
    public void ShouldReplaceCurrentTensorAndReturnModifiedResult()
    {
        TreeTraverseIterator iterator = new(TensorApi.Parse("a+b"));

        while (iterator.Next() is not null)
        {
            if (iterator.Current().ToString(OutputFormat.Redberry) == "a")
            {
                iterator.Set(NRedberry.Numbers.Complex.Zero);
                break;
            }
        }

        while (iterator.Next() is not null)
        {
        }

        iterator.Result().ToString(OutputFormat.Redberry).ShouldBe("b");
    }
}
