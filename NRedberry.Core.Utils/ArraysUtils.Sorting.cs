namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    public static void TimSort(int[] target, int[] coSort)
    {
        IntTimSort.Sort(target, coSort);
    }

    public static void StableSort(int[] target, int[] coSort)
    {
        if (target.Length > 100)
        {
            TimSort(target, coSort);
        }
        else
        {
            InsertionSort(target, coSort);
        }
    }
}
