namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    public static T[] Select<T>(T[] array, int[] positions)
    {
        if (array == null)
        {
            throw new ArgumentNullException();
        }

        int[] p = MathUtils.GetSortedDistinct(positions);
        T[] r = new T[p.Length];
        for (int i = 0; i < p.Length; ++i)
        {
            r[i] = array[p[i]];
        }

        return r;
    }
}
