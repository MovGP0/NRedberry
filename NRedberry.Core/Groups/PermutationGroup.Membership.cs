using NRedberry.Core.Combinatorics;

namespace NRedberry.Groups;

public sealed partial class PermutationGroup
{
    public bool Contains(Permutation permutation)
    {
        return MembershipTest(permutation);
    }

    public bool MembershipTest(Permutation permutation)
    {
        if (IsTrivial())
        {
            return permutation.IsIdentity;
        }

        return AlgorithmsBase.MembershipTest(GetBSGS(), permutation);
    }

    public bool MembershipTest(IEnumerable<Permutation> permutations)
    {
        foreach (Permutation permutation in permutations)
        {
            if (!MembershipTest(permutation))
            {
                return false;
            }
        }

        return true;
    }
}
