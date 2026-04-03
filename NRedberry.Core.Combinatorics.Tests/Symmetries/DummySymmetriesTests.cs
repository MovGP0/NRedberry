using Xunit;

namespace NRedberry.Core.Combinatorics.Tests.Symmetries;

public sealed class DummySymmetriesTests
{
    [Fact]
    public void ShouldAddUniqueSymmetriesAndCloneAsSelf()
    {
        TestDummySymmetries symmetries = new(2, new Symmetry([0, 1], false));
        Symmetry transposition = new([1, 0], false);

        symmetries.ShouldSatisfyAllConditions(
            s => s.Add(transposition).ShouldBeTrue(),
            s => s.Add(transposition).ShouldBeFalse(),
            s => s.Clone().ShouldBeSameAs(s));
    }
}
