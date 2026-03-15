using NRedberry.Concurrent;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;
using NRedberry.Core.Utils.Stretces;
using NRedberry.Graphs;
using NRedberry.Tensors;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Port of cc.redberry.core.transformations.substitutions.ProductsBijectionsPort.
/// </summary>
public sealed class ProductsBijectionsPort : IOutputPort<int[]>
{
    private readonly Tensor[] _fromData;
    private readonly Tensor[] _targetData;
    private readonly int[] _seeds;
    private readonly StructureOfContractions _targetContractionsStructure;
    private readonly StructureOfContractions _fromContractionsStructure;
    private readonly long[][] _fromContractions;
    private readonly long[][] _targetContractions;
    private readonly SeedPlanter _planter;
    private InnerPort? _innerPort;

    public ProductsBijectionsPort(ProductContent fromContent, ProductContent targetContent)
    {
        ArgumentNullException.ThrowIfNull(fromContent);
        ArgumentNullException.ThrowIfNull(targetContent);

        _targetContractionsStructure = targetContent.StructureOfContractions;
        _fromContractionsStructure = fromContent.StructureOfContractions;

        _fromContractions = _fromContractionsStructure.contractions;
        _targetContractions = _targetContractionsStructure.contractions;

        int[] seeds = new int[_fromContractionsStructure.componentCount];
        Array.Fill(seeds, -1);
        for (int i = 0; i < _fromContractionsStructure.components.Length; ++i)
        {
            if (seeds[_fromContractionsStructure.components[i]] == -1)
            {
                seeds[_fromContractionsStructure.components[i]] = i;
            }
        }

        _seeds = seeds;
        _fromData = fromContent.GetRange(0, fromContent.Size);
        _targetData = targetContent.GetRange(0, targetContent.Size);
        _planter = new SeedPlanter(this);
    }

    public int[] Take()
    {
        while (true)
        {
            if (_innerPort is not null)
            {
                int[] bijection = _innerPort.Take();
                if (bijection is not null)
                {
                    return bijection;
                }

                _innerPort = null;
            }

            int[] seedsInTarget = _planter.Next();
            if (seedsInTarget is null)
            {
                return null;
            }

            int[] bijection1 = new int[_fromData.Length];
            Array.Fill(bijection1, -1);
            for (int i = 0; i < seedsInTarget.Length; ++i)
            {
                bijection1[_seeds[i]] = seedsInTarget[i];
            }

            _innerPort = new InnerPort(this, bijection1, _seeds);
        }
    }

    private static bool WeakMatch(Tensor first, Tensor second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        if (first.GetHashCode() != second.GetHashCode())
        {
            return false;
        }

        if (first.GetType() != second.GetType())
        {
            return false;
        }

        if (first.Indices.GetType() != second.Indices.GetType())
        {
            return false;
        }

        return first.Indices.Size() == second.Indices.Size();
    }

    private static bool AlreadyContains(int[] bijection, int value)
    {
        foreach (int index in bijection)
        {
            if (index == value)
            {
                return true;
            }
        }

        return false;
    }

    private sealed class InnerPort : IOutputPort<int[]>
    {
        private readonly ProductsBijectionsPort _owner;
        private readonly int[] _bijection;
        private readonly int[] _seeds;
        private bool _closed;
        private PermutationInfo? _lastInfo;
        private PermutationInfo? _firstInfo;
        private List<int> _addedBijections = [];
        private InnerPort? _innerPort;

        public InnerPort(ProductsBijectionsPort owner, int[] bijection, int[] seeds)
        {
            ArgumentNullException.ThrowIfNull(owner);
            ArgumentNullException.ThrowIfNull(bijection);
            ArgumentNullException.ThrowIfNull(seeds);

            _owner = owner;
            _bijection = bijection;
            _seeds = seeds;
            Initialize();
        }

        public int[] Take()
        {
            if (_closed)
            {
                return null;
            }

            while (true)
            {
                if (_innerPort is not null)
                {
                    int[] nestedBijection = _innerPort.Take();
                    if (nestedBijection is not null)
                    {
                        return nestedBijection;
                    }

                    if (_lastInfo is null)
                    {
                        _closed = true;
                        return null;
                    }

                    _innerPort = null;
                }

                if (_lastInfo is null)
                {
                    if (_addedBijections.Count == 0)
                    {
                        _closed = true;
                        return _bijection;
                    }

                    _innerPort = new InnerPort(_owner, _bijection, [.. _addedBijections]);
                    continue;
                }

                if (!_lastInfo.Next())
                {
                    _closed = true;
                    return null;
                }

                int[] bijection = (int[])_bijection.Clone();
                List<int> addedBijections = [.. _addedBijections];

                if (!TryAdvancePermutationChain(bijection, addedBijections))
                {
                    continue;
                }

                _innerPort = new InnerPort(_owner, bijection, [.. addedBijections]);
            }
        }

        private bool TryAdvancePermutationChain(int[] bijection, List<int> addedBijections)
        {
            PermutationInfo? currentInfo = _firstInfo;
            while (currentInfo is not null)
            {
                for (int j = 0; j < currentInfo.FromContractions.Length; ++j)
                {
                    long fromContraction = currentInfo.FromContractions[j];
                    long targetContraction = currentInfo.TargetContractions[currentInfo.Permutation[j]];

                    if (StructureOfContractions.GetFromIndexId(fromContraction)
                        != StructureOfContractions.GetFromIndexId(targetContraction))
                    {
                        throw new InvalidOperationException("Mismatched contraction ids in permutation chain.");
                    }

                    int fromTensorIndex = StructureOfContractions.GetToTensorIndex(fromContraction);
                    int targetTensorIndex = StructureOfContractions.GetToTensorIndex(targetContraction);

                    if (StructureOfContractions.GetToIndexId(fromContraction)
                        != StructureOfContractions.GetToIndexId(targetContraction))
                    {
                        return FailCurrentPermutation(currentInfo);
                    }

                    if (targetTensorIndex == -1)
                    {
                        return FailCurrentPermutation(currentInfo);
                    }

                    if (!WeakMatch(_owner._fromData[fromTensorIndex], _owner._targetData[targetTensorIndex]))
                    {
                        return FailCurrentPermutation(currentInfo);
                    }

                    if (bijection[fromTensorIndex] == -1)
                    {
                        if (AlreadyContains(bijection, targetTensorIndex))
                        {
                            return FailCurrentPermutation(currentInfo);
                        }

                        bijection[fromTensorIndex] = targetTensorIndex;
                        addedBijections.Add(fromTensorIndex);
                    }
                    else if (bijection[fromTensorIndex] != targetTensorIndex)
                    {
                        return FailCurrentPermutation(currentInfo);
                    }
                }

                currentInfo = currentInfo.NextInfo;
            }

            return true;
        }

        private bool FailCurrentPermutation(PermutationInfo currentInfo)
        {
            if (!currentInfo.NextAndResetRightChain())
            {
                _closed = true;
            }

            return false;
        }

        private void Initialize()
        {
            PermutationInfo? previousInfo = null;
            List<int> addedBijections = [];

            for (int i = 0; i < _seeds.Length; ++i)
            {
                int seedFromIndex = _seeds[i];
                Tensor seedFrom = _owner._fromData[seedFromIndex];
                int seedTargetIndex = _bijection[seedFromIndex];

                short[] diffIds = (short[])seedFrom.Indices.GetDiffIds().Clone();
                int[] diffIdsPermutation = CreatePermutation(diffIds.Length);
                ArraysUtils.QuickSort(diffIds, diffIdsPermutation);

                foreach (Stretch stretch in new StretchIteratorS(diffIds))
                {
                    if (stretch.Length == 1)
                    {
                        long fromIndexContraction = _owner._fromContractions[seedFromIndex][diffIdsPermutation[stretch.From]];
                        long targetIndexContraction = _owner._targetContractions[seedTargetIndex][diffIdsPermutation[stretch.From]];

                        int fromTensorIndex = StructureOfContractions.GetToTensorIndex(fromIndexContraction);
                        if (fromTensorIndex == -1)
                        {
                            continue;
                        }

                        if (StructureOfContractions.GetToIndexId(fromIndexContraction)
                            != StructureOfContractions.GetToIndexId(targetIndexContraction))
                        {
                            _closed = true;
                            return;
                        }

                        int targetTensorIndex = StructureOfContractions.GetToTensorIndex(targetIndexContraction);
                        if (targetTensorIndex == -1)
                        {
                            _closed = true;
                            return;
                        }

                        if (!WeakMatch(_owner._fromData[fromTensorIndex], _owner._targetData[targetTensorIndex]))
                        {
                            _closed = true;
                            return;
                        }

                        if (_bijection[fromTensorIndex] == -1)
                        {
                            if (AlreadyContains(_bijection, targetTensorIndex))
                            {
                                _closed = true;
                                return;
                            }

                            _bijection[fromTensorIndex] = targetTensorIndex;
                            addedBijections.Add(fromTensorIndex);
                        }
                        else if (_bijection[fromTensorIndex] != targetTensorIndex)
                        {
                            _closed = true;
                            return;
                        }
                    }
                    else
                    {
                        int count = 0;
                        for (int j = 0; j < stretch.Length; ++j)
                        {
                            long contraction =
                                _owner._fromContractions[seedFromIndex][diffIdsPermutation[stretch.From + j]];
                            if (StructureOfContractions.GetToTensorIndex(contraction) != -1)
                            {
                                ++count;
                            }
                        }

                        long[] fromContractions = new long[count];
                        long[] targetContractions = new long[stretch.Length];
                        count = 0;
                        for (int j = 0; j < stretch.Length; ++j)
                        {
                            long contraction = _owner._fromContractions[seedFromIndex][diffIdsPermutation[stretch.From + j]];
                            if (StructureOfContractions.GetToTensorIndex(contraction) != -1)
                            {
                                fromContractions[count++] = contraction;
                            }

                            targetContractions[j] =
                                _owner._targetContractions[seedTargetIndex][diffIdsPermutation[stretch.From + j]];
                        }

                        previousInfo = new PermutationInfo(previousInfo, fromContractions, targetContractions);
                        _firstInfo ??= previousInfo;
                    }
                }
            }

            _addedBijections = addedBijections;
            _lastInfo = previousInfo;
        }

        private static int[] CreatePermutation(int length)
        {
            int[] permutation = new int[length];
            for (int i = 1; i < length; ++i)
            {
                permutation[i] = i;
            }

            return permutation;
        }
    }

    private sealed class PermutationInfo
    {
        private readonly PermutationInfo? _previous;
        private readonly IIntCombinatorialPort _generator;

        public PermutationInfo(PermutationInfo? previous, long[] fromContractions, long[] targetContractions)
        {
            ArgumentNullException.ThrowIfNull(fromContractions);
            ArgumentNullException.ThrowIfNull(targetContractions);

            _previous = previous;
            FromContractions = fromContractions;
            TargetContractions = targetContractions;
            _generator = (IIntCombinatorialPort)Combinatorics.CreateIntGenerator(
                targetContractions.Length,
                fromContractions.Length);
            Permutation = _generator.GetReference();
            if (previous is not null)
            {
                _ = previous._generator.Take();
                previous.NextInfo = this;
            }
        }

        public PermutationInfo? NextInfo { get; private set; }

        public long[] FromContractions { get; }

        public long[] TargetContractions { get; }

        public int[] Permutation { get; }

        public bool Next()
        {
            if (_generator.Take() is null)
            {
                _generator.Reset();
                _ = _generator.Take();
                if (_previous is not null)
                {
                    return _previous.Next();
                }

                return false;
            }

            return true;
        }

        public bool NextAndResetRightChain()
        {
            if (NextInfo is null)
            {
                return true;
            }

            if (!Next())
            {
                return false;
            }

            PermutationInfo current = this;
            while ((current = current.NextInfo!).NextInfo is not null)
            {
                current._generator.Reset();
                _ = current._generator.Take();
            }

            current._generator.Reset();
            return true;
        }
    }

    private sealed class SeedPlanter
    {
        private readonly IntDistinctTuplesPort _combinationsPort;

        public SeedPlanter(ProductsBijectionsPort owner)
        {
            ArgumentNullException.ThrowIfNull(owner);

            int[][] hits = new int[owner._seeds.Length][];
            for (int seedIndex = 0; seedIndex < owner._seeds.Length; ++seedIndex)
            {
                List<int> hitList = [];
                for (int i = 0; i < owner._targetData.Length; ++i)
                {
                    if (WeakMatch(owner._fromData[owner._seeds[seedIndex]], owner._targetData[i])
                        && GraphUtils.ComponentSize(owner._seeds[seedIndex], owner._fromContractionsStructure.components)
                        <= GraphUtils.ComponentSize(i, owner._targetContractionsStructure.components))
                    {
                        hitList.Add(i);
                    }
                }

                hits[seedIndex] = [.. hitList];
            }

            _combinationsPort = new IntDistinctTuplesPort(hits);
        }

        public int[] Next()
        {
            return _combinationsPort.Take();
        }
    }
}
