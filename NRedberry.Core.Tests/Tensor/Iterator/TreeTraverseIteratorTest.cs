using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorType = NRedberry.Tensors.Tensor;

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

        states.ShouldBe([TraverseState.Entering, TraverseState.Entering, TraverseState.Leaving, TraverseState.Leaving]);
        depths.ShouldBe([0, 1, 1, 0]);
        TensorUtils.EqualsExactly([tensor, tensor[0], tensor[0], tensor], visited.ToArray()).ShouldBeTrue();
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

        SimpleTensor result = iterator.Result().ShouldBeOfType<SimpleTensor>();

        result.Indices.GetFree().Size().ShouldBe(0);
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

        depths.ShouldBe([0, 1, 1, 1, 1, 0]);
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
