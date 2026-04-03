using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsCreateCycleTests
{
    [Fact(DisplayName = "Should throw for negative dimension")]
    public void ShouldThrowForNegativeDimension()
    {
        var exception = Should.Throw<ArgumentException>(() => _ = GroupPermutations.CreateCycle(-1));

        exception.ParamName.ShouldBe("dimension");
    }

    [Fact(DisplayName = "Should return empty cycle for zero dimension")]
    public void ShouldReturnEmptyCycleForZeroDimension()
    {
        int[] cycle = GroupPermutations.CreateCycle(0);

        cycle.ShouldBeEmpty();
    }

    [Fact(DisplayName = "Should return identity mapping for one dimension")]
    public void ShouldReturnIdentityMappingForOneDimension()
    {
        int[] cycle = GroupPermutations.CreateCycle(1);

        cycle.ShouldBe([0]);
    }

    [Fact(DisplayName = "Should create expected one-line mapping for positive dimension")]
    public void ShouldCreateExpectedOneLineMappingForPositiveDimension()
    {
        int[] cycle = GroupPermutations.CreateCycle(5);

        cycle.ShouldBe([4, 0, 1, 2, 3]);
    }

    [Fact(DisplayName = "Should map each position to previous element in cycle")]
    public void ShouldMapEachPositionToPreviousElementInCycle()
    {
        const int dimension = 6;
        int[] cycle = GroupPermutations.CreateCycle(dimension);

        for (int i = 1; i < dimension; i++)
        {
            cycle[i].ShouldBe(i - 1);
        }

        cycle[0].ShouldBe(dimension - 1);
    }
}
