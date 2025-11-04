using System.Diagnostics;

namespace NRedberry.Core.Combinatorics;

internal sealed class PermutationPriorityTuple
{
    public readonly int[] Permutation;
    public int Priority;

    public PermutationPriorityTuple(int[] permutation)
    {
        Debug.Assert(permutation != null);
        Permutation = permutation;
        Priority = 1;
    }

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;

        if (GetType() != obj.GetType())
            return false;

        var other = (PermutationPriorityTuple)obj;
        return Equals(Permutation, other.Permutation);
    }

    public override int GetHashCode()
    {
        var hash = 3;
        hash = 89 * hash + Permutation.GetHashCode();
        return hash;
    }

    public override string ToString() => string.Join(",", Permutation) + " : " + Priority;
}
