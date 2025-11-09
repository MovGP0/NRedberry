namespace NRedberry.Core.Groups;

public static partial class Permutations
{
    public static int[] CreateBlockCycle(int blockSize, int numberOfBlocks)
    {
        if (blockSize < 0 || numberOfBlocks < 0)
            throw new ArgumentException("Block size and number of blocks cannot be negative.");

        int[] cycle = new int[blockSize * numberOfBlocks];

        int i = blockSize * (numberOfBlocks - 1) - 1;
        for (; i >= 0; --i)
            cycle[i] = i + blockSize;

        i = blockSize * (numberOfBlocks - 1);
        int k = 0;
        for (; i < cycle.Length; ++i)
        {
            cycle[i] = k++;
        }

        return cycle;
    }
}
