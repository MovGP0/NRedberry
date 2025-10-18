using NRedberry.Core.Tensors;

namespace NRedberry.Core.Tensors.Iterators;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/iterator/FromParentToChildIterator.java
 */

public sealed class FromParentToChildIterator : TreeIteratorAbstract
{
    public FromParentToChildIterator(Tensor tensor, TraverseGuide guide)
        : base(tensor, guide, TraverseState.Entering)
    {
    }

    public FromParentToChildIterator(Tensor tensor)
        : base(tensor, TraverseState.Entering)
    {
    }
}
