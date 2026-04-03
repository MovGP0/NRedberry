using Xunit;
using SymmetriesContract = NRedberry.Core.Combinatorics.Symmetries.Symmetries;

namespace NRedberry.Core.Combinatorics.Tests.Symmetries;

public sealed class AbstractSymmetriesTests
{
    [Fact]
    public void ShouldReturnCopyOfBasisSymmetries()
    {
        TestDummySymmetries symmetries = new(2, new Symmetry([0, 1], false));

        List<Symmetry> copy = symmetries.BasisSymmetries;
        copy.Clear();

        symmetries.ShouldSatisfyAllConditions(
            s => s.BasisSymmetries.ShouldHaveSingleItem(),
            s => s.ShouldBeAssignableTo<SymmetriesContract>().Dimension.ShouldBe(2));
        }
}
