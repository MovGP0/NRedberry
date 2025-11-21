using NRedberry.Core.Combinatorics;

namespace NRedberry.Groups;

public static class RandomPermutation
{
    public const int DefaultRandomnessExtendToSize = 10;
    public const int DefaultNumberOfRandomRefinements = 20;

    public static void Randomness(IList<Permutation> generators)
    {
        Randomness(generators, DefaultRandomnessExtendToSize, DefaultNumberOfRandomRefinements, System.Random.Shared);
    }

    public static void Randomness(IList<Permutation> generators, int extendToSize, int numberOfRefinements, Random random)
    {
        if (generators.Count < 2 && extendToSize < 2)
        {
            throw new ArgumentException("List should extended by at least one element.");
        }

        if (generators.Count < extendToSize)
        {
            int delta = extendToSize - generators.Count + 1;
            int i = 0;
            while (--delta >= 0)
            {
                generators.Add(generators[i++]);
            }
        }

        if (!generators[^1].IsIdentity)
        {
            generators.Add(generators[0].Identity);
        }

        while (--numberOfRefinements >= 0)
        {
            Random(generators, random);
        }
    }

    public static Permutation Random(IList<Permutation> generators)
    {
        return Random(generators, System.Random.Shared);
    }

    public static Permutation Random(IList<Permutation> generators, Random random)
    {
        if (generators.Count < 3)
        {
            throw new ArgumentException("List size should be >= 3");
        }

        int generatorsSize = generators.Count - 1;
        int s = random.Next(generatorsSize);
        int t;
        do
        {
            t = random.Next(generatorsSize);
        }
        while (t == s);

        Permutation ps = generators[s];
        Permutation pt = generators[t];
        Permutation x0 = generators[generatorsSize];
        if (random.Next(2) == 1)
        {
            pt = pt.Inverse();
        }

        if (random.Next(2) == 1)
        {
            var composition0 = ps.Composition(pt);
            generators[s] = composition0;
            ps = composition0;

            var composition1 = x0.Composition(ps);
            generators[generatorsSize] = composition1;
            x0 = composition1;
        }
        else
        {
            var composition0 = pt.Composition(ps);
            generators[s] = composition0;
            ps = composition0;

            var composition1 = ps.Composition(x0);
            generators[generatorsSize] = composition1;
            x0 = composition1;
        }

        return x0;
    }
}
