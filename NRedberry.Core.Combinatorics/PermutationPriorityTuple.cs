using System.Collections;

namespace NRedberry.Core.Combinatorics;

internal sealed class PermutationPriorityTuple
{
    public readonly int[] Permutation;
    public int Priority;

    public PermutationPriorityTuple(int[] permutation)
    {
        ArgumentNullException.ThrowIfNull(permutation);
        Permutation = permutation;
        Priority = 1;
    }

    public override bool Equals(object? obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (GetType() != obj.GetType())
        {
            return false;
        }

        var other = (PermutationPriorityTuple)obj;
        return StructuralComparisons.StructuralEqualityComparer.Equals(Permutation, other.Permutation);
    }

    public override int GetHashCode()
    {
        var hash = 3;
        hash = 89 * hash + StructuralComparisons.StructuralEqualityComparer.GetHashCode(Permutation);
        return hash;
    }

    public override string ToString()
    {
        return $"[{string.Join(", ", Permutation)}] : {Priority}";
    }
}
