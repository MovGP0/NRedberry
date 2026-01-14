using System.Numerics;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Groups;

/// <summary>
/// Algorithms using backtrack search in permutation groups, including subgroup search, stabilizers, coset representatives,
/// intersections, and more.
/// </summary>
public static class AlgorithmsBacktrack
{
    private static readonly long[] ____VISITED_NODES___ = new long[1];

    /// <summary>
    /// Searches for a subgroup within a specified group.
    /// </summary>
    public static void SubgroupSearch(
        List<BSGSElement> group,
        List<BSGSCandidateElement> subgroup,
        IBacktrackSearchTestFunction testFunction,
        IIndicator<Permutation> property)
    {
        SubgroupSearchWithPayload(group, subgroup, BacktrackSearchPayload.CreateDefaultPayload(testFunction), property);
    }

    /// <summary>
    /// Searches for a subgroup using a payload for each tree level.
    /// </summary>
    public static void SubgroupSearchWithPayload(
        List<BSGSElement> group,
        List<BSGSCandidateElement> subgroup,
        BacktrackSearchPayload payload,
        IIndicator<Permutation> property)
    {
        int[] baseArray = AlgorithmsBase.GetBaseAsArray(group);
        InducedOrdering ordering = new(baseArray);
        SubgroupSearchWithPayload(group, subgroup, payload, property, baseArray, ordering);
    }

    /// <summary>
    /// Searches for a subgroup with a precomputed base and ordering.
    /// </summary>
    public static void SubgroupSearch(
        List<BSGSElement> group,
        List<BSGSCandidateElement> subgroup,
        IBacktrackSearchTestFunction testFunction,
        IIndicator<Permutation> property,
        int[] baseArray,
        InducedOrdering ordering)
    {
        SubgroupSearchWithPayload(
            group,
            subgroup,
            BacktrackSearchPayload.CreateDefaultPayload(testFunction),
            property,
            baseArray,
            ordering);
    }

    /// <summary>
    /// Searches for a subgroup with a precomputed base and ordering.
    /// </summary>
    public static void SubgroupSearchWithPayload(
        List<BSGSElement> group,
        List<BSGSCandidateElement> subgroup,
        BacktrackSearchPayload payload,
        IIndicator<Permutation> property,
        int[] baseArray,
        InducedOrdering ordering)
    {
        if (group.Count == 0 || group[0].StabilizerGeneratorsReference.Count == 0)
        {
            throw new ArgumentException("Empty group.");
        }

        ____VISITED_NODES___[0] = 0;

        int degree = group[0].InternalDegree;
        if (subgroup.Count > 0 && subgroup[0].InternalDegree > degree)
        {
            throw new ArgumentException("Specified subgroup is not a subgroup of specified group.");
        }

        int size = group.Count;
        Permutation identity = group[0].StabilizerGeneratorsReference[0].Identity;

        if (subgroup.Count == 0)
        {
            subgroup.Add(new BSGSCandidateElement(baseArray[0], new List<Permutation>(), degree));
        }

        int level = size - 1;
        Permutation[] word = new Permutation[size];

        int[][] cachedSortedOrbits = new int[size][];
        int[][] sortedOrbits = new int[size][];
        for (int i = 0; i < size; ++i)
        {
            cachedSortedOrbits[i] = group[i].OrbitListReference.ToArray();
            Array.Sort(cachedSortedOrbits[i], (a, b) => ordering.Compare(a, b));
            sortedOrbits[i] = cachedSortedOrbits[i];
            word[i] = identity;
        }

        payload.SetWordReference(word);
        int[] tuple = new int[size];

        RebaseWithRedundancy(subgroup, baseArray, degree);
        int subgroupLevel = level;
        List<BSGSCandidateElement> subgroupRebase = AlgorithmsBase.Clone(subgroup);

        int[] maxImages = new int[size];
        maxImages[level] = ordering.MinElement();

        int[] maxRepresentative = new int[size];
        maxRepresentative[level] = subgroup[level].OrbitSize <= 1
            ? int.MaxValue
            : sortedOrbits[level][sortedOrbits[level].Length - subgroup[level].OrbitSize + 1];

        while (true)
        {
            int image = word[level].NewIndexOf(baseArray[level]);
            ReplaceBasePointWithRedundancy(subgroupRebase, level, image);
            while (level < size - 1
                && IsMinimalInOrbit(subgroupRebase[level].OrbitListReference, image, ordering)
                && ordering.Compare(image, maxImages[level]) > 0
                && ordering.Compare(image, maxRepresentative[level]) < 0
                && payload.Test(word[level], level))
            {
                payload.BeforeLevelIncrement(level);
                if (!AssertPartialBaseImage(level, word, baseArray, subgroupRebase))
                {
                    throw new InvalidOperationException("Partial base image assertion failed.");
                }

                ++level;

                if (word[level - 1].IsIdentity)
                {
                    sortedOrbits[level] = cachedSortedOrbits[level];
                }
                else
                {
                    sortedOrbits[level] = word[level - 1].ImageOf(group[level].OrbitListReference.ToArray());
                    Array.Sort(sortedOrbits[level], (a, b) => ordering.Compare(a, b));
                }

                int max = ordering.MinElement();
                for (int j = 0; j < level; ++j)
                {
                    if (subgroup[j].BelongsToOrbit(baseArray[level]))
                    {
                        max = ordering.Max(max, word[j].NewIndexOf(baseArray[j]));
                    }
                }

                maxImages[level] = max;
                maxRepresentative[level] = subgroup[level].OrbitSize <= 1
                    ? int.MaxValue
                    : sortedOrbits[level][sortedOrbits[level].Length - subgroup[level].OrbitSize + 1];

                tuple[level] = 0;
                word[level] = group[level]
                    .GetTransversalOf(word[level - 1].NewIndexOfUnderInverse(sortedOrbits[level][tuple[level]]))
                    .Composition(word[level - 1]);

                image = word[level].NewIndexOf(baseArray[level]);
                ReplaceBasePointWithRedundancy(subgroupRebase, level, image);

                payload.AfterLevelIncrement(level);
            }

            image = word[level].NewIndexOf(baseArray[level]);
            ++____VISITED_NODES___[0];

            if (level == size - 1
                && IsMinimalInOrbit(subgroupRebase[level].OrbitListReference, image, ordering)
                && ordering.Compare(image, maxImages[level]) > 0
                && ordering.Compare(image, maxRepresentative[level]) < 0
                && payload.Test(word[level], level)
                && property.Is(word[level]))
            {
                if (!AlgorithmsBase.MembershipTest(subgroup, word[level]))
                {
                    subgroup[0].AddStabilizer(word[level]);
                    AlgorithmsBase.SchreierSimsAlgorithm(subgroup);
                    subgroupRebase = AlgorithmsBase.Clone(subgroup);
                }

                level = subgroupLevel;
            }

            while (level >= 0 && tuple[level] == group[level].OrbitListReference.Count - 1)
            {
                --level;
            }

            if (level == -1)
            {
                return;
            }

            if (level < subgroupLevel)
            {
                subgroupLevel = level;
                tuple[level] = 0;
                maxImages[level] = ordering.MinElement();
                maxRepresentative[level] = subgroup[level].OrbitSize <= 1
                    ? int.MaxValue
                    : sortedOrbits[level][sortedOrbits[level].Length - subgroup[level].OrbitSize + 1];
            }

            ++tuple[level];

            if (level == 0)
            {
                word[0] = group[0].GetTransversalOf(sortedOrbits[0][tuple[0]]);
            }
            else
            {
                word[level] = group[level]
                    .GetTransversalOf(word[level - 1].NewIndexOfUnderInverse(sortedOrbits[level][tuple[level]]))
                    .Composition(word[level - 1]);
            }
        }
    }

    /// <summary>
    /// Computes left coset representatives of the specified subgroup.
    /// </summary>
    public static Permutation[] LeftCosetRepresentatives(
        List<BSGSElement> group,
        List<BSGSElement> subgroup)
    {
        int[] baseArray = AlgorithmsBase.GetBaseAsArray(group);
        InducedOrdering ordering = new(baseArray);
        return LeftCosetRepresentatives(group, subgroup, baseArray, ordering);
    }

    /// <summary>
    /// Computes left coset representatives with a precomputed base.
    /// </summary>
    public static Permutation[] LeftCosetRepresentatives(
        List<BSGSElement> group,
        List<BSGSElement> subgroup,
        int[] baseArray,
        InducedOrdering ordering)
    {
        if (group.Count == 0 || group[0].StabilizerGeneratorsReference.Count == 0)
        {
            throw new ArgumentException("Empty group.");
        }

        ____VISITED_NODES___[0] = 0;

        List<BSGSCandidateElement> subgroupCandidates = AlgorithmsBase.AsBSGSCandidatesList(subgroup);
        BigInteger orderRatio = AlgorithmsBase.CalculateOrder(group) / AlgorithmsBase.CalculateOrderFromCandidates(subgroupCandidates);
        int cosetSize = (int)orderRatio;
        Permutation[] coset = new Permutation[cosetSize];
        int cosetCounter = 0;

        int degree = group[0].InternalDegree;
        int size = group.Count;
        int level = size - 1;
        Permutation[] word = new Permutation[size];
        Permutation identity = group[0].StabilizerGeneratorsReference[0].Identity;

        int[][] cachedSortedOrbits = new int[size][];
        int[][] sortedOrbits = new int[size][];
        for (int i = 0; i < size; ++i)
        {
            cachedSortedOrbits[i] = group[i].OrbitListReference.ToArray();
            Array.Sort(cachedSortedOrbits[i], (a, b) => ordering.Compare(a, b));
            sortedOrbits[i] = cachedSortedOrbits[i];
            word[i] = identity;
        }

        int[] tuple = new int[size];
        RebaseWithRedundancy(subgroupCandidates, baseArray, degree);
        List<BSGSCandidateElement> subgroupRebase = AlgorithmsBase.Clone(subgroupCandidates);

        while (true)
        {
            int image = word[level].NewIndexOf(baseArray[level]);
            ReplaceBasePointWithRedundancy(subgroupRebase, level, image);
            while (level < size - 1
                && IsMinimalInOrbit(subgroupRebase[level].OrbitListReference, image, ordering))
            {
                ++level;
                if (word[level - 1].IsIdentity)
                {
                    sortedOrbits[level] = cachedSortedOrbits[level];
                }
                else
                {
                    sortedOrbits[level] = word[level - 1].ImageOf(group[level].OrbitListReference.ToArray());
                    Array.Sort(sortedOrbits[level], (a, b) => ordering.Compare(a, b));
                }

                tuple[level] = 0;
                word[level] = group[level]
                    .GetTransversalOf(word[level - 1].NewIndexOfUnderInverse(sortedOrbits[level][tuple[level]]))
                    .Composition(word[level - 1]);

                image = word[level].NewIndexOf(baseArray[level]);
                ReplaceBasePointWithRedundancy(subgroupRebase, level, image);
            }

            ++____VISITED_NODES___[0];
            if (level == size - 1
                && IsMinimalInOrbit(subgroupRebase[level].OrbitListReference, image, ordering))
            {
                coset[cosetCounter++] = word[level];
            }

            while (level >= 0 && tuple[level] == group[level].OrbitListReference.Count - 1)
            {
                --level;
            }

            if (level == -1)
            {
                return coset;
            }

            ++tuple[level];
            if (level == 0)
            {
                word[0] = group[0].GetTransversalOf(sortedOrbits[0][tuple[0]]);
            }
            else
            {
                word[level] = group[level]
                    .GetTransversalOf(word[level - 1].NewIndexOfUnderInverse(sortedOrbits[level][tuple[level]]))
                    .Composition(word[level - 1]);
            }
        }
    }

    /// <summary>
    /// Computes minimal coset representative of a group element.
    /// </summary>
    public static Permutation LeftTransversalOf(
        Permutation element,
        List<BSGSElement> group,
        List<BSGSElement> subgroup)
    {
        int[] baseArray = AlgorithmsBase.GetBaseAsArray(group);
        InducedOrdering ordering = new(baseArray);
        return LeftTransversalOf(element, group, subgroup, baseArray, ordering);
    }

    /// <summary>
    /// Computes minimal coset representative with a precomputed base.
    /// </summary>
    public static Permutation LeftTransversalOf(
        Permutation element,
        List<BSGSElement> group,
        List<BSGSElement> subgroup,
        int[] baseArray,
        InducedOrdering ordering)
    {
        if (group.Count == 0 || subgroup.Count == 0)
        {
            throw new ArgumentException("Empty group.");
        }

        int degree = group[0].InternalDegree;
        List<BSGSCandidateElement> candidates = AlgorithmsBase.AsBSGSCandidatesList(subgroup);
        RebaseWithRedundancy(candidates, baseArray, degree);

        Permutation transversal = element;
        int[] minimalImage = new int[baseArray.Length];

        for (int level = 0; level < group.Count; ++level)
        {
            int image = transversal.NewIndexOf(baseArray[level]);
            IList<int> orbit = Permutations.GetOrbitList(candidates[level].StabilizerGeneratorsReference.AsReadOnly(), image, degree);
            minimalImage[level] = ordering.Min(orbit);
            ReplaceBasePointWithRedundancy(candidates, level, image);
            transversal = transversal
                .Composition(candidates[level].GetTransversalOf(minimalImage[level]));
            ReplaceBasePointWithRedundancy(candidates, level, minimalImage[level]);
        }

        return transversal;
    }

    /// <summary>
    /// Computes intersection of two subgroups using subgroup search.
    /// </summary>
    public static void Intersection(
        List<BSGSElement> group1,
        List<BSGSElement> group2,
        List<BSGSCandidateElement> intersection)
    {
        if (AlgorithmsBase.CalculateOrder(group2) < AlgorithmsBase.CalculateOrder(group1))
        {
            Intersection(group2, group1, intersection);
            return;
        }

        List<BSGSCandidateElement> smaller = AlgorithmsBase.AsBSGSCandidatesList(group1);
        List<BSGSCandidateElement> larger = AlgorithmsBase.AsBSGSCandidatesList(group2);
        int degree = larger[0].InternalDegree;

        RebaseWithRedundancy(smaller, AlgorithmsBase.GetBaseAsArray(larger), degree);
        int[] baseArray = AlgorithmsBase.GetBaseAsArray(smaller);
        RebaseWithRedundancy(larger, baseArray, degree);

        System.Diagnostics.Debug.Assert(smaller.Count == larger.Count);

        Permutation identity = smaller[0].StabilizerGeneratorsReference[0].Identity;
        Permutation[] intersectionWord = new Permutation[smaller.Count];
        for (int i = 0; i < intersectionWord.Length; ++i)
        {
            intersectionWord[i] = identity;
        }

        BacktrackSearchPayload payload = new IntersectionPayload(larger, baseArray, intersectionWord);
        IIndicator<Permutation> intersectionProperty = new IntersectionIndicator(larger);

        SubgroupSearchWithPayload(smaller.OfType<BSGSElement>().ToList(), intersection, payload, intersectionProperty);
    }

    public static void RebaseWithRedundancy(List<BSGSCandidateElement> group, int[] baseArray, int degree)
    {
        AlgorithmsBase.Rebase(group, baseArray);
        if (group.Count < baseArray.Length)
        {
            for (int i = group.Count; i < baseArray.Length; ++i)
            {
                group.Add(new BSGSCandidateElement(baseArray[i], new List<Permutation>(), degree));
            }
        }
    }

    private static bool IsMinimalInOrbit(List<int> orbit, int point, InducedOrdering ordering)
    {
        bool belongsToOrbit = false;
        for (int i = orbit.Count - 1; i >= 0; --i)
        {
            int compare = ordering.Compare(orbit[i], point);
            if (compare < 0)
            {
                return false;
            }

            if (compare == 0)
            {
                belongsToOrbit = true;
            }
        }

        return belongsToOrbit;
    }

    private static void ReplaceBasePointWithRedundancy(List<BSGSCandidateElement> group, int index, int newPoint)
    {
        if (group[index].BasePoint == newPoint)
        {
            return;
        }

        int oldSize = group.Count;
        AlgorithmsBase.ChangeBasePointWithTranspositions(group, index, newPoint);

        while (group.Count > oldSize)
        {
            BSGSCandidateElement last = group[group.Count - 1];
            if (last.StabilizerGeneratorsReference.Count == 0)
            {
                group.RemoveAt(group.Count - 1);
            }
            else
            {
                break;
            }
        }
    }

    private static bool AssertPartialBaseImage(int level, Permutation[] word, int[] baseArray, List<BSGSCandidateElement> subgroupRebase)
    {
        for (int i = 0; i <= level; ++i)
        {
            if (subgroupRebase[i].BasePoint != word[i].NewIndexOf(baseArray[i]))
            {
                return false;
            }
        }

        return true;
    }
}
