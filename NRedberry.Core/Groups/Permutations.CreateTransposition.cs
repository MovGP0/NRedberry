namespace NRedberry.Groups;

public static partial class Permutations
{
    public static int[] CreateTransposition(int dimension)
    {
        if (dimension < 0)
            throw new ArgumentException("Dimension cannot be negative.", nameof(dimension));

        if (dimension > 1)
            return CreateTransposition(dimension, 0, 1);

        return new int[dimension];
    }

    public static int[] CreateTransposition(int dimension, int position1, int position2)
    {
        if (dimension < 0)
            throw new ArgumentException("Dimension is negative.", nameof(dimension));
        if (position1 < 0 || position2 < 0)
            throw new ArgumentOutOfRangeException("Positions must be non-negative.");
        if (position1 >= dimension || position2 >= dimension)
            throw new ArgumentOutOfRangeException("Positions must be within dimension range.");

        int[] transposition = new int[dimension];
        for (int i = 0; i < dimension; ++i)
            transposition[i] = i;
        int temp = transposition[position1];
        transposition[position1] = transposition[position2];
        transposition[position2] = temp;
        return transposition;
    }
}
