namespace NRedberry.Groups;

public sealed partial class PermutationGroup
{
    public int[] PositionsInOrbits => _positionsInOrbits.ToArray();

    public int[] Orbit(int point)
    {
        if (point >= _internalDegree)
        {
            return Array.Empty<int>();
        }

        return _orbits[_positionsInOrbits[point]].ToArray();
    }

    public int OrbitSize(int point)
    {
        if (point >= _internalDegree)
        {
            return 0;
        }

        return _orbits[_positionsInOrbits[point]].Length;
    }

    public int[] Orbit(params int[] points)
    {
        var set = new HashSet<int>();
        foreach (int point in points)
        {
            if (point >= _internalDegree)
            {
                continue;
            }

            foreach (int orbitPoint in _orbits[_positionsInOrbits[point]])
            {
                set.Add(orbitPoint);
            }
        }

        return set.ToArray();
    }

    public int[][] Orbits()
    {
        int[][] result = new int[_orbits.Length][];
        for (int i = 0; i < _orbits.Length; ++i)
        {
            result[i] = _orbits[i].ToArray();
        }

        return result;
    }

    public int IndexOfOrbit(int point)
    {
        if (point >= _internalDegree)
        {
            return -1;
        }

        return _positionsInOrbits[point];
    }
}
