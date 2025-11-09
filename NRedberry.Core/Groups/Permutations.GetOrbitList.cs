using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Groups;

public static partial class Permutations
{
    public static IntArrayList GetOrbitList(IEnumerable<Permutation> stabilizerGenerators, int point, int degree)
    {
        var orbitList = new IntArrayList();
        orbitList.Add(point);
        var generators = stabilizerGenerators ?? Enumerable.Empty<Permutation>();

        var seen = new BitArray(degree);
        seen.Set(point, true);

        for (int orbitIndex = 0; orbitIndex < orbitList.Count; ++orbitIndex)
        {
            int current = orbitList[orbitIndex];
            foreach (Permutation generator in generators)
            {
                int imageOfPoint = generator.NewIndexOf(current);
                if (!seen.Get(imageOfPoint))
                {
                    orbitList.Add(imageOfPoint);
                    seen.Set(imageOfPoint, true);
                }
            }
        }

        return orbitList;
    }
}
