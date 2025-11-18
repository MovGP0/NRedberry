using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Groups;

public sealed partial class PermutationGroup
{
    public Permutation? Mapping(int from, int to)
    {
        if (_positionsInOrbits[from] != _positionsInOrbits[to])
        {
            return null;
        }

        List<BSGSElement> bsgs = GetBSGS();
        if (bsgs[0].BasePoint == from)
        {
            return bsgs[0].GetTransversalOf(to);
        }

        for (int i = 0; i < bsgs.Count; ++i)
        {
            if (bsgs[i].BasePoint == from && bsgs[i].BelongsToOrbit(to))
            {
                return bsgs[i].GetTransversalOf(to);
            }
        }

        List<BSGSCandidateElement> candidates = AlgorithmsBase.AsBSGSCandidatesList(bsgs);
        AlgorithmsBase.ChangeBasePointWithTranspositions(candidates, 0, from);
        return candidates[0].GetTransversalOf(to);
    }

    public BacktrackSearch Mapping(int[] from, int[] to)
    {
        if (from.Length != to.Length)
        {
            throw new ArgumentException("Length of from is not equal to length of to.");
        }

        int[] fromCopy = (int[])from.Clone();
        int[] toCopy = (int[])to.Clone();
        ArraysUtils.QuickSort(fromCopy, toCopy, Ordering());
        List<BSGSCandidateElement> bsgs = GetBSGSCandidate();
        AlgorithmsBacktrack.RebaseWithRedundancy(bsgs, fromCopy, _internalDegree);

        var mapping = new SearchForMapping(fromCopy, toCopy);
        return new BacktrackSearch(AlgorithmsBase.AsBSGSList(bsgs), mapping, mapping);
    }

    private sealed class SearchForMapping : IBacktrackSearchTestFunction, IIndicator<Permutation>
    {
        private readonly int[] _from;
        private readonly int[] _to;

        public SearchForMapping(int[] from, int[] to)
        {
            _from = from;
            _to = to;
        }

        public bool Test(Permutation permutation, int level)
        {
            if (level < _from.Length)
            {
                return permutation.NewIndexOf(_from[level]) == _to[level];
            }

            return true;
        }

        public bool Is(Permutation permutation)
        {
            return true;
        }
    }
}
