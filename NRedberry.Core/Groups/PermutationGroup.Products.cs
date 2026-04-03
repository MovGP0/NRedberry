using NRedberry.Core.Combinatorics;

namespace NRedberry.Groups;

public sealed partial class PermutationGroup
{
    public PermutationGroup DirectProduct(PermutationGroup group)
    {
        List<Permutation> generators = new(Generators.Count + group.Generators.Count);
        generators.AddRange(Generators);

        foreach (Permutation permutation in group.Generators)
        {
            generators.Add(permutation.MoveRight(Degree));
        }

        return CreatePermutationGroup(generators);
    }
}
