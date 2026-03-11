using System.Numerics;

namespace NRedberry.Contexts;

public static class LongExtensions
{
    public static int BitCount(this long i)
    {
        return BitOperations.PopCount((ulong)i);
    }

    public static int NumberOfTrailingZeros(this long n)
    {
        if (n == 0)
        {
            return 64;
        }

        return BitOperations.TrailingZeroCount((ulong)n);
    }
}
