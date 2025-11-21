namespace NRedberry.Groups;

public static partial class Permutations
{
    public static int[] CreateBlockTransposition(int length1, int length2)
    {
        if (length1 < 0 || length2 < 0)
            throw new ArgumentException("Block lengths cannot be negative.");

        int[] result = new int[length1 + length2];
        int i = 0;
        for (; i < length2; ++i)
            result[i] = length1 + i;
        for (; i < result.Length; ++i)
            result[i] = i - length2;
        return result;
    }
}
