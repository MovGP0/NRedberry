using NRedberry.Core.Combinatorics;
using NRedberry.Groups;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationGroupEnumerableTests
{
    [Fact(DisplayName = "Should enumerate all elements of symmetric group")]
    public void ShouldEnumerateAllElementsOfSymmetricGroup()
    {
        PermutationGroup group = PermutationGroup.SymmetricGroup(3);

        List<Permutation> elements = group.ToList();

        elements.Count.ShouldBe((int)group.Order);
    }

    [Fact(DisplayName = "Should support non generic enumeration for symmetric group")]
    public void ShouldSupportNonGenericEnumerationForSymmetricGroup()
    {
        IEnumerable<Permutation> group = PermutationGroup.SymmetricGroup(3);

        group.Count().ShouldBe(6);
    }
}
