using System.Numerics;
using NRedberry.Core.Combinatorics;

namespace NRedberry.Core.Groups;

public static class AlgorithmsBase
{
    /// <summary>
    /// Holder returned by <see cref="Strip(IReadOnlyList{BSGSElement}, Permutation)"/>.
    /// </summary>
    public sealed class StripContainer
    {
        public int TerminationLevel { get; }
        public Permutation Remainder { get; }

        public StripContainer(int terminationLevel, Permutation remainder)
        {
            TerminationLevel = terminationLevel;
            Remainder = remainder;
        }
    }

    public static StripContainer Strip(IReadOnlyList<BSGSElement> bsgs, Permutation permutation)
    {
        for (int i = 0, size = bsgs.Count; i < size; ++i)
        {
            int beta = permutation.NewIndexOf(bsgs[i].BasePoint);
            if (!bsgs[i].BelongsToOrbit(beta))
                return new StripContainer(i, permutation);

            permutation = permutation.Composition(bsgs[i].GetInverseTransversalOf(beta));
        }

        return new StripContainer(bsgs.Count, permutation);
    }

    public static bool MembershipTest(IReadOnlyList<BSGSElement> bsgs, Permutation permutation)
    {
        StripContainer container = Strip(bsgs, permutation);
        return container.TerminationLevel == bsgs.Count && container.Remainder.IsIdentity();
    }

    public static BigInteger CalculateOrder(IReadOnlyList<BSGSElement> bsgs)
    {
        BigInteger order = BigInteger.One;
        foreach (var element in bsgs)
        {
            order *= element.OrbitSize;
        }

        return order;
    }

    public static BigInteger CalculateOrderFromCandidates(IEnumerable<BSGSCandidateElement> bsgsCandidates)
    {
        return CalculateOrder(AsBSGSList(new List<BSGSCandidateElement>(bsgsCandidates)));
    }

    public static List<BSGSCandidateElement> AsBSGSCandidatesList(IEnumerable<BSGSElement> bsgs)
    {
        var result = new List<BSGSCandidateElement>();
        foreach (var element in bsgs)
        {
            result.Add(element.AsBSGSCandidateElement());
        }

        return result;
    }

    public static List<BSGSElement> AsBSGSList(IEnumerable<BSGSCandidateElement> candidates)
    {
        var result = new List<BSGSElement>();
        foreach (var candidate in candidates)
        {
            result.Add(candidate.AsBSGSElement());
        }

        return result;
    }

    public static List<BSGSCandidateElement> Clone(IReadOnlyList<BSGSCandidateElement> bsgsCandidate)
    {
        var copy = new List<BSGSCandidateElement>(bsgsCandidate.Count);
        foreach (var element in bsgsCandidate)
        {
            copy.Add(element.Clone());
        }

        return copy;
    }

    public static int[] GetBaseAsArray(IReadOnlyList<BSGSElement> bsgs)
    {
        int[] baseArray = new int[bsgs.Count];
        for (int i = 0; i < baseArray.Length; ++i)
        {
            baseArray[i] = bsgs[i].BasePoint;
        }

        return baseArray;
    }

    public static void Rebase(List<BSGSCandidateElement> bsgs, int[] newBase)
    {
        if (bsgs.Count == 0 || newBase == null)
            return;

        int limit = Math.Min(bsgs.Count, newBase.Length);
        for (int i = 0; i < limit; ++i)
        {
            int newBasePoint = newBase[i];
            if (bsgs[i].BasePoint != newBasePoint)
                ChangeBasePointWithTranspositions(bsgs, i, newBasePoint);
        }
    }

    public static void ChangeBasePointWithTranspositions(
        List<BSGSCandidateElement> bsgs,
        int oldBasePointPosition,
        int newBasePoint)
    {
        if (bsgs[oldBasePointPosition].BasePoint == newBasePoint)
            return;

        int existingIndex = bsgs.FindIndex(e => e.BasePoint == newBasePoint);
        if (existingIndex >= 0)
        {
            while (existingIndex > oldBasePointPosition)
            {
                SwapAdjacentBasePoints(bsgs, existingIndex - 1);
                existingIndex--;
            }

            return;
        }

        int degree = bsgs[0].InternalDegree;
        bsgs.Insert(oldBasePointPosition + 1, new BSGSCandidateElement(newBasePoint, new List<Permutation>(), degree));

        int insertIndex = oldBasePointPosition + 1;
        while (insertIndex > oldBasePointPosition)
        {
            SwapAdjacentBasePoints(bsgs, insertIndex - 1);
            insertIndex--;
        }
    }

    private static void SwapAdjacentBasePoints(List<BSGSCandidateElement> bsgs, int index)
    {
        (bsgs[index], bsgs[index + 1]) = (bsgs[index + 1], bsgs[index]);
    }

    public static void RemoveRedundantGenerators(List<BSGSCandidateElement> bsgsCandidate)
    {
        if (bsgsCandidate.Count <= 1)
            return;

        for (int i = bsgsCandidate.Count - 1; i >= 0; --i)
        {
            if (bsgsCandidate[i].StabilizerGeneratorsReference.Count == 0)
                bsgsCandidate.RemoveAt(i);
        }
    }

    public static void SchreierSimsAlgorithm(List<BSGSCandidateElement> bsgsCandidate)
    {
        // Placeholder implementation for now. Actual Schreier-Sims logic should be ported from Java version.
        if (bsgsCandidate == null || bsgsCandidate.Count == 0)
            return;

        foreach (var candidate in bsgsCandidate)
        {
            candidate.InternalDegree = Math.Max(candidate.InternalDegree, Permutations.InternalDegree(candidate.GetStabilizersOfThisBasePoint()));
        }
    }
}
