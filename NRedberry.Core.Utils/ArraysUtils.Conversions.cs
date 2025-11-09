namespace NRedberry.Core.Utils;

public static partial class ArraysUtils
{
    public static sbyte[] Int2byte(int[] a)
    {
        sbyte[] r = new sbyte[a.Length];
        for (int i = 0; i < a.Length; ++i)
        {
            r[i] = (sbyte)a[i];
        }

        return r;
    }

    public static short[] Int2short(int[] a)
    {
        short[] r = new short[a.Length];
        for (int i = 0; i < a.Length; ++i)
        {
            r[i] = (short)a[i];
        }

        return r;
    }

    public static int[] ShortToInt(short[] a)
    {
        int[] r = new int[a.Length];
        for (int i = 0; i < a.Length; ++i)
        {
            r[i] = a[i];
        }

        return r;
    }

    public static short[] IntToShort(int[] a)
    {
        short[] r = new short[a.Length];
        for (int i = 0; i < a.Length; ++i)
        {
            r[i] = (short)a[i];
        }

        return r;
    }

    public static int[] ByteToInt(sbyte[] a)
    {
        int[] r = new int[a.Length];
        for (int i = 0; i < a.Length; ++i)
        {
            r[i] = a[i];
        }

        return r;
    }

    public static short[] ByteToShort(sbyte[] a)
    {
        short[] r = new short[a.Length];
        for (int i = 0; i < a.Length; ++i)
        {
            r[i] = a[i];
        }

        return r;
    }

    public static byte[] IntToByte(int[] a)
    {
        byte[] r = new byte[a.Length];
        for (int i = 0; i < a.Length; ++i)
        {
            r[i] = (byte)a[i];
        }

        return r;
    }
}
