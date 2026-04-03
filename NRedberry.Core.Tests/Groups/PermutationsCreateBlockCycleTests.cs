using GroupPermutations = NRedberry.Groups.Permutations;

namespace NRedberry.Core.Tests.Groups;

public sealed class PermutationsCreateBlockCycleTests
{
    [Fact(DisplayName = "Should create expected block cycle for 3x4")]
    public void ShouldCreateExpectedBlockCycleForThreeByFour()
    {
        int[] cycle = GroupPermutations.CreateBlockCycle(3, 4);

        cycle.ShouldBe([3, 4, 5, 6, 7, 8, 9, 10, 11, 0, 1, 2]);
    }

    [Fact(DisplayName = "Should rotate each position by block size")]
    public void ShouldRotateEachPositionByBlockSize()
    {
        const int blockSize = 2;
        const int numberOfBlocks = 5;
        int[] cycle = GroupPermutations.CreateBlockCycle(blockSize, numberOfBlocks);
        const int degree = blockSize * numberOfBlocks;

        for (int i = 0; i < degree; i++)
        {
            cycle[i].ShouldBe((i + blockSize) % degree);
        }
    }

    [Fact(DisplayName = "Should create identity mapping for a single block")]
    public void ShouldCreateIdentityMappingForSingleBlock()
    {
        int[] cycle = GroupPermutations.CreateBlockCycle(5, 1);

        cycle.ShouldBe([0, 1, 2, 3, 4]);
    }

    [Fact(DisplayName = "Should return empty mapping when block size is zero")]
    public void ShouldReturnEmptyMappingWhenBlockSizeIsZero()
    {
        GroupPermutations.CreateBlockCycle(0, 4).ShouldBeEmpty();
        GroupPermutations.CreateBlockCycle(0, 0).ShouldBeEmpty();
    }

    [Fact(DisplayName = "Should throw when number of blocks is zero and block size is positive")]
    public void ShouldThrowWhenNumberOfBlocksIsZeroAndBlockSizeIsPositive()
    {
        Should.Throw<IndexOutOfRangeException>(() => _ = GroupPermutations.CreateBlockCycle(3, 0));
    }

    [Fact(DisplayName = "Should throw for negative block size or count")]
    public void ShouldThrowForNegativeBlockSizeOrCount()
    {
        Should.Throw<ArgumentException>(() => _ = GroupPermutations.CreateBlockCycle(-1, 3));
        Should.Throw<ArgumentException>(() => _ = GroupPermutations.CreateBlockCycle(3, -1));
        Should.Throw<ArgumentException>(() => _ = GroupPermutations.CreateBlockCycle(-1, -1));
    }

    [Fact(DisplayName = "Should produce a valid one-line permutation for upstream 3x4 scenario")]
    public void ShouldProduceAValidOneLinePermutationForUpstreamScenario()
    {
        int[] cycle = GroupPermutations.CreateBlockCycle(3, 4);

        GroupPermutations.TestPermutationCorrectness(cycle).ShouldBeTrue();
    }
}
