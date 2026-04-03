using NRedberry.Core.Combinatorics.Symmetries;
using Xunit;

namespace NRedberry.Core.Combinatorics.Tests.Symmetries;

public sealed class ISymmetriesTests
{
    [Fact]
    public void ShouldExposeSharedInterfaceMembers()
    {
        Core.Combinatorics.Symmetries.Symmetries symmetries = new FullSymmetries(2);

        symmetries.ShouldSatisfyAllConditions(
            s => s.Dimension.ShouldBe(2),
            s => s.IsEmpty.ShouldBeFalse(),
            s => s.BasisSymmetries.ShouldNotBeEmpty());
    }
}
