using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;
using Xunit;

namespace NRedberry.Core.Tests.Tensor.Iterator;

public sealed class TreeTraverseIteratorTest
{
    [Fact]
    public void ShouldTraverseTreeWithCustomGuide()
    {
        TensorType tensor = TensorApi.Parse("a+b");
        TreeTraverseIterator iterator = new(tensor, new HideSecondChildGuide());
        List<TraverseState> states = [];
        List<int> depths = [];
        List<TensorType> visited = [];

        TraverseState? state;
        while ((state = iterator.Next()) is not null)
        {
            states.Add(state.Value);
            depths.Add(iterator.Depth);
            visited.Add(iterator.Current());
        }

        Assert.Equal([TraverseState.Entering, TraverseState.Entering, TraverseState.Leaving, TraverseState.Leaving], states);
        Assert.Equal([0, 1, 1, 0], depths);
        Assert.True(TensorUtils.EqualsExactly([tensor, tensor[0], tensor[0], tensor], visited.ToArray()));
    }

    [Fact]
    public void ShouldReplaceNodesDuringTraversal()
    {
        TensorType tensor = TensorApi.Parse("a+b");
        TreeTraverseIterator iterator = new(tensor);

        while (iterator.Next() is not null)
        {
            if (TensorUtils.EqualsExactly(iterator.Current(), tensor[0]))
            {
                iterator.Set(NRedberry.Numbers.Complex.Zero);
                break;
            }
        }

        while (iterator.Next() is not null)
        {
        }

        SimpleTensor result = Assert.IsType<SimpleTensor>(iterator.Result());

        Assert.Equal(0, result.Indices.GetFree().Size());
    }

    [Fact]
    public void ShouldTrackDepthForNestedSum()
    {
        TreeTraverseIterator iterator = new(TensorApi.Parse("a+b"));
        List<int> depths = [];

        while (iterator.Next() is not null)
        {
            depths.Add(iterator.Depth);
        }

        Assert.Equal([0, 1, 1, 1, 1, 0], depths);
    }

    private sealed class HideSecondChildGuide : TraverseGuide
    {
        public TraversePermission GetPermission(NRedberry.Tensors.Tensor tensor, NRedberry.Tensors.Tensor parent, int indexInParent)
        {
            if (indexInParent == 1)
            {
                return TraversePermission.DontShow;
            }

            return TraversePermission.Enter;
        }
    }
}
