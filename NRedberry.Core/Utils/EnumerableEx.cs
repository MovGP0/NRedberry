using System.Linq;

namespace NRedberry.Core.Utils;

public static class EnumerableEx
{
    public static int GetHashCode(int[] data)
    {
        unchecked
        {
            const int primeNumber1 = 17;
            const int primeNumber2 = 31;
            return data.Aggregate(primeNumber1, (current, val) => current * primeNumber2 + val.GetHashCode());
        }
    }
}