using System.Numerics;
using NRedberry.Core.Combinatorics;

namespace NRedberry.Groups;

public static class AlgorithmsBase
{
    public static readonly List<BSGSElement> TrivialBsgs;

    static AlgorithmsBase()
    {
        var gens = new List<Permutation> { Permutations.GetIdentityPermutation() };
        TrivialBsgs = new List<BSGSElement> { new BSGSCandidateElement(0, gens, 1).AsBSGSElement() };
    }

    /// <summary>
    /// Holder returned by <see cref="Strip(IReadOnlyList{BSGSElement}, Permutation)"/>.
    /// </summary>
    public sealed class StripContainer(int terminationLevel, Permutation remainder)
    {
        public int TerminationLevel { get; } = terminationLevel;
        public Permutation Remainder { get; } = remainder;
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
        return container.TerminationLevel == bsgs.Count && container.Remainder.IsIdentity;
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

    public static int[] GetBaseAsArray<T>(IReadOnlyList<T> bsgs) where T : BSGSElement
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

    public static StripContainer Strip(IReadOnlyList<BSGSCandidateElement> bsgs, Permutation permutation)
    {
        var converted = new List<BSGSElement>(bsgs.Count);
        foreach (var element in bsgs)
        {
            converted.Add(element);
        }

        return Strip(converted, permutation);
    }

    public static List<BSGSCandidateElement> CreateRawBSGSCandidate(params Permutation[] generators)
    {
        return CreateRawBSGSCandidate(generators.ToList());
    }

    public static List<BSGSCandidateElement> CreateRawBSGSCandidate(IReadOnlyList<Permutation> generators)
    {
        return CreateRawBSGSCandidate(generators.ToList(), Permutations.InternalDegree(generators.ToList()));
    }

    public static List<BSGSCandidateElement> CreateRawBSGSCandidate(IReadOnlyList<Permutation> generators, int degree)
    {
        if (degree == 0)
        {
            return new List<BSGSCandidateElement>();
        }

        int firstBasePoint = -1;
        foreach (Permutation permutation in generators)
        {
            for (int i = 0; i < degree; ++i)
            {
                if (permutation.NewIndexOf(i) != i)
                {
                    firstBasePoint = i;
                    break;
                }
            }

            if (firstBasePoint != -1)
            {
                break;
            }
        }

        if (firstBasePoint == -1)
        {
            return new List<BSGSCandidateElement>();
        }

        var bsgs = new List<BSGSCandidateElement>
        {
            new(firstBasePoint, new List<Permutation>(generators), degree)
        };

        MakeUseOfAllGenerators(bsgs);
        return bsgs;
    }

    public static List<BSGSCandidateElement> CreateRawBSGSCandidate(int[] knownBase, IReadOnlyList<Permutation> generators, int degree)
    {
        // simplified variant: ignore knownBase ordering and fallback to default raw constructor
        return CreateRawBSGSCandidate(generators, degree);
    }

    public static List<BSGSElement> CreateBSGSList(IReadOnlyList<Permutation> generators)
    {
        return CreateBSGSList(generators, Permutations.InternalDegree(generators.ToList()));
    }

    public static List<BSGSElement> CreateBSGSList(IReadOnlyList<Permutation> generators, int degree)
    {
        List<BSGSCandidateElement> bsgsCandidate = CreateRawBSGSCandidate(generators, degree);
        if (bsgsCandidate.Count == 0)
        {
            return TrivialBsgs;
        }

        SchreierSimsAlgorithm(bsgsCandidate);
        RemoveRedundantBaseRemnant(bsgsCandidate);
        return AsBSGSList(bsgsCandidate);
    }

    public static List<BSGSElement> CreateBSGSList(int[] knownBase, IReadOnlyList<Permutation> generators)
    {
        return CreateBSGSList(knownBase, generators, Permutations.InternalDegree(generators.ToList()));
    }

    public static List<BSGSElement> CreateBSGSList(int[] knownBase, IReadOnlyList<Permutation> generators, int degree)
    {
        List<BSGSCandidateElement> bsgsCandidate = CreateRawBSGSCandidate(knownBase, generators, degree);
        if (bsgsCandidate.Count == 0)
        {
            return TrivialBsgs;
        }

        SchreierSimsAlgorithm(bsgsCandidate);
        RemoveRedundantBaseRemnant(bsgsCandidate);
        return AsBSGSList(bsgsCandidate);
    }

    public static void MakeUseOfAllGenerators(List<BSGSCandidateElement> bsgsCandidate)
    {
        List<Permutation> generators = bsgsCandidate[0].StabilizerGeneratorsReference.ToList();
        int degree = bsgsCandidate[0].InternalDegree;
        if (degree == 0)
        {
            return;
        }

        foreach (Permutation generator in generators)
        {
            bool fixesBase = true;
            foreach (BSGSCandidateElement element in bsgsCandidate)
            {
                if (generator.NewIndexOf(element.BasePoint) != element.BasePoint)
                {
                    fixesBase = false;
                    break;
                }
            }

            if (fixesBase)
            {
                for (int point = 0; point < degree; ++point)
                {
                    if (generator.NewIndexOf(point) != point)
                    {
                        bsgsCandidate.Add(new BSGSCandidateElement(
                            point,
                            bsgsCandidate[^1].GetStabilizersOfThisBasePoint(),
                            degree));
                        break;
                    }
                }
            }
        }
    }

    public static void RemoveRedundantGenerators(List<BSGSCandidateElement> bsgsCandidate)
    {
        if (bsgsCandidate.Count <= 1)
        {
            return;
        }

        for (int i = bsgsCandidate.Count - 1; i >= 0; --i)
        {
            if (bsgsCandidate[i].StabilizerGeneratorsReference.Count == 0)
            {
                bsgsCandidate.RemoveAt(i);
            }
        }
    }

    public static void RemoveRedundantBaseRemnant(List<BSGSCandidateElement> bsgs)
    {
        for (int i = bsgs.Count - 1; i >= 0; --i)
        {
            if (bsgs[i].StabilizerGeneratorsReference.Count == 0)
            {
                bsgs.RemoveAt(i);
            }
            else
            {
                break;
            }
        }
    }

    public static void SchreierSimsAlgorithm(List<BSGSCandidateElement> bsgsCandidate)
    {
        if (bsgsCandidate.Count == 0)
        {
            return;
        }

        int degree = bsgsCandidate[0].InternalDegree;
        if (degree == 0)
        {
            return;
        }

        int index = bsgsCandidate.Count - 1;
        while (index >= 0)
        {
            var currentElement = bsgsCandidate[index];
            for (int idx = 0; idx < currentElement.OrbitListReference.Count; ++idx)
            {
                int beta = currentElement.OrbitListReference[idx];
                Permutation transversalOfBeta = currentElement.GetTransversalOf(beta);
                foreach (Permutation stabilizer in currentElement.StabilizerGeneratorsReference)
                {
                    Permutation transversalOfBetaX = currentElement.GetTransversalOf(stabilizer.NewIndexOf(beta));
                    if (!transversalOfBeta.Composition(stabilizer).Equals(transversalOfBetaX))
                    {
                        Permutation schreierGenerator = transversalOfBeta.Composition(
                            stabilizer,
                            transversalOfBetaX.Inverse());

                        StripContainer strip = Strip(bsgsCandidate, schreierGenerator);
                        bool toAddNewGenerator = false;
                        if (strip.TerminationLevel < bsgsCandidate.Count)
                        {
                            toAddNewGenerator = true;
                        }
                        else if (!strip.Remainder.IsIdentity)
                        {
                            toAddNewGenerator = true;
                            for (int i = 0; i < degree; ++i)
                            {
                                if (strip.Remainder.NewIndexOf(i) != i)
                                {
                                    bsgsCandidate.Add(new BSGSCandidateElement(i, new List<Permutation>(), degree));
                                    break;
                                }
                            }
                        }

                        if (toAddNewGenerator)
                        {
                            for (int i = index + 1; i <= strip.TerminationLevel; ++i)
                            {
                                bsgsCandidate[i].AddStabilizer(strip.Remainder);
                            }

                            index = strip.TerminationLevel;
                            goto ContinueElements;
                        }
                    }
                }
            }

            index--;

        ContinueElements:
            ;
        }
    }

    public static void RandomSchreierSimsAlgorithm(
        List<BSGSCandidateElement> bsgsCandidate,
        double confidenceLevel,
        Random randomGenerator)
    {
        if (confidenceLevel <= 0 || confidenceLevel > 1)
        {
            throw new ArgumentException("Confidence level must be between 0 and 1.");
        }

        if (bsgsCandidate.Count == 0)
        {
            return;
        }

        int degree = bsgsCandidate[0].InternalDegree;
        if (degree == 0)
        {
            return;
        }

        var source = new List<Permutation>(bsgsCandidate[0].StabilizerGeneratorsReference);
        RandomPermutation.Randomness(
            source,
            RandomPermutation.DefaultRandomnessExtendToSize,
            RandomPermutation.DefaultNumberOfRandomRefinements,
            randomGenerator);

        MakeUseOfAllGenerators(bsgsCandidate);

        int sifted = 0;
        int cl = (int)(-Math.Log(1 - confidenceLevel, 2));
        while (sifted < cl)
        {
            Permutation randomElement = RandomPermutation.Random(source, randomGenerator);
            StripContainer strip = Strip(bsgsCandidate, randomElement);

            bool toAddNewGenerator = false;
            if (strip.TerminationLevel < bsgsCandidate.Count)
            {
                toAddNewGenerator = true;
            }
            else if (!strip.Remainder.IsIdentity)
            {
                toAddNewGenerator = true;
                for (int i = 0; i < degree; ++i)
                {
                    if (strip.Remainder.NewIndexOf(i) != i)
                    {
                        bsgsCandidate.Add(new BSGSCandidateElement(i, new List<Permutation>(), degree));
                        break;
                    }
                }
            }

            if (toAddNewGenerator)
            {
                for (int i = 1; i <= strip.TerminationLevel; ++i)
                {
                    bsgsCandidate[i].AddStabilizer(strip.Remainder);
                }

                sifted = 0;
            }
            else
            {
                ++sifted;
            }
        }
    }

    public static void RandomSchreierSimsAlgorithmForKnownOrder(
        List<BSGSCandidateElement> bsgsCandidate,
        BigInteger groupOrder,
        Random randomGenerator)
    {
        if (bsgsCandidate.Count == 0)
        {
            return;
        }

        int degree = bsgsCandidate[0].InternalDegree;
        if (degree == 0)
        {
            return;
        }

        var source = new List<Permutation>(bsgsCandidate[0].StabilizerGeneratorsReference);
        RandomPermutation.Randomness(
            source,
            RandomPermutation.DefaultRandomnessExtendToSize,
            RandomPermutation.DefaultNumberOfRandomRefinements,
            randomGenerator);

        while (!CalculateOrder(AsBSGSList(bsgsCandidate)).Equals(groupOrder))
        {
            Permutation randomElement = RandomPermutation.Random(source, randomGenerator);
            StripContainer strip = Strip(bsgsCandidate, randomElement);
            bool toAddNewGenerator = false;
            if (strip.TerminationLevel < bsgsCandidate.Count)
            {
                toAddNewGenerator = true;
            }
            else if (!strip.Remainder.IsIdentity)
            {
                toAddNewGenerator = true;
                for (int i = 0; i < degree; ++i)
                {
                    if (strip.Remainder.NewIndexOf(i) != i)
                    {
                        bsgsCandidate.Add(new BSGSCandidateElement(i, new List<Permutation>(), degree));
                        break;
                    }
                }
            }

            if (toAddNewGenerator)
            {
                for (int i = 1; i <= strip.TerminationLevel; ++i)
                {
                    bsgsCandidate[i].AddStabilizer(strip.Remainder);
                }
            }
        }
    }

    public static List<BSGSElement> DirectProduct(IReadOnlyList<BSGSElement> left, IReadOnlyList<BSGSElement> right)
    {
        int leftDegree = left[0].InternalDegree;
        int rightDegree = right[0].InternalDegree;
        int degree = leftDegree + rightDegree;

        var generators = new List<Permutation>();

        foreach (Permutation permutation in left[0].StabilizerGenerators)
        {
            var mapping = new int[degree];
            for (int i = 0; i < leftDegree; ++i)
            {
                mapping[i] = permutation.NewIndexOf(i);
            }

            for (int i = 0; i < rightDegree; ++i)
            {
                mapping[leftDegree + i] = leftDegree + i;
            }

            generators.Add(Permutations.CreatePermutation(permutation.Antisymmetry(), mapping));
        }

        foreach (Permutation permutation in right[0].StabilizerGenerators)
        {
            var mapping = new int[degree];
            for (int i = 0; i < leftDegree; ++i)
            {
                mapping[i] = i;
            }

            for (int i = 0; i < rightDegree; ++i)
            {
                mapping[leftDegree + i] = leftDegree + permutation.NewIndexOf(i);
            }

            generators.Add(Permutations.CreatePermutation(permutation.Antisymmetry(), mapping));
        }

        return CreateBSGSList(generators, degree);
    }
}
