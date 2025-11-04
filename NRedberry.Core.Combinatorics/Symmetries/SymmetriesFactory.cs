namespace NRedberry.Core.Combinatorics.Symmetries;

/// <summary>
/// src\main\java\cc\redberry\core\combinatorics\symmetries\SymmetriesFactory.java
/// </summary>
public static class SymmetriesFactory
{
    private static readonly Symmetries EmptySymmetries0 = new EmptySymmetries(0);
    private static readonly Symmetries EmptySymmetries1 = new EmptySymmetries(1);

    public static Symmetries CreateSymmetries(int dimension)
    {
        if (dimension < 0)
            throw new ArgumentException();
        if (dimension == 0)
            return EmptySymmetries0;
        if (dimension == 1)
            return EmptySymmetries1;
        return new SymmetriesImpl(dimension);
    }

    public static Symmetries CreateFullSymmetries(int dimension)
    {
        if (dimension < 0)
            throw new ArgumentException();
        if (dimension == 0)
            return EmptySymmetries0;
        if (dimension == 1)
            return EmptySymmetries1;
        return new FullSymmetries(dimension);
    }

    public static Symmetries CreateFullSymmetries(int upperCount, int lowerCount)
    {
        if (upperCount < 0 || upperCount < 0)
            throw new ArgumentException();
        if (upperCount + lowerCount <= 1)
            return CreateSymmetries(upperCount + lowerCount);

        var symmetries = new SymmetriesImpl(upperCount + lowerCount);

        //transposition
        int i;
        if (upperCount > 1)
        {
            var upperTransposition = Combinatorics.CreateIdentity(upperCount + lowerCount);
            upperTransposition[0] = 1;
            upperTransposition[1] = 0;
            var upperTranspositionSymmetry = new Symmetry(upperTransposition, false);
            symmetries.AddUnsafe(upperTranspositionSymmetry);
        }

        if (lowerCount > 1)
        {
            var lowerTransposition = Combinatorics.CreateIdentity(upperCount + lowerCount);
            lowerTransposition[upperCount] = 1 + upperCount;
            lowerTransposition[upperCount + 1] = upperCount;
            var lowerTranspositionSymmetry = new Symmetry(lowerTransposition, false);
            symmetries.AddUnsafe(lowerTranspositionSymmetry);
        }

        //cycle
        if (upperCount > 2)
        {
            var upperCycle = new int[upperCount + lowerCount];
            upperCycle[0] = upperCount - 1;
            for (i = 1; i < upperCount; ++i)
                upperCycle[i] = i - 1;
            for (; i < upperCount + lowerCount; ++i)
                upperCycle[i] = i;
            var upperCycleSymmetry = new Symmetry(upperCycle, false);
            symmetries.AddUnsafe(upperCycleSymmetry);
        }

        if (lowerCount > 2)
        {
            var lowerCycle = new int[upperCount + lowerCount];
            for (i = 0; i < upperCount; ++i)
                lowerCycle[i] = i;
            lowerCycle[upperCount] = upperCount + lowerCount - 1;
            ++i;
            for (; i < upperCount + lowerCount; ++i)
                lowerCycle[i] = i - 1;
            var lowerCycleSymmetry = new Symmetry(lowerCycle, false);
            symmetries.AddUnsafe(lowerCycleSymmetry);
        }

        return symmetries;
    }
}
