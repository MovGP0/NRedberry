namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    private static void RangeCheck(int arrayLen, int fromIndex, int toIndex)
    {
        if (fromIndex > toIndex)
        {
            throw new ArgumentException($"fromIndex({fromIndex}) > toIndex({toIndex})");
        }

        if (fromIndex < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(fromIndex), $"fromIndex({fromIndex}) must not be less than zero.");
        }

        if (toIndex > arrayLen)
        {
            throw new ArgumentOutOfRangeException(nameof(toIndex), $"toIndex({toIndex}) is greater than array length ({arrayLen}).");
        }
    }
}
