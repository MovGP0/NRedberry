using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Groups;

public static partial class Permutations
{
    public static int GetOrbitSize(IEnumerable<Permutation> generators, int point, int degree)
    {
        return GetOrbitList(generators, point, degree).Count;
    }

    public static int GetOrbitSize(IEnumerable<Permutation> generators, int point)
    {
        var list = generators is IReadOnlyCollection<Permutation> collection ? collection : generators.ToList();
        int degree = InternalDegree(list.ToList());
        return GetOrbitSize(generators, point, degree);
    }

    public static int[][] Orbits(IList<Permutation> generators, int[] positionsInOrbit)
    {
        if (generators.Count == 0)
            return [];

        Array.Fill(positionsInOrbit, -1);
        var orbits = new List<int[]>();
        int seenCount = 0;
        int orbitsIndex = 0;

        while (seenCount < positionsInOrbit.Length)
        {
            var orbit = new List<int>();
            int point = Array.IndexOf(positionsInOrbit, -1);
            if (point == -1)
                break;

            orbit.Add(point);
            positionsInOrbit[point] = orbitsIndex;
            seenCount++;

            for (int orbitIndex = 0; orbitIndex < orbit.Count; ++orbitIndex)
            {
                int current = orbit[orbitIndex];
                foreach (Permutation generator in generators)
                {
                    int imageOfPoint = generator.NewIndexOf(current);
                    if (positionsInOrbit[imageOfPoint] == -1)
                    {
                        positionsInOrbit[imageOfPoint] = orbitsIndex;
                        seenCount++;
                        orbit.Add(imageOfPoint);
                    }
                }
            }

            orbits.Add(orbit.ToArray());
            orbitsIndex++;
        }

        return orbits.ToArray();
    }
}
