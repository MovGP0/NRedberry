namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    public static int[] AddAll(int[] array1, params int[] array2)
    {
        int[] r = new int[array1.Length + array2.Length];
        Array.Copy(array1, 0, r, 0, array1.Length);
        Array.Copy(array2, 0, r, array1.Length, array2.Length);
        return r;
    }

    public static int[] AddAll(params int[][] arrays)
    {
        if (arrays.Length == 0)
        {
            return Array.Empty<int>();
        }

        int length = 0;
        foreach (int[] array in arrays)
        {
            length += array.Length;
        }

        if (length == 0)
        {
            return Array.Empty<int>();
        }

        int[] r = new int[length];
        int pointer = 0;
        foreach (int[] array in arrays)
        {
            Array.Copy(array, 0, r, pointer, array.Length);
            pointer += array.Length;
        }

        return r;
    }
}
