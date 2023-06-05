using System;
using System.Collections.Generic;
using System.Linq;

namespace NRedberry.Core.Combinatorics
{
    public class PermutationsSpanIterator<T> : IEnumerator<T> where T : Permutation
    {
        private SortedSet<Permutation> set;
        public IEnumerator<Permutation> iterator;
        private List<Permutation> upperLayer;
        private List<Permutation?> lowerLayer;
        private List<Permutation> nextLayer = new();
        private bool forward;
        private int upperIndex, lowerIndex;

        public PermutationsSpanIterator(List<T> permutations)
        {
            set = new SortedSet<Permutation>();
            upperLayer = new List<Permutation>();
            upperLayer.Add(permutations[0].GetOne());
            lowerLayer = permutations.Cast<Permutation?>().ToList();
        }

        private Permutation? current;

        public T? Current => (T?)current;

        object System.Collections.IEnumerator.Current => current;

        public bool MoveNext()
        {
            current = Next1();
            return current != null;
        }

        private Permutation? Next1()
        {
            Permutation? composition = null;
            while (composition == null)
                if (forward)
                {
                    composition = TryPair(upperLayer[upperIndex], lowerLayer[lowerIndex]);
                    NextIndices();
                    if (lowerLayer.Count == 0)
                        break;
                    forward = !forward;
                }
                else
                {
                    composition = TryPair(lowerLayer[lowerIndex], upperLayer[upperIndex]);
                    forward = !forward;
                }
            return composition;
        }

        private void NextIndices()
        {
            if (++upperIndex < upperLayer.Count)
                return;
            upperIndex = 0;
            if (++lowerIndex < lowerLayer.Count)
                return;
            lowerIndex = 0;
            upperLayer = new List<Permutation>(set);
            lowerLayer = nextLayer;
            nextLayer = new List<Permutation>();
        }

        private Permutation? TryPair(Permutation? p0, Permutation? p1)
        {
            var composition = p0.Composition(p1);
            var setComposition = set.FirstOrDefault(x => x.CompareTo(composition) == 0);
            if (setComposition != null && setComposition.Equals(composition))
                return null;
            if (setComposition != null)
                throw new Exception("InconsistentGeneratorsException");
            set.Add(composition);
            nextLayer.Add(composition);
            return composition;
        }

        public void Dispose() { }

        public void Reset() => throw new NotImplementedException();
    }
}
