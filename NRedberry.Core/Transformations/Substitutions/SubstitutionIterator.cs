using System.Collections;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using NRedberry.Tensors.Iterators;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Port of cc.redberry.core.transformations.substitutions.SubstitutionIterator.
/// </summary>
public sealed class SubstitutionIterator : ITreeIterator
{
    private static readonly HashSet<int> s_emptyIntSet = [];
    private static readonly IForbiddenContainer s_scalarFunctionContainer = new ScalarFunctionForbiddenContainer();
    private static readonly IForbiddenContainer s_emptyContainer = new EmptyForbiddenContainer();

    private TreeTraverseIterator<IForbiddenContainer> InnerIterator { get; }

    public SubstitutionIterator(Tensor tensor)
        : this(tensor, TraverseGuide.All)
    {
    }

    public SubstitutionIterator(Tensor tensor, TraverseGuide traverseGuide)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(traverseGuide);

        InnerIterator = new TreeTraverseIterator<IForbiddenContainer>(tensor, traverseGuide, new FCPayloadFactory());
    }

    public Tensor Next()
    {
        TraverseState? nextState;
        while ((nextState = InnerIterator.Next()) == TraverseState.Entering)
        {
        }

        if (nextState is null)
        {
            return null!;
        }

        return InnerIterator.Current();
    }

    public void UnsafeSet(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        InnerIterator.Set(tensor);
    }

    public void Set(Tensor tensor)
    {
        Set(tensor, true);
    }

    public void Set(Tensor tensor, bool supposeIndicesAreAdded)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        Tensor oldTensor = InnerIterator.Current();
        if (ReferenceEquals(oldTensor, tensor))
        {
            return;
        }

        if (TensorUtils.IsZeroOrIndeterminate(tensor))
        {
            InnerIterator.Set(tensor);
            return;
        }

        if (!tensor.Indices.GetFree().EqualsRegardlessOrder(oldTensor.Indices.GetFree()))
        {
            throw new InvalidOperationException(
                $"Substituting tensor {tensor} with different free indices ({oldTensor.Indices.GetFree()} != {tensor.Indices.GetFree()}).");
        }

        if (supposeIndicesAreAdded)
        {
            StackPosition<IForbiddenContainer> previous = InnerIterator.CurrentStackPosition().Previous();
            if (previous is not null)
            {
                HashSet<int> oldDummyIndices = TensorUtils.GetAllDummyIndicesT(oldTensor);
                HashSet<int> newDummyIndices = TensorUtils.GetAllDummyIndicesT(tensor);

                HashSet<int> added = new(newDummyIndices);
                added.ExceptWith(oldDummyIndices);

                if (added.Count != 0 || previous.IsPayloadInitialized())
                {
                    IForbiddenContainer forbiddenContainer = previous.GetPayload();

                    HashSet<int> removed = new(oldDummyIndices);
                    removed.ExceptWith(newDummyIndices);

                    forbiddenContainer.Submit(removed, added);
                }
            }
        }

        InnerIterator.Set(tensor);
    }

    public void SafeSet(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (!ReferenceEquals(InnerIterator.Current(), tensor))
        {
            Set(ApplyIndexMapping.RenameDummy(tensor, GetForbidden()));
        }
    }

    public bool IsCurrentModified()
    {
        return InnerIterator.CurrentStackPosition().IsModified();
    }

    public Tensor Result()
    {
        return InnerIterator.Result();
    }

    public int Depth => InnerIterator.Depth();

    public int[] GetForbidden()
    {
        StackPosition<IForbiddenContainer> previous = InnerIterator.CurrentStackPosition().Previous();
        if (previous is null)
        {
            return [];
        }

        return [.. previous.GetPayload().GetForbidden()];
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
            return true;
        }

        public IForbiddenContainer Create(StackPosition<IForbiddenContainer> stackPosition)
        {
            ArgumentNullException.ThrowIfNull(stackPosition);

            Tensor tensor = stackPosition.GetInitialTensor();
            StackPosition<IForbiddenContainer> previousPosition = stackPosition.Previous();
            IForbiddenContainer parent = previousPosition is null
                ? s_emptyContainer
                : previousPosition.GetPayload();

            if (ReferenceEquals(parent, s_emptyContainer))
            {
                if (tensor is Product)
                {
                    return new TopProductForbiddenContainer(stackPosition);
                }

                return s_emptyContainer;
            }

            if (tensor is Product)
            {
                return new ProductForbiddenContainer(stackPosition);
            }

            if (tensor is Sum)
            {
                return new SumForbiddenContainer(stackPosition);
            }

            if (tensor is TensorField)
            {
                return s_emptyContainer;
            }

            if (tensor is ScalarFunction)
            {
                return s_scalarFunctionContainer;
            }

            return new TransparentForbiddenContainer(parent);
        }
    }

    private abstract class AbstractForbiddenContainer : IForbiddenContainer
    {
        protected AbstractForbiddenContainer(StackPosition<IForbiddenContainer> position)
        {
            Position = position ?? throw new ArgumentNullException(nameof(position));
            Tensor = position.GetInitialTensor();
        }

        protected StackPosition<IForbiddenContainer> Position { get; }

        protected Tensor Tensor { get; }

        protected HashSet<int>? Forbidden { get; set; }

        public abstract void Submit(HashSet<int> removed, HashSet<int> added);

        protected abstract void EnsureInitialized();

        public virtual HashSet<int> GetForbidden()
        {
            EnsureInitialized();

            HashSet<int> result = new(Forbidden!);
            result.ExceptWith(TensorUtils.GetAllIndicesNamesT(Tensor[Position.CurrentIndex()]));
            return result;
        }

        public virtual Tensor OnLeaving(StackPosition<IForbiddenContainer> stackPosition)
        {
            _ = stackPosition;
            return null!;
        }
    }

    private sealed class ProductForbiddenContainer : AbstractForbiddenContainer
    {
        public ProductForbiddenContainer(StackPosition<IForbiddenContainer> position)
            : base(position)
        {
        }

        public override void Submit(HashSet<int> removed, HashSet<int> added)
        {
            ArgumentNullException.ThrowIfNull(removed);
            ArgumentNullException.ThrowIfNull(added);

            EnsureInitialized();
            Forbidden!.UnionWith(added);
            Forbidden.ExceptWith(removed);
            Position.Previous().GetPayload().Submit(removed, added);
        }

        protected override void EnsureInitialized()
        {
            if (Forbidden is not null)
            {
                return;
            }

            Forbidden = new HashSet<int>(Position.Previous().GetPayload().GetForbidden());
            Forbidden.UnionWith(TensorUtils.GetAllIndicesNamesT(Tensor));
        }
    }

    private sealed class SumForbiddenContainer : AbstractForbiddenContainer
    {
        private int[]? _allDummyIndices;
        private BitArray[]? _usedArrays;

        public SumForbiddenContainer(StackPosition<IForbiddenContainer> position)
            : base(position)
        {
        }

        public override void Submit(HashSet<int> removed, HashSet<int> added)
        {
            ArgumentNullException.ThrowIfNull(removed);
            ArgumentNullException.ThrowIfNull(added);

            EnsureInitialized();

            HashSet<int>? parentRemoved = null;
            foreach (int index in removed)
            {
                int indexPosition = Array.BinarySearch(_allDummyIndices!, index);
                _usedArrays![indexPosition].Set(Position.CurrentIndex(), false);
                if (IsBitArrayEmpty(_usedArrays[indexPosition]))
                {
                    parentRemoved ??= new HashSet<int>(removed.Count);
                    parentRemoved.Add(index);
                }
            }

            HashSet<int> parentAdded = new(added);
            foreach (int index in parentAdded.ToArray())
            {
                int indexPosition = Array.BinarySearch(_allDummyIndices!, index);
                if (indexPosition < 0)
                {
                    continue;
                }

                if (!IsBitArrayEmpty(_usedArrays![indexPosition]))
                {
                    parentAdded.Remove(index);
                }

                _usedArrays[indexPosition].Set(Position.CurrentIndex(), true);
            }

            Position.Previous().GetPayload().Submit(parentRemoved ?? s_emptyIntSet, parentAdded);
        }

        public override HashSet<int> GetForbidden()
        {
            EnsureInitialized();
            return new HashSet<int>(Forbidden!);
        }

        protected override void EnsureInitialized()
        {
            if (Forbidden is not null)
            {
                return;
            }

            Forbidden = Position.Previous().GetPayload().GetForbidden();

            HashSet<int> allDummyIndices = TensorUtils.GetAllDummyIndicesT(Tensor);
            _allDummyIndices = [.. allDummyIndices];
            Array.Sort(_allDummyIndices);

            int size = Tensor.Size;
            _usedArrays = new BitArray[_allDummyIndices.Length];
            for (int i = _allDummyIndices.Length - 1; i >= 0; --i)
            {
                _usedArrays[i] = new BitArray(size);
            }

            for (int i = size - 1; i >= 0; --i)
            {
                HashSet<int> dummy = TensorUtils.GetAllDummyIndicesT(Tensor[i]);
                foreach (int index in dummy)
                {
                    _usedArrays[Array.BinarySearch(_allDummyIndices, index)].Set(i, true);
                }
            }
        }
    }

    private sealed class TopProductForbiddenContainer : AbstractForbiddenContainer
    {
        public TopProductForbiddenContainer(StackPosition<IForbiddenContainer> position)
            : base(position)
        {
        }

        public override void Submit(HashSet<int> removed, HashSet<int> added)
        {
            ArgumentNullException.ThrowIfNull(removed);
            ArgumentNullException.ThrowIfNull(added);

            EnsureInitialized();
            Forbidden!.UnionWith(added);
            Forbidden.ExceptWith(removed);
        }

        protected override void EnsureInitialized()
        {
            if (Forbidden is not null)
            {
                return;
            }

            Forbidden = TensorUtils.GetAllIndicesNamesT(Tensor);
        }
    }

    private sealed class TransparentForbiddenContainer : IForbiddenContainer
    {
        public TransparentForbiddenContainer(IForbiddenContainer parent)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        private IForbiddenContainer Parent { get; }

        public HashSet<int> GetForbidden()
        {
            return Parent.GetForbidden();
        }

        public void Submit(HashSet<int> removed, HashSet<int> added)
        {
            Parent.Submit(removed, added);
        }

        public Tensor OnLeaving(StackPosition<IForbiddenContainer> stackPosition)
        {
            _ = stackPosition;
            return null!;
        }
    }

    private sealed class ScalarFunctionForbiddenContainer : IForbiddenContainer
    {
        public HashSet<int> GetForbidden()
        {
            return s_emptyIntSet;
        }

        public void Submit(HashSet<int> removed, HashSet<int> added)
        {
            _ = removed;
            _ = added;
        }

        public Tensor OnLeaving(StackPosition<IForbiddenContainer> stackPosition)
        {
            ArgumentNullException.ThrowIfNull(stackPosition);

            if (!stackPosition.IsModified())
            {
                return null!;
            }

            StackPosition<IForbiddenContainer> previous = stackPosition.Previous();
            if (previous is null)
            {
                return null!;
            }

            Tensor tensor = stackPosition.GetTensor();
            tensor = ApplyIndexMapping.RenameDummy(tensor, [.. previous.GetPayload().GetForbidden()]);
            previous.GetPayload().Submit(s_emptyIntSet, TensorUtils.GetAllIndicesNamesT(tensor));
            return tensor;
        }
    }

    private sealed class EmptyForbiddenContainer : IForbiddenContainer
    {
        public HashSet<int> GetForbidden()
        {
            return s_emptyIntSet;
        }

        public void Submit(HashSet<int> removed, HashSet<int> added)
        {
            _ = removed;
            _ = added;
        }

        public Tensor OnLeaving(StackPosition<IForbiddenContainer> stackPosition)
        {
            _ = stackPosition;
            return null!;
        }
    }

    private static bool IsBitArrayEmpty(BitArray bitArray)
    {
        foreach (bool value in bitArray)
        {
            if (value)
            {
                return false;
            }
        }

        return true;
    }
}
