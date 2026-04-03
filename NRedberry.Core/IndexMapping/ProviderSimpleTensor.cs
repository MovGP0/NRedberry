using NRedberry.Concurrent;
using NRedberry.Core.Combinatorics;
using NRedberry.Indices;
using NRedberry.Tensors;
using static NRedberry.Indices.IndicesUtils;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/ProviderSimpleTensor.java
 */

internal sealed class ProviderSimpleTensor : IndexMappingProviderAbstractFT<SimpleTensor>
{
    public static IIndexMappingProviderFactory SimpleTensorFactory { get; } = new ProviderSimpleTensorFactory();

    public static IIndexMappingProviderFactory TensorFieldFactory { get; } = new ProviderTensorFieldFactory();

    private IPermutationSource? _searchForPermutations;

    private ProviderSimpleTensor(IOutputPort<IIndexMappingBuffer> outputPort, SimpleTensor fromTensor, SimpleTensor toTensor)
        : base(outputPort, fromTensor, toTensor)
    {
    }

    public static IIndexMappingProvider CreateSimpleTensor(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        SimpleTensor fromTensor = (SimpleTensor)from;
        SimpleTensor toTensor = (SimpleTensor)to;
        if (fromTensor.Name != toTensor.Name)
        {
            return IndexMappingProviderUtil.EmptyProvider;
        }

        if (from.Indices.Size() == 0)
        {
            return new DummyIndexMappingProvider(provider);
        }

        return new ProviderSimpleTensor(provider, fromTensor, toTensor);
    }

    public static IIndexMappingProvider CreateTensorField(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        TensorField fromField = (TensorField)from;
        TensorField toField = (TensorField)to;
        if (fromField.Name != toField.Name)
        {
            return IndexMappingProviderUtil.EmptyProvider;
        }

        for (int i = 0; i < from.Size; ++i)
        {
            if (!IndexMappings.PositiveMappingExists(from[i], to[i]))
            {
                return IndexMappingProviderUtil.EmptyProvider;
            }
        }

        return new ProviderSimpleTensor(provider, fromField, toField);
    }

    public override IIndexMappingBuffer? Take()
    {
        if (currentBuffer is null)
        {
            return null;
        }

        SimpleIndices fromIndices = From.SimpleIndices;
        SimpleIndices toIndices = To.SimpleIndices;
        int size = fromIndices.Size();
        if (size == 0)
        {
            IIndexMappingBuffer result = currentBuffer;
            currentBuffer = null;
            return result;
        }

        if (_searchForPermutations is not null)
        {
            Permutation? permutation;
            while ((permutation = _searchForPermutations.Take()) is not null)
            {
                IIndexMappingBuffer tempBuffer = (IIndexMappingBuffer)currentBuffer.Clone();
                bool consistent = true;
                for (int i = 0; i < size; ++i)
                {
                    if (!tempBuffer.TryMap(fromIndices[i], toIndices[permutation.NewIndexOf(i)]))
                    {
                        consistent = false;
                        break;
                    }
                }

                if (!consistent)
                {
                    continue;
                }

                tempBuffer.AddSign(permutation.IsAntisymmetry);
                return tempBuffer;
            }

            _searchForPermutations = null;
            currentBuffer = null;
            return null;
        }

        if (fromIndices.Size() == 1 || fromIndices.Symmetries.IsTrivial())
        {
            IIndexMappingBuffer tempBuffer = currentBuffer;
            for (int i = 0; i < size; ++i)
            {
                if (!tempBuffer.TryMap(fromIndices[i], toIndices[i]))
                {
                    currentBuffer = null;
                    return null;
                }
            }

            currentBuffer = null;
            return tempBuffer;
        }

        List<int>? permutationMappingFrom = null;
        List<int>? permutationMappingTo = null;
        for (int mapFrom = 0; mapFrom < size; ++mapFrom)
        {
            int fromIndex = fromIndices[mapFrom];
            if (!currentBuffer.GetMap().TryGetValue(GetNameWithType(fromIndex), out IndexMappingBufferRecord? record))
            {
                continue;
            }

            if (GetRawStateInt(fromIndex) == record.GetFromRawState())
            {
                currentBuffer = null;
                return null;
            }

            int toIndex = InverseIndexState(SetRawState(record.GetToRawState(), record.GetIndexName()));
            bool found = false;
            for (int mapTo = 0; mapTo < size; ++mapTo)
            {
                if (toIndices[mapTo] != toIndex)
                {
                    continue;
                }

                permutationMappingFrom ??= [];
                permutationMappingTo ??= [];
                permutationMappingFrom.Add(mapFrom);
                permutationMappingTo.Add(mapTo);
                found = true;
                break;
            }

            if (!found)
            {
                currentBuffer = null;
                return null;
            }
        }

        if (permutationMappingFrom is null)
        {
            _searchForPermutations = new EnumerablePermutationSource(fromIndices.Symmetries.PermutationGroup.GetEnumerator());
        }
        else
        {
            _searchForPermutations = new OutputPortPermutationSource(
                fromIndices.Symmetries.PermutationGroup.Mapping(
                    permutationMappingFrom.ToArray(),
                    permutationMappingTo!.ToArray()));
        }

        return Take();
    }

    protected override void BeforeTick()
    {
        _searchForPermutations = null;
        currentBuffer = null;
    }

    private interface IPermutationSource
    {
        Permutation? Take();
    }

    private sealed class EnumerablePermutationSource(IEnumerator<Permutation> enumerator) : IPermutationSource
    {
        private readonly IEnumerator<Permutation> _enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));

        public Permutation? Take()
        {
            return _enumerator.MoveNext() ? _enumerator.Current : null;
        }
    }

    private sealed class OutputPortPermutationSource(IOutputPort<Permutation> outputPort) : IPermutationSource
    {
        private readonly IOutputPort<Permutation> _outputPort = outputPort ?? throw new ArgumentNullException(nameof(outputPort));

        public Permutation? Take()
        {
            return _outputPort.Take();
        }
    }
}

internal sealed class ProviderSimpleTensorFactory : IIndexMappingProviderFactory
{
    public IIndexMappingProvider Create(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        return ProviderSimpleTensor.CreateSimpleTensor(provider, from, to);
    }
}

internal sealed class ProviderTensorFieldFactory : IIndexMappingProviderFactory
{
    public IIndexMappingProvider Create(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        return ProviderSimpleTensor.CreateTensorField(provider, from, to);
    }
}
