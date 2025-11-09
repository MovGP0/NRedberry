namespace NRedberry.Core.Groups;

public static partial class Permutations
{
    public static int[] ConvertCyclesToOneLine(int[][] cycles)
    {
        int degree = -1;
        foreach (int[] cycle in cycles)
        {
            degree = Math.Max(degree, cycle.Max());
        }

        ++degree;

        int[] permutation = new int[degree];
        for (int i = 1; i < degree; ++i)
        {
            permutation[i] = i;
        }

        foreach (int[] cycle in cycles)
        {
            if (cycle.Length == 0)
                continue;
            if (cycle.Length == 1)
                throw new ArgumentException($"Illegal use of cycle notation: {string.Join(", ", cycle)}");

            for (int k = 0, s = cycle.Length - 1; k < s; ++k)
                permutation[cycle[k]] = cycle[k + 1];

            permutation[cycle[^1]] = cycle[0];
        }

        return permutation;
    }

    public static int[][] ConvertOneLineToCycles(int[] permutation)
    {
        var cycles = new List<int[]>();
        var seen = new bool[permutation.Length];
        int counter = 0;

        while (counter < permutation.Length)
        {
            int start = Array.IndexOf(seen, false);
            if (permutation[start] == start)
            {
                counter++;
                seen[start] = true;
                continue;
            }

            var cycle = new List<int>();
            while (!seen[start])
            {
                seen[start] = true;
                counter++;
                cycle.Add(start);
                start = permutation[start];
            }

            cycles.Add(cycle.ToArray());
        }

        return cycles.ToArray();
    }

    public static int[][] ConvertOneLineToCycles(short[] permutation)
    {
        var cycles = new List<int[]>();
        var seen = new bool[permutation.Length];
        int counter = 0;

        while (counter < permutation.Length)
        {
            int start = Array.IndexOf(seen, false);
            if (permutation[start] == start)
            {
                counter++;
                seen[start] = true;
                continue;
            }

            var cycle = new List<int>();
            while (!seen[start])
            {
                seen[start] = true;
                counter++;
                cycle.Add(start);
                start = permutation[start];
            }

            cycles.Add(cycle.ToArray());
        }

        return cycles.ToArray();
    }

    public static int[][] ConvertOneLineToCycles(sbyte[] permutation)
    {
        var cycles = new List<int[]>();
        var seen = new bool[permutation.Length];
        int counter = 0;

        while (counter < permutation.Length)
        {
            int start = Array.IndexOf(seen, false);
            if (permutation[start] == start)
            {
                counter++;
                seen[start] = true;
                continue;
            }

            var cycle = new List<int>();
            while (!seen[start])
            {
                seen[start] = true;
                counter++;
                cycle.Add(start);
                start = permutation[start];
            }

            cycles.Add(cycle.ToArray());
        }

        return cycles.ToArray();
    }
}
