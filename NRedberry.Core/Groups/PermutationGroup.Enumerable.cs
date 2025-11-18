using System.Collections;
using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Groups;

public sealed partial class PermutationGroup : IEnumerable<Permutation>
{
    public IEnumerator<Permutation> GetEnumerator()
    {
        EnsureBsgsIsInitialized();
        if (_internalDegree == 1)
        {
            return new List<Permutation> { Permutations.GetIdentityPermutation() }.GetEnumerator();
        }

        return new PermIterator(this);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private sealed class PermIterator : IEnumerator<Permutation>
    {
        private readonly PermutationGroup _group;
        private readonly IntTuplesPort _tuplesPort;
        private int[]? _tuple;
        private Permutation? _current;

        public PermIterator(PermutationGroup group)
        {
            _group = group;
            int[] orbitSizes = new int[group._base!.Length];
            for (int i = 0; i < orbitSizes.Length; ++i)
            {
                orbitSizes[i] = group._bsgs![i].OrbitSize;
            }

            _tuplesPort = new IntTuplesPort(orbitSizes);
            _tuple = _tuplesPort.Take();
        }

        public bool MoveNext()
        {
            if (_tuple == null)
            {
                return false;
            }

            Permutation p = _group._bsgs![0].GetInverseTransversalOf(_group._bsgs[0].GetOrbitPoint(_tuple[0]));
            for (int i = 1; i < _group._bsgs.Count; ++i)
            {
                BSGSElement element = _group._bsgs[i];
                p = p.Composition(element.GetInverseTransversalOf(element.GetOrbitPoint(_tuple[i])));
            }

            _current = p;
            _tuple = _tuplesPort.Take();
            return true;
        }

        public void Reset()
        {
            throw new NotSupportedException("Illegal operation.");
        }

        public Permutation Current => _current!;

        object IEnumerator.Current => Current;

        public void Dispose()
        {
        }
    }
}
