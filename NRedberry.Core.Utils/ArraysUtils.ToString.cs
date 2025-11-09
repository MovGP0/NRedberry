using System.Text;

namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    public static string ToString<T>(T[]? a, IToStringConverter<T> format)
    {
        if (a == null)
        {
            return "null";
        }

        int iMax = a.Length - 1;
        if (iMax == -1)
        {
            return "[]";
        }

        var b = new StringBuilder();
        b.Append('[');
        for (int i = 0; ; i++)
        {
            b.Append(format.ToString(a[i]));
            if (i == iMax)
            {
                return b.Append(']').ToString();
            }

            b.Append(", ");
        }
    }

    public static string ToString(int[]? a, IToStringConverter<int> format)
    {
        if (a == null)
        {
            return "null";
        }

        int iMax = a.Length - 1;
        if (iMax == -1)
        {
            return "[]";
        }

        var b = new StringBuilder();
        b.Append('[');
        for (int i = 0; ; i++)
        {
            b.Append(format.ToString(a[i]));
            if (i == iMax)
            {
                return b.Append(']').ToString();
            }

            b.Append(", ");
        }
    }
}
