using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.substitutions.SubstitutionIterator.
/// </summary>
public sealed class SubstitutionIterator : ITreeIterator
{
    private TreeTraverseIterator Iterator { get; }

    private bool _currentModified;

    public SubstitutionIterator(Tensor tensor)
        : this(tensor, TraverseGuide.All)
    {
    }

    public SubstitutionIterator(Tensor tensor, TraverseGuide traverseGuide)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(traverseGuide);

        Iterator = new TreeTraverseIterator(tensor, traverseGuide);
    }

    public Tensor Next()
    {
        _currentModified = false;

        TraverseState? nextState;
        while ((nextState = Iterator.Next()) == TraverseState.Entering)
        {
        }

        if (nextState is null)
        {
            return null!;
        }

        return Iterator.Current();
    }

    public void UnsafeSet(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        _currentModified = true;
        Iterator.Set(tensor);
    }

    public void Set(Tensor tensor)
    {
        Set(tensor, true);
    }

    public void Set(Tensor tensor, bool supposeIndicesAreAdded)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        Tensor oldTensor = Iterator.Current();
        if (ReferenceEquals(oldTensor, tensor))
        {
            return;
        }

        if (!TensorUtils.IsZeroOrIndeterminate(tensor)
            && !tensor.Indices.GetFree().EqualsRegardlessOrder(oldTensor.Indices.GetFree()))
        {
            throw new InvalidOperationException(
                $"Substituting tensor {oldTensor} with different free indices ({oldTensor.Indices.GetFree()} != {tensor.Indices.GetFree()}).");
        }

        _currentModified = true;
        Iterator.Set(tensor);
    }

    public void SafeSet(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (!ReferenceEquals(Iterator.Current(), tensor))
        {
            Set(ApplyIndexMapping.RenameDummy(tensor, GetForbidden()));
        }
    }

    public bool IsCurrentModified()
    {
        return _currentModified;
    }

    public Tensor Result()
    {
        return Iterator.Result();
    }

    public int Depth => Iterator.Depth;

    public int[] GetForbidden()
    {
        return Array.Empty<int>();
    }

    private interface IForbiddenContainer : Payload<IForbiddenContainer>
    {
        HashSet<int> GetForbidden();

        void Submit(HashSet<int> removed, HashSet<int> added);
    }

    private sealed class FCPayloadFactory : PayloadFactory<IForbiddenContainer>
    {
        public bool AllowLazyInitialization()
        {
            throw new NotImplementedException();
        }

        public IForbiddenContainer Create(StackPosition<IForbiddenContainer> stackPosition)
        {
            throw new NotImplementedException();
        }
    }

    private abstract class AbstractForbiddenContainer : IForbiddenContainer
    {
        protected AbstractForbiddenContainer(StackPosition<IForbiddenContainer> position)
        {
            throw new NotImplementedException();
        }

        public abstract void Submit(HashSet<int> removed, HashSet<int> added);

        public abstract HashSet<int> GetForbidden();

        public Tensor OnLeaving(StackPosition<IForbiddenContainer> stackPosition)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class ProductForbiddenContainer : AbstractForbiddenContainer
    {
        public ProductForbiddenContainer(StackPosition<IForbiddenContainer> position)
            : base(position)
        {
            throw new NotImplementedException();
        }

        public override void Submit(HashSet<int> removed, HashSet<int> added)
        {
            throw new NotImplementedException();
        }

        public override HashSet<int> GetForbidden()
        {
            throw new NotImplementedException();
        }
    }

    private sealed class SumForbiddenContainer : AbstractForbiddenContainer
    {
        public SumForbiddenContainer(StackPosition<IForbiddenContainer> position)
            : base(position)
        {
            throw new NotImplementedException();
        }

        public override void Submit(HashSet<int> removed, HashSet<int> added)
        {
            throw new NotImplementedException();
        }

        public override HashSet<int> GetForbidden()
        {
            throw new NotImplementedException();
        }
    }

    private sealed class TopProductForbiddenContainer : AbstractForbiddenContainer
    {
        public TopProductForbiddenContainer(StackPosition<IForbiddenContainer> position)
            : base(position)
        {
            throw new NotImplementedException();
        }

        public override void Submit(HashSet<int> removed, HashSet<int> added)
        {
            throw new NotImplementedException();
        }

        public override HashSet<int> GetForbidden()
        {
            throw new NotImplementedException();
        }
    }

    private sealed class TransparentForbiddenContainer : IForbiddenContainer
    {
        public TransparentForbiddenContainer(IForbiddenContainer parent)
        {
            throw new NotImplementedException();
        }

        public HashSet<int> GetForbidden()
        {
            throw new NotImplementedException();
        }

        public void Submit(HashSet<int> removed, HashSet<int> added)
        {
            throw new NotImplementedException();
        }

        public Tensor OnLeaving(StackPosition<IForbiddenContainer> stackPosition)
        {
            throw new NotImplementedException();
        }
    }

    private static readonly IForbiddenContainer ScalarFunctionContainer = new ScalarFunctionForbiddenContainer();

    private sealed class ScalarFunctionForbiddenContainer : IForbiddenContainer
    {
        public HashSet<int> GetForbidden()
        {
            throw new NotImplementedException();
        }

        public void Submit(HashSet<int> removed, HashSet<int> added)
        {
            throw new NotImplementedException();
        }

        public Tensor OnLeaving(StackPosition<IForbiddenContainer> stackPosition)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class EmptyForbiddenContainer : IForbiddenContainer
    {
        public HashSet<int> GetForbidden()
        {
            throw new NotImplementedException();
        }

        public void Submit(HashSet<int> removed, HashSet<int> added)
        {
            throw new NotImplementedException();
        }

        public Tensor OnLeaving(StackPosition<IForbiddenContainer> stackPosition)
        {
            throw new NotImplementedException();
        }
    }

    private static readonly IForbiddenContainer EmptyContainer = new EmptyForbiddenContainer();
}
