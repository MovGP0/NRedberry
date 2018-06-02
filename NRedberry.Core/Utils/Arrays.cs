using System.Linq;

namespace NRedberry.Core.Utils
{
    public static class Arrays
    {
        public static int[] copyOfRange(int[] src, int start, int end)
        {
            int len = end - start;
            int[] dest = new int[len];
            for (int i = 0; i < len; ++i)
            {
                dest[i] = src[start + i]; // so 0..n = 0+x..n+x
            }
            return dest;
        }

        public static int[] copyOf(int[] original, int newLength)
        {
            return original.Take(newLength).ToArray();
        }

        public static void fill(int[] original, int number)
        {
            for (int i = 0; i < original.Length; ++i)
            {
                original[i] = number;
            }
        }
    }
}