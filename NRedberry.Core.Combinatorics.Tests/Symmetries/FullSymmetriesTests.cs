using NRedberry.Core.Combinatorics.Symmetries;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests.Symmetries;

public sealed class FullSymmetriesTests
{
    [Fact]
    public void ShouldEnumerateAllDimensionTwoSymmetries()
    {
        FullSymmetries symmetries = new(2);

        List<int[]> visited = symmetries.Select(symmetry => symmetry.OneLine()).ToList();

        symmetries.ShouldSatisfyAllConditions(
            s => s.IsEmpty.ShouldBeFalse(),
            _ => visited.Count.ShouldBe(2),
            _ => visited.Any(permutation => permutation.SequenceEqual([0, 1])).ShouldBeTrue(),
            _ => visited.Any(permutation => permutation.SequenceEqual([1, 0])).ShouldBeTrue());
    }
}
