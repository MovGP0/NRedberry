using System.Collections;
using System.Numerics;
using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Groups;

/// <summary>
/// Skeleton port of cc.redberry.core.groups.permutations.PermutationGroup.
/// </summary>
public sealed class PermutationGroup : IEnumerable<Permutation>
{
    private PermutationGroup(IReadOnlyList<Permutation> generators)
    {
        throw new NotImplementedException();
    }

    public static PermutationGroup TrivialGroup()
    {
        throw new NotImplementedException();
    }

    public static PermutationGroup CreatePermutationGroup(params Permutation[] generators)
    {
        throw new NotImplementedException();
    }

    public BigInteger Order => throw new NotImplementedException();

    public int Degree => throw new NotImplementedException();

    public IReadOnlyList<Permutation> Generators { get; } = [];

    public bool Contains(Permutation permutation)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<Permutation> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
