using NRedberry.Concurrent;
using NRedberry.Core.Combinatorics;
using NRedberry.Tensors;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/ProviderSum.java
 */

internal sealed class ProviderSum : IIndexMappingProvider
{
    public static IIndexMappingProviderFactory Factory { get; } = new ProviderSumFactory();

    private readonly IIndexMappingProvider _mainProvider;
    private readonly ITester[] _testers;

    internal ProviderSum(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        int size = from.Size;
        if (size == 0)
        {
            _mainProvider = new DummyIndexMappingProvider(provider);
            _testers = [];
            return;
        }

        List<ITester> testers = [];
        int mainStretchCoord = -1;
        int mainStretchLength = int.MaxValue;
        int mainStretchIndex = -1;
        int begin = 0;

        for (int i = 1; i <= size; ++i)
        {
            if (i != size && from[i].GetHashCode() == from[i - 1].GetHashCode())
            {
                continue;
            }

            int length = i - begin;
            ITester tester = length == 1
                ? new SinglePairTester(from[begin], to[begin])
                : new StretchPairTester(from.GetRange(begin, i), to.GetRange(begin, i));

            testers.Add(tester);
            if (length < mainStretchLength)
            {
                mainStretchCoord = begin;
                mainStretchLength = length;
                mainStretchIndex = testers.Count - 1;
            }

            begin = i;
        }

        if (mainStretchLength == 1)
        {
            _mainProvider = IndexMappings.CreatePort(provider, from[mainStretchCoord], to[mainStretchCoord]);
        }
        else
        {
            _mainProvider = new StretchPairSource(
                provider,
                from.GetRange(mainStretchCoord, mainStretchCoord + mainStretchLength),
                to.GetRange(mainStretchCoord, mainStretchCoord + mainStretchLength));
        }

        testers.RemoveAt(mainStretchIndex);
        _testers = new ITester[testers.Count];
        for (int i = 0; i < testers.Count; ++i)
        {
            _testers[i] = testers[i];
        }
    }

    public bool Tick()
    {
        return _mainProvider.Tick();
    }

    public IIndexMappingBuffer Take()
    {
        while (true)
        {
            IIndexMappingBuffer buffer = _mainProvider.Take();
            if (buffer is null)
            {
                return null!;
            }

            buffer.RemoveContracted();

            bool accepted = true;
            foreach (ITester tester in _testers)
            {
                if (tester.Test(buffer))
                {
                    continue;
                }

                accepted = false;
                break;
            }

            if (accepted)
            {
                return buffer;
            }
        }
    }

    private static bool TestBuffer(IIndexMappingBuffer buffer, Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(buffer);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        IIndexMappingBuffer cloned = (IIndexMappingBuffer)buffer.Clone();
        return IndexMappings.CreatePortOfBuffers(cloned, from, to).Take() is not null;
    }

    private interface ITester
    {
        bool Test(IIndexMappingBuffer buffer);
    }

    private sealed class StretchPairSource : IndexMappingProviderAbstract
    {
        private readonly Tensor[] _from;
        private readonly Tensor[] _to;
        private readonly IntPermutationsGenerator _permutationGenerator;
        private IOutputPort<IIndexMappingBuffer>? _currentSource;

        public StretchPairSource(IOutputPort<IIndexMappingBuffer> outputPort, Tensor[] from, Tensor[] to)
            : base(outputPort)
        {
            ArgumentNullException.ThrowIfNull(from);
            ArgumentNullException.ThrowIfNull(to);

            _from = from;
            _to = to;
            _permutationGenerator = new IntPermutationsGenerator(from.Length);
        }

        public override IIndexMappingBuffer? Take()
        {
            if (currentBuffer is null)
            {
                return null;
            }

            while (true)
            {
                if (_currentSource?.Take() is IIndexMappingBuffer buffer)
                {
                    return buffer;
                }

                int[]? permutation = _permutationGenerator.Take();
                if (permutation is null)
                {
                    currentBuffer = null;
                    return null;
                }

                Tensor[] permutedTo = new Tensor[_to.Length];
                for (int i = 0; i < permutation.Length; ++i)
                {
                    permutedTo[i] = _to[permutation[i]];
                }

                _currentSource = new SimpleProductMappingsPort(
                    IndexMappingProviderUtil.Singleton((IIndexMappingBuffer)currentBuffer.Clone()),
                    _from,
                    permutedTo);
            }
        }

        protected override void BeforeTick()
        {
            base.BeforeTick();
            _permutationGenerator.Reset();
            _currentSource = null;
        }
    }

    private sealed class StretchPairTester : ITester
    {
        private readonly Tensor[] _from;
        private readonly Tensor[] _to;
        private readonly int _length;

        public StretchPairTester(Tensor[] from, Tensor[] to)
        {
            ArgumentNullException.ThrowIfNull(from);
            ArgumentNullException.ThrowIfNull(to);

            _from = from;
            _to = to;
            _length = from.Length;
        }

        public bool Test(IIndexMappingBuffer buffer)
        {
            bool[] bijection = new bool[_length];
            for (int i = 0; i < _length; ++i)
            {
                for (int j = 0; j < _length; ++j)
                {
                    if (bijection[j] || !TestBuffer(buffer, _from[j], _to[i]))
                    {
                        continue;
                    }

                    bijection[j] = true;
                    goto ContinueOuter;
                }

                return false;

            ContinueOuter:
                continue;
            }

            return true;
        }
    }

    private sealed class SinglePairTester : ITester
    {
        private readonly Tensor _from;
        private readonly Tensor _to;

        public SinglePairTester(Tensor from, Tensor to)
        {
            ArgumentNullException.ThrowIfNull(from);
            ArgumentNullException.ThrowIfNull(to);

            _from = from;
            _to = to;
        }

        public bool Test(IIndexMappingBuffer buffer)
        {
            return TestBuffer(buffer, _from, _to);
        }
    }
}

internal sealed class ProviderSumFactory : IIndexMappingProviderFactory
{
    public IIndexMappingProvider Create(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        if (from.Size != to.Size)
        {
            return IndexMappingProviderUtil.EmptyProvider;
        }

        for (int i = 0; i < from.Size; ++i)
        {
            if (from[i].GetHashCode() != to[i].GetHashCode())
            {
                return IndexMappingProviderUtil.EmptyProvider;
            }
        }

        return new ProviderSum(provider, from, to);
    }
}
