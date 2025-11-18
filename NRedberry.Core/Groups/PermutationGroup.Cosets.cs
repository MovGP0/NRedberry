using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Groups;

public sealed partial class PermutationGroup
{
    public Permutation[] LeftCosetRepresentatives(PermutationGroup subgroup)
    {
        if (IsTrivial())
        {
            return new[] { Permutations.GetIdentityPermutation() };
        }

        return AlgorithmsBacktrack.LeftCosetRepresentatives(GetBSGS(), subgroup.GetBSGS(), BaseArray(), Ordering());
    }

    public Permutation[] RightCosetRepresentatives(PermutationGroup subgroup)
    {
        Permutation[] reps = LeftCosetRepresentatives(subgroup);
        for (int i = 0; i < reps.Length; ++i)
        {
            reps[i] = reps[i].Inverse();
        }

        return reps;
    }

    public Permutation LeftTransversalOf(PermutationGroup subgroup, Permutation element)
    {
        return AlgorithmsBacktrack.LeftTransversalOf(element, GetBSGS(), subgroup.GetBSGS(), BaseArray(), Ordering());
    }
}
