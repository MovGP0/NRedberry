using System.Collections;
using System.Numerics;

namespace NRedberry.Core.Groups;

/// <summary>
/// Skeleton port of cc.redberry.core.groups.permutations.PermutationGroup.
/// </summary>
public sealed class PermutationGroup : IEnumerable<IPermutation>
{
    private readonly IReadOnlyList<IPermutation> generators = [];

    private PermutationGroup(IReadOnlyList<IPermutation> generators)
    {
        throw new NotImplementedException();
    }

    public static PermutationGroup TrivialGroup()
    {
        throw new NotImplementedException();
    }

    public static PermutationGroup CreatePermutationGroup(params IPermutation[] generators)
    {
        throw new NotImplementedException();
    }

    public BigInteger Order => throw new NotImplementedException();

    public int Degree => throw new NotImplementedException();

    public IReadOnlyList<IPermutation> Generators => generators;

    public bool Contains(IPermutation permutation)
    {
        throw new NotImplementedException();
    }

    public IEnumerator<IPermutation> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
