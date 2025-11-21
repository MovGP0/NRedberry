using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Groups;

public sealed partial class PermutationGroup
{
    public PermutationGroup PointwiseStabilizer(params int[] set)
    {
        if (IsTrivial())
        {
            return this;
        }

        if (set.Length == 0)
        {
            return this;
        }

        set = MathUtils.GetSortedDistinct(set.ToArray());

        ArraysUtils.QuickSort(set, Ordering());

        List<BSGSCandidateElement> bsgs = GetBSGSCandidate();
        AlgorithmsBase.Rebase(bsgs, set);

        if (bsgs.Count <= set.Length)
        {
            return TrivialGroupInstance;
        }

        return CreatePermutationGroupFromBSGS(AlgorithmsBase.AsBSGSList(bsgs.GetRange(set.Length, bsgs.Count - set.Length)));
    }

    public PermutationGroup PointwiseStabilizerRestricted(params int[] set)
    {
        if (IsTrivial())
        {
            return this;
        }

        if (set.Length == 0)
        {
            return this;
        }

        set = MathUtils.GetSortedDistinct(set);
        int newDegree = _internalDegree - set.Length;
        int[] newBase = set.ToArray();
        ArraysUtils.QuickSort(newBase, Ordering());

        List<BSGSCandidateElement> bsgs = GetBSGSCandidate();
        AlgorithmsBase.Rebase(bsgs, newBase);

        if (bsgs.Count <= newBase.Length)
        {
            return TrivialGroupInstance;
        }

        int[] closure = new int[newDegree];
        int[] mapping = new int[_internalDegree];
        Array.Fill(mapping, -1);
        int pointer = 0;
        int counter = 0;
        for (int i = 0; i < _internalDegree; ++i)
        {
            if (pointer < set.Length && i == set[pointer])
            {
                ++pointer;
                continue;
            }

            closure[counter] = i;
            mapping[i] = counter;
            ++counter;
        }

        var stab = new List<BSGSCandidateElement>();
        for (int i = newBase.Length; i < bsgs.Count; ++i)
        {
            BSGSCandidateElement element = bsgs[i];
            if (mapping[element.BasePoint] == -1)
            {
                continue;
            }

            var newStabs = new List<Permutation>(element.StabilizerGenerators.Count);
            foreach (Permutation permutation in element.StabilizerGenerators)
            {
                int[] perm = new int[newDegree];
                for (int j = 0; j < newDegree; ++j)
                {
                    perm[j] = mapping[permutation.NewIndexOf(closure[j])];
                }

                newStabs.Add(Permutations.CreatePermutation(permutation.Antisymmetry(), perm));
            }

            stab.Add(new BSGSCandidateElement(mapping[element.BasePoint], newStabs, newDegree));
        }

        return CreatePermutationGroupFromBSGS(AlgorithmsBase.AsBSGSList(stab));
    }

    public PermutationGroup SetwiseStabilizer(params int[] set)
    {
        if (set.Length == 0)
        {
            return this;
        }

        set = MathUtils.GetSortedDistinct(set.ToArray());
        ArraysUtils.QuickSort(set, Ordering());
        List<BSGSCandidateElement> bsgs = GetBSGSCandidate();
        AlgorithmsBase.Rebase(bsgs, set);

        Array.Sort(set);
        for (int i = bsgs.Count - 1; i >= set.Length; --i)
        {
            if (Array.BinarySearch(set, bsgs[i].BasePoint) >= 0)
            {
                bsgs.RemoveAt(i);
            }
        }

        var stabilizer = AlgorithmsBase.Clone(new List<BSGSCandidateElement>(bsgs.GetRange(set.Length, bsgs.Count - set.Length)));
        int[] newBase = AlgorithmsBase.GetBaseAsArray(bsgs);
        var swTest = new SetwiseStabilizerSearchTest(newBase, set);
        AlgorithmsBacktrack.SubgroupSearchWithPayload(
            bsgs.OfType<BSGSElement>().ToList(),
            stabilizer,
            BacktrackSearchPayload.CreateDefaultPayload(swTest),
            swTest,
            newBase,
            new InducedOrdering(newBase));

        return CreatePermutationGroupFromBSGS(AlgorithmsBase.AsBSGSList(stabilizer));
    }

    private sealed class SetwiseStabilizerSearchTest(int[] @base, int[] set)
        : IBacktrackSearchTestFunction, IIndicator<Permutation>
    {
        public bool Test(Permutation permutation, int level)
        {
            if (level < set.Length)
            {
                return Array.BinarySearch(set, permutation.NewIndexOf(@base[level])) >= 0;
            }

            return Array.BinarySearch(set, permutation.NewIndexOf(@base[level])) < 0;
        }

        public bool Is(Permutation permutation)
        {
            foreach (int s in set)
            {
                if (Array.BinarySearch(set, permutation.NewIndexOf(s)) < 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
