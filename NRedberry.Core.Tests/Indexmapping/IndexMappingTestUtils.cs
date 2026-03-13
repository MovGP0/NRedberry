using NRedberry.IndexMapping;
using NRedberry.Indices;

namespace NRedberry.Core.Tests.Indexmapping;

public static class IndexMappingTestUtils
{
    private static readonly IComparer<Mapping> s_comparator = Comparer<Mapping>.Create(
        static (left, right) => left.GetHashCode().CompareTo(right.GetHashCode()));

    public static Mapping Parse(string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        string[] singleMaps = value.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (singleMaps.Length == 0)
        {
            throw new ArgumentException("Mapping string cannot be empty.", nameof(value));
        }

        bool sign = singleMaps[0] switch
        {
            "+" => false,
            "-" => true,
            _ => throw new InvalidOperationException("First token must be '+' or '-'."),
        };

        if (singleMaps.Length == 1)
        {
            return new Mapping([], [], sign);
        }

        int[] from = new int[singleMaps.Length - 1];
        int[] to = new int[singleMaps.Length - 1];
        for (int i = 1; i < singleMaps.Length; ++i)
        {
            string[] parts = singleMaps[i].Split("->", StringSplitOptions.TrimEntries);
            if (parts.Length != 2)
            {
                throw new ArgumentException("Invalid mapping entry: " + singleMaps[i], nameof(value));
            }

            from[i - 1] = ParseIndex(parts[0]);
            to[i - 1] = ParseIndex(parts[1]);
        }

        return new Mapping(from, to, sign);
    }

    public static bool Compare(IList<Mapping> first, IList<Mapping> second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        List<Mapping> firstCopy = [.. first];
        List<Mapping> secondCopy = [.. second];
        firstCopy.Sort(s_comparator);
        secondCopy.Sort(s_comparator);

        if (firstCopy.Count != secondCopy.Count)
        {
            return false;
        }

        for (int i = 0; i < firstCopy.Count; ++i)
        {
            if (!firstCopy[i].Equals(secondCopy[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static IComparer<Mapping> GetComparator()
    {
        return s_comparator;
    }

    private static int ParseIndex(string value)
    {
        return IndicesUtils.ParseIndex(value);
    }
}
