using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;
using NRedberry.Groups;
using Shouldly;
using Xunit;

namespace NRedberry.Core.Tests.Groups;

public sealed class AlgorithmsBacktrackTests
{
    [Fact(DisplayName = "Should throw for empty group in subgroup search")]
    public void ShouldThrowForEmptyGroupInSubgroupSearch()
    {
        // Arrange
        var group = new List<BSGSElement>();
        var subgroup = new List<BSGSCandidateElement>();

        // Act + Assert
        Should.Throw<ArgumentException>(() => AlgorithmsBacktrack.SubgroupSearch(
            group,
            subgroup,
            IBacktrackSearchTestFunction.True,
            new TrueIndicator<Permutation>()));
    }
}
