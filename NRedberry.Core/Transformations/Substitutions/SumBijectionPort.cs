using NRedberry.Concurrent;
using NRedberry.Core.Combinatorics;
using NRedberry.IndexMapping;
using NRedberry.Tensors;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Port of cc.redberry.core.transformations.substitutions.SumBijectionPort.
/// </summary>
public sealed class SumBijectionPort : IOutputPort<BijectionContainer>
{
    private readonly List<IMapper>? _mappers;
    private readonly int[]? _bijection;
    private readonly IMapperSource? _source;
    private bool _finished;

    public SumBijectionPort(Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        if (from.Size > to.Size)
        {
            _finished = true;
            return;
        }

        _mappers = [];
        int j = 0;
        int fromBegin = 0;
        int fromSize = from.Size;
        int toSize = to.Size;
        int mainStretchFromCoord = -1;
        int mainStretchFromPointer = -1;
        int mainStretchFromLength = -1;
        int mainStretchToLength = int.MaxValue;
        int mainStretchIndex = -1;
        int hash = from[0].GetHashCode();

        for (int i = 1; i <= fromSize; ++i)
        {
            if (i != fromSize && from[i].GetHashCode() == hash)
            {
                continue;
            }

            for (; j < toSize; ++j)
            {
                if (to[j].GetHashCode() >= hash)
                {
                    break;
                }
            }

            if (j == toSize || to[j].GetHashCode() > hash)
            {
                _finished = true;
                return;
            }

            int toBegin = j;
            for (; j < toSize; ++j)
            {
                if (to[j].GetHashCode() != hash)
                {
                    break;
                }
            }

            int fromLength = i - fromBegin;
            int toLength = j - toBegin;
            if (toLength < fromLength)
            {
                _finished = true;
                return;
            }

            if (toLength == 1)
            {
                _mappers.Add(new SinglePairMapper(from[fromBegin], to[toBegin], toBegin));
            }
            else
            {
                _mappers.Add(new StretchPairMapper(from.GetRange(fromBegin, i), to.GetRange(toBegin, j), toBegin));
            }

            if (toLength < mainStretchToLength)
            {
                mainStretchToLength = toLength;
                mainStretchFromLength = fromLength;
                mainStretchFromCoord = fromBegin;
                mainStretchFromPointer = toBegin;
                mainStretchIndex = _mappers.Count - 1;
            }

            fromBegin = i;
            if (i != fromSize)
            {
                hash = from[i].GetHashCode();
            }
        }

        if (mainStretchToLength == 1)
        {
            _source = new SinglePairSource(from[mainStretchFromCoord], to[mainStretchFromPointer], mainStretchFromPointer);
        }
        else
        {
            _source = new StretchPairSource(
                from.GetRange(mainStretchFromCoord, mainStretchFromCoord + mainStretchFromLength),
                to.GetRange(mainStretchFromPointer, mainStretchFromPointer + mainStretchToLength),
                mainStretchFromPointer);
        }

        _mappers[mainStretchIndex] = _source;
        _bijection = new int[from.Size];
    }

    public BijectionContainer Take()
    {
        if (_finished)
        {
            return null!;
        }

        List<int[]> bijections = [];
        int mapperCount = _mappers!.Count;

        while (true)
        {
            Mapping buffer = _source!.Take();
            if (buffer is null)
            {
                _finished = true;
                return null!;
            }

            bool restart = false;
            for (int i = 0; i < mapperCount; ++i)
            {
                int[]? bijection = _mappers[i].NextMapping(buffer);
                if (bijection is null)
                {
                    for (; i >= 0; --i)
                    {
                        _mappers[i].Reset();
                    }

                    bijections.Clear();
                    restart = true;
                    break;
                }

                bijections.Add(bijection);
            }

            if (restart)
            {
                continue;
            }

            return new BijectionContainer(buffer, Fill(_bijection!, bijections));
        }
    }

    private static int[] Fill(int[] storage, List<int[]> bijections)
    {
        ArgumentNullException.ThrowIfNull(storage);
        ArgumentNullException.ThrowIfNull(bijections);

        int begin = 0;
        for (int i = 0; i < bijections.Count; ++i)
        {
            int[] bijection = bijections[i];
            Array.Copy(bijection, 0, storage, begin, bijection.Length);
            begin += bijection.Length;
        }

        return storage;
    }

    private interface IMapper
    {
        int[]? NextMapping(Mapping buffer);

        void Reset();
    }

    private abstract class AbstractMapper : IMapper
    {
        public int[]? NextMapping(Mapping buffer)
        {
            if (buffer is null)
            {
                return null;
            }

            return NextMappingCore(buffer);
        }

        public virtual void Reset()
        {
        }

        protected abstract int[]? NextMappingCore(Mapping buffer);
    }

    private abstract class AbstractStretchMapper : AbstractMapper
    {
        protected AbstractStretchMapper(Tensor[] from, Tensor[] to, int fromPointer)
        {
            ArgumentNullException.ThrowIfNull(from);
            ArgumentNullException.ThrowIfNull(to);

            From = from;
            To = to;
            FromPointer = fromPointer;
            PermutationGenerator = new IntCombinationPermutationGenerator(to.Length, from.Length);
        }

        protected Tensor[] From { get; }

        protected Tensor[] To { get; }

        protected int FromPointer { get; }

        protected IntCombinationPermutationGenerator PermutationGenerator { get; set; }

        protected int[]? CurrentPermutation { get; set; }

        protected bool Test(Mapping buffer)
        {
            for (int i = 1; i < From.Length; ++i)
            {
                if (!IndexMappings.TestMapping(buffer, From[i], To[CurrentPermutation![i]]))
                {
                    return false;
                }
            }

            return true;
        }
    }

    private interface IMapperSource : IMapper, IOutputPort<Mapping>;

    private sealed class SinglePairSource : AbstractMapper, IMapperSource
    {
        private readonly MappingsPort _mappingsPort;
        private readonly int[] _fromPointer;

        public SinglePairSource(Tensor from, Tensor to, int fromPointer)
        {
            ArgumentNullException.ThrowIfNull(from);
            ArgumentNullException.ThrowIfNull(to);

            _mappingsPort = IndexMappings.CreatePort(from, to);
            _fromPointer = [fromPointer];
        }

        protected override int[] NextMappingCore(Mapping buffer)
        {
            return _fromPointer;
        }

        public Mapping Take()
        {
            return _mappingsPort.Take();
        }
    }

    private sealed class SinglePairMapper : AbstractMapper
    {
        private readonly Tensor _from;
        private readonly Tensor _to;
        private readonly int[] _fromPointer;

        public SinglePairMapper(Tensor from, Tensor to, int fromPointer)
        {
            _from = from ?? throw new ArgumentNullException(nameof(from));
            _to = to ?? throw new ArgumentNullException(nameof(to));
            _fromPointer = [fromPointer];
        }

        protected override int[]? NextMappingCore(Mapping buffer)
        {
            if (!IndexMappings.TestMapping(buffer, _from, _to))
            {
                return null;
            }

            return _fromPointer;
        }
    }

    private sealed class StretchPairSource : AbstractStretchMapper, IMapperSource
    {
        private MappingsPort? _currentSource;

        public StretchPairSource(Tensor[] from, Tensor[] to, int fromPointer)
            : base(from, to, fromPointer)
        {
        }

        protected override int[]? NextMappingCore(Mapping buffer)
        {
            if (!Test(buffer))
            {
                return null;
            }

            int[] mapping = new int[From.Length];
            for (int i = 0; i < From.Length; ++i)
            {
                mapping[i] = FromPointer + CurrentPermutation![i];
            }

            return mapping;
        }

        public Mapping Take()
        {
            while (true)
            {
                if (_currentSource is not null)
                {
                    Mapping mapping = _currentSource.Take();
                    if (mapping is not null)
                    {
                        return mapping;
                    }
                }

                int[]? permutation = PermutationGenerator.Take();
                if (permutation is null)
                {
                    return null!;
                }

                CurrentPermutation = (int[])permutation.Clone();
                _currentSource = IndexMappings.CreatePort(From[0], To[CurrentPermutation[0]]);
            }
        }
    }

    private sealed class StretchPairMapper : AbstractStretchMapper
    {
        public StretchPairMapper(Tensor[] from, Tensor[] to, int fromPointer)
            : base(from, to, fromPointer)
        {
            int[]? permutation = PermutationGenerator.Take();
            CurrentPermutation = permutation is null ? null : (int[])permutation.Clone();
        }

        public override void Reset()
        {
            PermutationGenerator = new IntCombinationPermutationGenerator(To.Length, From.Length);
            int[]? permutation = PermutationGenerator.Take();
            CurrentPermutation = permutation is null ? null : (int[])permutation.Clone();
        }

        protected override int[]? NextMappingCore(Mapping buffer)
        {
            while (true)
            {
                if (CurrentPermutation is null)
                {
                    return null;
                }

                if (!Test(buffer))
                {
                    int[]? permutation = PermutationGenerator.Take();
                    CurrentPermutation = permutation is null ? null : (int[])permutation.Clone();
                    continue;
                }

                int[] bijection = new int[From.Length];
                for (int i = 0; i < From.Length; ++i)
                {
                    bijection[i] = FromPointer + CurrentPermutation[i];
                }

                return bijection;
            }
        }
    }
}
