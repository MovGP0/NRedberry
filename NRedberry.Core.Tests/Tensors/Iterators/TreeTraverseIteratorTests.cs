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
        TensorType sum = TensorApi.Parse("a+b");
        string sumText = sum.ToString(OutputFormat.Redberry);
        string firstChild = sum[0].ToString(OutputFormat.Redberry);
        string secondChild = sum[1].ToString(OutputFormat.Redberry);
        TreeTraverseIterator iterator = new(sum);

        List<(TraverseState State, string Tensor, int Depth)> visited = [];
        TraverseState? state;
        while ((state = iterator.Next()) is not null)
        {
            visited.Add((state.Value, iterator.Current().ToString(OutputFormat.Redberry), iterator.Depth));
        }

        visited.ShouldBe([
            (TraverseState.Entering, sumText, 0),
            (TraverseState.Entering, firstChild, 1),
            (TraverseState.Leaving, firstChild, 1),
            (TraverseState.Entering, secondChild, 1),
            (TraverseState.Leaving, secondChild, 1),
            (TraverseState.Leaving, sumText, 0),
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
