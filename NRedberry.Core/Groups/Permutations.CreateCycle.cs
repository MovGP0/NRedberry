namespace NRedberry.Core.Groups;

public static partial class Permutations
{
    public static int[] CreateCycle(int dimension)
    {
        if (dimension < 0)
            throw new ArgumentException("Negative degree.", nameof(dimension));

        int[] cycle = new int[dimension];
        for (int i = 0; i < dimension - 1; ++i)
        {
            cycle[i + 1] = i;
        }

        if (dimension > 0)
            cycle[0] = dimension - 1;

        return cycle;
    }
}
