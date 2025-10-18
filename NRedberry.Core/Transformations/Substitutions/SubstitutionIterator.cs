using NRedberry.Core.Tensors;
using NRedberry.Core.Tensors.Iterators;

namespace NRedberry.Core.Transformations.Substitutions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.substitutions.SubstitutionIterator.
/// </summary>
public sealed class SubstitutionIterator : ITreeIterator
{
    public SubstitutionIterator(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public SubstitutionIterator(Tensor tensor, TraverseGuide traverseGuide)
    {
        throw new NotImplementedException();
    }

    public Tensor Next()
    {
        throw new NotImplementedException();
    }

    public void UnsafeSet(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public void Set(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public void Set(Tensor tensor, bool supposeIndicesAreAdded)
    {
        throw new NotImplementedException();
    }

    public void SafeSet(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public bool IsCurrentModified()
    {
        throw new NotImplementedException();
    }

    public Tensor Result()
    {
        throw new NotImplementedException();
    }

    public int Depth => throw new NotImplementedException();

    public int[] GetForbidden()
    {
        throw new NotImplementedException();
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
