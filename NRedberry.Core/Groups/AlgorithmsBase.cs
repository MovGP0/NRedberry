using System.Collections;
using System.Numerics;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Groups;

public static class AlgorithmsBase
{
    public static readonly List<BSGSElement> TrivialBsgs;

    public const int SmallDegreeThreshold = 100;

    private static readonly List<BSGSElement>[] s_cachedSymmetricGroups = new List<BSGSElement>[SmallDegreeThreshold];
    private static readonly List<BSGSElement>[] s_cachedAntisymmetricGroups = new List<BSGSElement>[SmallDegreeThreshold];
    private static readonly List<BSGSElement>[] s_cachedAlternatingGroups = new List<BSGSElement>[SmallDegreeThreshold];

    static AlgorithmsBase()
    {
        var gens = new List<Permutation> { Permutations.GetIdentityPermutation() };
        TrivialBsgs = new List<BSGSElement> { new BSGSCandidateElement(0, gens, 1).AsBSGSElement() };
    }

    /// <summary>
    /// Calculates representation of specified permutation in terms of specified BSGS.
    /// </summary>
    public static StripContainer Strip(IReadOnlyList<BSGSElement> bsgs, Permutation permutation)
    {
        for (int i = 0, size = bsgs.Count; i < size; ++i)
        {
            int beta = permutation.NewIndexOf(bsgs[i].BasePoint);
            if (!bsgs[i].BelongsToOrbit(beta))
            {
                return new StripContainer(i, permutation);
            }

            permutation = permutation.Composition(bsgs[i].GetInverseTransversalOf(beta));
        }

        return new StripContainer(bsgs.Count, permutation);
    }

    /// <summary>
    /// Returns whether specified permutation belongs to permutation group defined by specified BSGS.
    /// </summary>
    public static bool MembershipTest(IReadOnlyList<BSGSElement> bsgs, Permutation permutation)
    {
        StripContainer container = Strip(bsgs, permutation);
        return container.TerminationLevel == bsgs.Count && container.Remainder.IsIdentity;
    }

    public static List<BSGSCandidateElement> CreateRawBSGSCandidate(params Permutation[] generators)
    {
        return CreateRawBSGSCandidate(new List<Permutation>(generators));
    }

    public static List<BSGSCandidateElement> CreateRawBSGSCandidate(IReadOnlyList<Permutation> generators)
    {
        return CreateRawBSGSCandidate(generators, Permutations.InternalDegree(generators));
    }

    public static List<BSGSCandidateElement> CreateRawBSGSCandidate(IReadOnlyList<Permutation> generators, int degree)
    {
        if (degree == 0)
        {
            return [];
        }

        int firstBasePoint = -1;
        foreach (Permutation permutation in generators)
        {
            for (int i = 0; i < degree; ++i)
            {
                if (permutation.NewIndexOf(i) != i)
                {
                    firstBasePoint = i;
                    goto FoundBasePoint;
                }
            }
        }

    FoundBasePoint:
        if (firstBasePoint == -1)
        {
            return [];
        }

        List<BSGSCandidateElement> bsgs =
        [
            new BSGSCandidateElement(firstBasePoint, new List<Permutation>(generators), degree)
        ];

        MakeUseOfAllGenerators(bsgs);
        return bsgs;
    }

    public static List<BSGSCandidateElement> CreateRawBSGSCandidate(int[] knownBase, IReadOnlyList<Permutation> generators)
    {
        return CreateRawBSGSCandidate(knownBase, generators, Permutations.InternalDegree(generators));
    }

    public static List<BSGSCandidateElement> CreateRawBSGSCandidate(int[] knownBase, IReadOnlyList<Permutation> generators, int degree)
    {
        if (degree == 0)
        {
            return [];
        }

        List<int> basePoints = new List<int>(knownBase);
        for (int i = basePoints.Count - 1; i >= 0; --i)
        {
            int point = basePoints[i];
            bool fixedByAll = true;
            foreach (Permutation permutation in generators)
            {
                if (permutation.NewIndexOf(point) != point)
                {
                    fixedByAll = false;
                    break;
                }
            }

            if (fixedByAll)
            {
                basePoints.RemoveAt(i);
            }
        }

        if (basePoints.Count == 0)
        {
            return CreateRawBSGSCandidate(generators, degree);
        }

        List<BSGSCandidateElement> bsgs = new(basePoints.Count)
        {
            new BSGSCandidateElement(basePoints[0], new List<Permutation>(generators), degree)
        };

        for (int i = 1; i < basePoints.Count; ++i)
        {
            List<Permutation> stabilizerGenerators = [];
            foreach (Permutation generator in generators)
            {
                bool fixesPrevious = true;
                for (int j = 0; j < i; ++j)
                {
                    if (generator.NewIndexOf(basePoints[j]) != basePoints[j])
                    {
                        fixesPrevious = false;
                        break;
                    }
                }

                if (fixesPrevious)
                {
                    stabilizerGenerators.Add(generator);
                }
            }

            bsgs.Add(new BSGSCandidateElement(basePoints[i], stabilizerGenerators, degree));
        }

        MakeUseOfAllGenerators(bsgs);
        return bsgs;
    }

    public static List<BSGSElement> CreateBSGSList(IReadOnlyList<Permutation> generators)
    {
        return CreateBSGSList(generators, Permutations.InternalDegree(generators));
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
        return CreateBSGSList(knownBase, generators, Permutations.InternalDegree(generators));
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
        List<Permutation> generators = new(bsgsCandidate[0].StabilizerGeneratorsReference);
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

            if (!fixesBase)
            {
                continue;
            }

            for (int point = 0; point < degree; ++point)
            {
                if (generator.NewIndexOf(point) != point)
                {
                    bsgsCandidate.Add(new BSGSCandidateElement(
                        point,
                        bsgsCandidate[^1].GetStabilizersOfThisBasePoint(),
                        degree));
                }
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

        BSGSCandidateElement currentElement;
        int index = bsgsCandidate.Count - 1;
        Elements:
        while (index >= 0)
        {
            currentElement = bsgsCandidate[index];
            for (int indexInOrbit = 0, sizeOfOrbit = currentElement.OrbitListReference.Count;
                 indexInOrbit < sizeOfOrbit; ++indexInOrbit)
            {
                int beta = currentElement.OrbitListReference[indexInOrbit];
                Permutation transversalOfBeta = currentElement.GetTransversalOf(beta);

                foreach (Permutation stabilizer in currentElement.StabilizerGeneratorsReference)
                {
                    Permutation transversalOfBetaX = currentElement.GetTransversalOf(stabilizer.NewIndexOf(beta));
                    if (!transversalOfBeta.Composition(stabilizer).Equals(transversalOfBetaX))
                    {
                        Permutation schreierGenerator = transversalOfBeta.Composition(stabilizer, transversalOfBetaX.Inverse());
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
                            goto Elements;
                        }
                    }
                }
            }

            --index;
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

        List<Permutation> source = new(bsgsCandidate[0].StabilizerGeneratorsReference);
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

        List<Permutation> source = new(bsgsCandidate[0].StabilizerGeneratorsReference);
        RandomPermutation.Randomness(
            source,
            RandomPermutation.DefaultRandomnessExtendToSize,
            RandomPermutation.DefaultNumberOfRandomRefinements,
            randomGenerator);

        while (!CalculateOrder(bsgsCandidate).Equals(groupOrder))
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

    public static BigInteger CalculateOrder(IReadOnlyList<BSGSElement> bsgs)
    {
        return CalculateOrder(bsgs, 0);
    }

    public static BigInteger CalculateOrder(IReadOnlyList<BSGSElement> bsgs, int from)
    {
        BigInteger order = BigInteger.One;
        for (int i = from; i < bsgs.Count; ++i)
        {
            order *= bsgs[i].OrbitSize;
        }

        return order;
    }

    public static BigInteger CalculateOrderFromCandidates(IEnumerable<BSGSCandidateElement> bsgsCandidates)
    {
        return CalculateOrder(AsBSGSList(new List<BSGSCandidateElement>(bsgsCandidates)));
    }

    public static void RemoveRedundantGenerators(List<BSGSCandidateElement> bsgsCandidate)
    {
        if (bsgsCandidate.Count <= 1)
        {
            return;
        }

        int degree = bsgsCandidate[0].InternalDegree;
        if (degree == 0)
        {
            return;
        }

        for (int i = bsgsCandidate.Count - 2; i > 0; --i)
        {
            BSGSCandidateElement element = bsgsCandidate[i];
            List<Permutation> stabilizers = new(element.StabilizerGeneratorsReference);
            bool removed = false;

            for (int index = 0; index < stabilizers.Count;)
            {
                Permutation current = stabilizers[index];
                if (current.IsIdentity)
                {
                    stabilizers.RemoveAt(index);
                    removed = true;
                    continue;
                }

                if (current.NewIndexOf(element.BasePoint) == element.BasePoint
                    && bsgsCandidate[i + 1].StabilizerGeneratorsReference.Contains(current))
                {
                    ++index;
                    continue;
                }

                List<Permutation> tempStabilizers = new List<Permutation>(stabilizers);
                tempStabilizers.Remove(current);

                if (Permutations.GetOrbitSize(tempStabilizers, element.BasePoint, degree) == element.OrbitSize)
                {
                    int[] subBase = GetBaseAsArray(bsgsCandidate, i);
                    List<BSGSCandidateElement> sub = CreateRawBSGSCandidate(subBase, tempStabilizers, degree);
                    if (sub.Count == 0)
                    {
                        if (CalculateOrder(bsgsCandidate, i) == BigInteger.One)
                        {
                            stabilizers.RemoveAt(index);
                            removed = true;
                            continue;
                        }

                        ++index;
                        continue;
                    }

                    SchreierSimsAlgorithm(sub);
                    if (!CalculateOrder(bsgsCandidate, i).Equals(CalculateOrder(sub)))
                    {
                        ++index;
                        continue;
                    }

                    bool ok = true;
                    foreach (Permutation stabGen in bsgsCandidate[i + 1].StabilizerGeneratorsReference)
                    {
                        if (!MembershipTest(sub, stabGen))
                        {
                            ok = false;
                            break;
                        }
                    }

                    if (!ok)
                    {
                        ++index;
                        continue;
                    }

                    stabilizers.RemoveAt(index);
                    removed = true;
                    continue;
                }

                ++index;
            }

            if (removed)
            {
                var rebuilt = new BSGSCandidateElement(element.BasePoint, stabilizers, element.InternalDegree);
                rebuilt.InternalDegree = element.InternalDegree;
                bsgsCandidate[i] = rebuilt;
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

    public static bool IsBSGS(IReadOnlyList<BSGSElement> bsgsCandidate)
    {
        if (bsgsCandidate.Count == 0)
        {
            return true;
        }

        BSGSElement currentElement;
        int index = bsgsCandidate.Count - 1;
        while (index >= 0)
        {
            currentElement = bsgsCandidate[index];
            for (int indexInOrbit = 0, sizeOfOrbit = currentElement.OrbitListReference.Count;
                 indexInOrbit < sizeOfOrbit; ++indexInOrbit)
            {
                int beta = currentElement.OrbitListReference[indexInOrbit];
                Permutation transversalOfBeta = currentElement.GetTransversalOf(beta);

                foreach (Permutation stabilizer in currentElement.StabilizerGeneratorsReference)
                {
                    Permutation transversalOfBetaX = currentElement.GetTransversalOf(stabilizer.NewIndexOf(beta));
                    if (!transversalOfBeta.Composition(stabilizer).Equals(transversalOfBetaX))
                    {
                        Permutation schreierGenerator = transversalOfBeta.Composition(stabilizer, transversalOfBetaX.Inverse());
                        StripContainer strip = Strip(bsgsCandidate, schreierGenerator);
                        if (strip.TerminationLevel < bsgsCandidate.Count || !strip.Remainder.IsIdentity)
                        {
                            return false;
                        }
                    }
                }
            }

            --index;
        }

        return true;
    }

    public static bool IsBSGS(IReadOnlyList<BSGSElement> bsgsCandidate, double confidenceLevel, Random randomGenerator)
    {
        if (confidenceLevel <= 0 || confidenceLevel > 1)
        {
            throw new ArgumentException("Confidence level must be between 0 and 1.");
        }

        List<Permutation> source = new(bsgsCandidate[0].StabilizerGeneratorsReference);
        RandomPermutation.Randomness(
            source,
            RandomPermutation.DefaultRandomnessExtendToSize,
            RandomPermutation.DefaultNumberOfRandomRefinements,
            randomGenerator);
        source = new List<Permutation>(source);

        int sifted = 0;
        int cl = (int)(-Math.Log(1 - confidenceLevel, 2));
        while (sifted < cl)
        {
            Permutation randomElement = RandomPermutation.Random(source, randomGenerator);
            StripContainer strip = Strip(bsgsCandidate, randomElement);
            if (strip.TerminationLevel < bsgsCandidate.Count || !strip.Remainder.IsIdentity)
            {
                return false;
            }

            ++sifted;
        }

        return true;
    }

    public static long NumberOfStrongGenerators(IReadOnlyList<BSGSElement> bsgs)
    {
        long num = 0;
        foreach (BSGSElement element in bsgs)
        {
            num += element.StabilizerGeneratorsReference.Count;
        }

        return num;
    }

    public static void SwapAdjacentBasePoints(List<BSGSCandidateElement> bsgs, int i)
    {
        if (i > bsgs.Count - 2)
        {
            throw new ArgumentOutOfRangeException(nameof(i));
        }

        int ithBeta = bsgs[i].BasePoint;
        int jthBeta = bsgs[i + 1].BasePoint;
        int effectiveDegree = Math.Max(bsgs[0].InternalDegree, Math.Max(ithBeta + 1, jthBeta + 1));

        int d = Permutations.GetOrbitSize(bsgs[i].StabilizerGeneratorsReference, bsgs[i + 1].BasePoint, effectiveDegree);
        int s = (int)(((long)bsgs[i].OrbitSize * bsgs[i + 1].OrbitSize) / d);

        List<Permutation> newStabilizers = i == bsgs.Count - 2
            ? []
            : new List<Permutation>(bsgs[i + 2].StabilizerGeneratorsReference);

        BitArray allowedPoints = new BitArray(effectiveDegree);
        SetAll(allowedPoints, bsgs[i].OrbitListReference, true);
        allowedPoints.Set(ithBeta, false);
        allowedPoints.Set(jthBeta, false);

        BSGSCandidateElement newOrbitStabilizer = new(ithBeta, newStabilizers, effectiveDegree);

    MainLoop:
        while (newOrbitStabilizer.OrbitSize != s)
        {
            int nextBasePoint = -1;
            while ((nextBasePoint = NextSetBit(allowedPoints, nextBasePoint + 1)) != -1)
            {
                Permutation transversal = bsgs[i].GetTransversalOf(nextBasePoint);
                int newIndexUnderInverse = transversal.NewIndexOfUnderInverse(jthBeta);
                if (!bsgs[i + 1].BelongsToOrbit(newIndexUnderInverse))
                {
                    List<int> toRemove = Permutations.GetOrbitList(
                        newOrbitStabilizer.StabilizerGeneratorsReference,
                        nextBasePoint,
                        effectiveDegree);
                    SetAll(allowedPoints, toRemove, false);
                }
                else
                {
                    Permutation newStabilizer = bsgs[i + 1].GetTransversalOf(newIndexUnderInverse)
                        .Composition(transversal);
                    if (!newOrbitStabilizer.BelongsToOrbit(newStabilizer.NewIndexOf(ithBeta)))
                    {
                        newOrbitStabilizer.AddStabilizer(newStabilizer);
                        List<int> toRemove = Permutations.GetOrbitList(
                            newOrbitStabilizer.StabilizerGeneratorsReference,
                            nextBasePoint,
                            effectiveDegree);
                        SetAll(allowedPoints, toRemove, false);
                        goto MainLoop;
                    }
                }
            }
        }

        BSGSCandidateElement ith = new(
            bsgs[i + 1].BasePoint,
            new List<Permutation>(bsgs[i].StabilizerGeneratorsReference),
            effectiveDegree);
        BSGSCandidateElement jth = new(
            bsgs[i].BasePoint,
            newStabilizers,
            effectiveDegree);
        bsgs[i] = ith;
        bsgs[i + 1] = jth;
    }

    public static void ChangeBasePointWithTranspositions(List<BSGSCandidateElement> bsgs, int oldBasePointPosition, int newBasePoint)
    {
        if (bsgs[oldBasePointPosition].BasePoint == newBasePoint)
        {
            return;
        }

        for (int index = oldBasePointPosition + 1; index < bsgs.Count; ++index)
        {
            if (bsgs[index].BasePoint == newBasePoint)
            {
                while (index > oldBasePointPosition)
                {
                    SwapAdjacentBasePoints(bsgs, --index);
                }

                return;
            }
        }

        int insertionPosition = oldBasePointPosition + 1;
        for (; insertionPosition < bsgs.Count; ++insertionPosition)
        {
            bool fixesNewBase = true;
            foreach (Permutation permutation in bsgs[insertionPosition].StabilizerGeneratorsReference)
            {
                if (permutation.NewIndexOf(newBasePoint) != newBasePoint)
                {
                    fixesNewBase = false;
                    break;
                }
            }

            if (fixesNewBase)
            {
                break;
            }
        }

        int degree = bsgs[0].InternalDegree;
        if (insertionPosition == bsgs.Count)
        {
            bsgs.Add(new BSGSCandidateElement(newBasePoint, new List<Permutation>(), degree));
        }
        else if (bsgs[insertionPosition].BasePoint != newBasePoint)
        {
            bsgs.Insert(
                insertionPosition,
                new BSGSCandidateElement(
                    newBasePoint,
                    new List<Permutation>(bsgs[insertionPosition].StabilizerGeneratorsReference),
                    degree));
        }

        while (insertionPosition > oldBasePointPosition)
        {
            SwapAdjacentBasePoints(bsgs, --insertionPosition);
        }
    }

    public static void RebaseWithTranspositions(List<BSGSCandidateElement> bsgs, int[] newBase)
    {
        for (int i = 0; i < newBase.Length && i < bsgs.Count; ++i)
        {
            int newBasePoint = newBase[i];
            if (bsgs[i].BasePoint != newBasePoint)
            {
                ChangeBasePointWithTranspositions(bsgs, i, newBasePoint);
            }
        }

        RemoveRedundantBaseRemnant(bsgs);
    }

    public static void RebaseWithConjugationAndTranspositions(List<BSGSCandidateElement> bsgs, int[] newBase)
    {
        Permutation conjugation = Permutations.GetIdentityPermutation();
        int degree = bsgs[0].InternalDegree;
        int positionOfFirstChanged = -1;

        for (int i = 0; i < newBase.Length && i < bsgs.Count; ++i)
        {
            int newBasePoint = conjugation.NewIndexOfUnderInverse(newBase[i]);
            if (bsgs[i].BasePoint == newBasePoint)
            {
                continue;
            }

            if (positionOfFirstChanged == -1)
            {
                positionOfFirstChanged = i;
            }

            if (bsgs[i].BelongsToOrbit(newBasePoint))
            {
                Permutation transversal = bsgs[i].GetTransversalOf(newBasePoint);
                conjugation = transversal.Composition(conjugation);
                continue;
            }

            ChangeBasePointWithTranspositions(bsgs, i, newBasePoint);
        }

        RemoveRedundantBaseRemnant(bsgs);
        if (positionOfFirstChanged == -1 || bsgs.Count <= positionOfFirstChanged)
        {
            return;
        }

        if (!conjugation.IsIdentity)
        {
            Permutation inverse = conjugation.Inverse();
            for (int i = positionOfFirstChanged; i < bsgs.Count; ++i)
            {
                BSGSCandidateElement element = bsgs[i];
                List<Permutation> newStabilizers = new(element.StabilizerGeneratorsReference.Count);
                foreach (Permutation oldStabilizer in element.StabilizerGeneratorsReference)
                {
                    newStabilizers.Add(inverse.Composition(oldStabilizer, conjugation));
                }

                int newBasePoint = conjugation.NewIndexOf(element.BasePoint);
                bsgs[i] = new BSGSCandidateElement(newBasePoint, newStabilizers, degree);
            }
        }

        RemoveRedundantBaseRemnant(bsgs);
    }

    public static void RebaseFromScratch(List<BSGSCandidateElement> bsgs, int[] newBase)
    {
        List<BSGSCandidateElement> newBSGS = CreateRawBSGSCandidate(
            newBase,
            new List<Permutation>(bsgs[0].StabilizerGeneratorsReference),
            bsgs[0].InternalDegree);
        if (newBSGS.Count == 0)
        {
            return;
        }

        BigInteger order = CalculateOrder(bsgs);
        RandomSchreierSimsAlgorithmForKnownOrder(newBSGS, order, Random.Shared);
        int i = 0;
        for (; i < newBSGS.Count && i < bsgs.Count; ++i)
        {
            bsgs[i] = newBSGS[i];
        }

        if (i < newBSGS.Count)
        {
            for (; i < newBSGS.Count; ++i)
            {
                bsgs.Add(newBSGS[i]);
            }
        }

        if (i < bsgs.Count)
        {
            for (int j = bsgs.Count - 1; j >= i; --j)
            {
                bsgs.RemoveAt(j);
            }
        }
    }

    public static void Rebase(List<BSGSCandidateElement> bsgs, int[] newBase)
    {
        RebaseWithConjugationAndTranspositions(bsgs, newBase);
    }

    public static List<BSGSElement> DirectProduct(IReadOnlyList<BSGSElement> bsgs1, IReadOnlyList<BSGSElement> bsgs2)
    {
        int degree1 = bsgs1[0].InternalDegree;
        int degree2 = bsgs2[0].InternalDegree;
        int degree = degree1 + degree2;

        List<BSGSElement> groupBsgsExtended = new(bsgs2.Count);
        foreach (BSGSElement element in bsgs2)
        {
            List<Permutation> stabilizers = new(element.StabilizerGeneratorsReference.Count);
            foreach (Permutation p in element.StabilizerGeneratorsReference)
            {
                stabilizers.Add(p.MoveRight(degree1));
            }

            groupBsgsExtended.Add(
                new BSGSCandidateElement(element.BasePoint + degree1, stabilizers, degree).AsBSGSElement());
        }

        List<BSGSElement> bsgs = new(bsgs1.Count + bsgs2.Count);
        foreach (BSGSElement element in bsgs1)
        {
            List<Permutation> stabilizers = new(element.StabilizerGeneratorsReference.Count + groupBsgsExtended[0].StabilizerGeneratorsReference.Count);
            stabilizers.AddRange(element.StabilizerGeneratorsReference);
            stabilizers.AddRange(groupBsgsExtended[0].StabilizerGeneratorsReference);
            bsgs.Add(new BSGSCandidateElement(element.BasePoint, stabilizers, degree).AsBSGSElement());
        }

        bsgs.AddRange(groupBsgsExtended);
        return bsgs;
    }

    public static List<BSGSCandidateElement> Union(IReadOnlyList<BSGSElement> bsgs1, IReadOnlyList<BSGSElement> bsgs2)
    {
        if (bsgs2.Count == 0)
        {
            return AsBSGSCandidatesList(bsgs1);
        }

        if (bsgs1.Count == 0)
        {
            return AsBSGSCandidatesList(bsgs2);
        }

        int[] base1 = GetBaseAsArray(bsgs1);
        int[] base2 = GetBaseAsArray(bsgs2);
        int[] @base = MathUtils.IntSetUnion(base1, base2);
        List<Permutation> generators = [];
        generators.AddRange(bsgs1[0].StabilizerGeneratorsReference);
        generators.AddRange(bsgs2[0].StabilizerGeneratorsReference);

        int degree = Math.Max(ArraysUtils.Max(@base) + 1, Permutations.InternalDegree(generators));
        List<BSGSCandidateElement> bsgs = CreateRawBSGSCandidate(@base, generators, degree);
        SchreierSimsAlgorithm(bsgs);
        return bsgs;
    }

    public static List<BSGSElement> CreateAlternatingGroupBSGS(int degree)
    {
        if (degree == 0)
        {
            throw new ArgumentException("Degree = 0.");
        }

        if (degree <= SmallDegreeThreshold)
        {
            List<BSGSElement>? bsgs = s_cachedAlternatingGroups[degree - 1];
            if (bsgs == null)
            {
                bsgs = CreateAlternatingGroupBSGSForSmallDegree(degree);
                s_cachedAlternatingGroups[degree - 1] = bsgs;
            }

            return bsgs;
        }

        return CreateAlternatingGroupBSGSForLargeDegree(degree);
    }

    internal static List<BSGSElement> CreateAlternatingGroupBSGSForSmallDegree(int degree)
    {
        if (degree < 3)
        {
            return TrivialBsgs;
        }

        List<BSGSElement> bsgs = [];
        List<Permutation> stabilizers = new(degree);
        for (int i = 0; i < degree - 2; ++i)
        {
            List<int> orbit = new(degree - i);
            for (int j = i; j < degree; ++j)
            {
                orbit.Add(j);
            }

            int[] schreierVector = new int[degree];
            Array.Fill(schreierVector, -2);
            schreierVector[i] = -1;

            int[] perm = new int[degree];
            for (int j = 1; j < i; ++j)
            {
                perm[j] = j;
            }

            for (int j = i + 3; j < degree; ++j)
            {
                perm[j] = j;
            }

            perm[i] = i + 1;
            perm[i + 1] = i + 2;
            perm[i + 2] = i;

            schreierVector[i + 1] = stabilizers.Count;
            stabilizers.Add(Permutations.CreatePermutation(perm));
            schreierVector[i + 2] = stabilizers.Count;
            stabilizers.Add(stabilizers[0].Pow(2));

            int inverseParity = 1 - ((degree - i) % 2);
            Permutation @base = inverseParity == 1 ? stabilizers[0] : stabilizers[0].Identity;
            for (int k = 3; k < degree - i; ++k)
            {
                perm = new int[degree];
                for (int j = 1; j <= i; ++j)
                {
                    perm[j] = j;
                }

                int jIndex;
                for (jIndex = i + inverseParity; jIndex < degree - k + inverseParity; ++jIndex)
                {
                    perm[jIndex] = jIndex + k - inverseParity;
                }

                for (int t = 0; jIndex < degree; ++jIndex, ++t)
                {
                    perm[jIndex] = i + inverseParity + t;
                }

                schreierVector[i + k] = stabilizers.Count;
                stabilizers.Add(@base.Composition(Permutations.CreatePermutation(perm)));
            }

            bsgs.Add(new BSGSElement(i, new List<Permutation>(stabilizers), new SchreierVector(schreierVector), orbit));
            stabilizers.Clear();
        }

        return bsgs;
    }

    internal static List<BSGSElement> CreateAlternatingGroupBSGSForLargeDegree(int degree)
    {
        if (degree < 3)
        {
            return TrivialBsgs;
        }

        List<BSGSElement> bsgs = [];
        List<Permutation> stabilizers = new(degree);
        for (int i = 0; i < degree - 2; ++i)
        {
            int[] perm = new int[degree];
            for (int j = 1; j < i; ++j)
            {
                perm[j] = j;
            }

            for (int j = i + 3; j < degree; ++j)
            {
                perm[j] = j;
            }

            perm[i] = i + 1;
            perm[i + 1] = i + 2;
            perm[i + 2] = i;

            stabilizers.Add(Permutations.CreatePermutation(perm));
            stabilizers.Add(stabilizers[0].Pow(2));

            int inverseParity = 1 - ((degree - i) % 2);
            Permutation @base = inverseParity == 1 ? stabilizers[0] : stabilizers[0].Identity;
            for (int r = degree - i - 3; r > 0; r /= 2)
            {
                int k = r + 2;
                perm = new int[degree];
                for (int j = 1; j <= i; ++j)
                {
                    perm[j] = j;
                }

                int jIndex;
                for (jIndex = i + inverseParity; jIndex < degree - k + inverseParity; ++jIndex)
                {
                    perm[jIndex] = jIndex + k - inverseParity;
                }

                for (int t = 0; jIndex < degree; ++jIndex, ++t)
                {
                    perm[jIndex] = i + inverseParity + t;
                }

                stabilizers.Add(@base.Composition(Permutations.CreatePermutation(perm)));
            }

            bsgs.Add(new BSGSCandidateElement(i, new List<Permutation>(stabilizers), degree).AsBSGSElement());
            stabilizers.Clear();
        }

        return bsgs;
    }

    public static List<BSGSElement> CreateSymmetricGroupBSGS(int degree)
    {
        if (degree == 0)
        {
            throw new ArgumentException("Degree = 0.");
        }

        if (degree <= SmallDegreeThreshold)
        {
            List<BSGSElement>? bsgs = s_cachedSymmetricGroups[degree - 1];
            if (bsgs == null)
            {
                bsgs = CreateSymmetricGroupBSGSForSmallDegree(degree);
                s_cachedSymmetricGroups[degree - 1] = bsgs;
            }

            return bsgs;
        }

        return CreateSymmetricGroupBSGSForLargeDegree(degree);
    }

    internal static List<BSGSElement> CreateSymmetricGroupBSGSForSmallDegree(int degree)
    {
        List<BSGSElement> bsgs = new(degree - 1);
        for (int i = 0; i < degree - 1; ++i)
        {
            List<int> orbit = new(degree - i);
            for (int j = i; j < degree; ++j)
            {
                orbit.Add(j);
            }

            Permutation[] stabilizers = new Permutation[degree - i - 1];
            int[] schreierVector = new int[degree];
            Array.Fill(schreierVector, -2);
            schreierVector[i] = -1;

            int c = 0;
            for (int j = i + 1; j < degree; ++j)
            {
                int[] permutation = new int[degree];
                for (int k = 1; k < degree; ++k)
                {
                    permutation[k] = k;
                }

                permutation[j] = i;
                permutation[i] = j;
                stabilizers[c] = Permutations.CreatePermutation(permutation);
                schreierVector[j] = c++;
            }

            BSGSElement element = new BSGSElement(i, new List<Permutation>(stabilizers), new SchreierVector(schreierVector), orbit);
            bsgs.Add(element);
        }

        return bsgs;
    }

    internal static List<BSGSElement> CreateSymmetricGroupBSGSForLargeDegree(int degree)
    {
        List<BSGSElement> bsgs = new(degree - 1);
        for (int i = 0; i < degree - 1; ++i)
        {
            List<int> orbit = new(degree - i);
            for (int j = i; j < degree; ++j)
            {
                orbit.Add(j);
            }

            int capacity = (int)Math.Log(degree - i, 2);
            List<Permutation> stabilizers = new(capacity);

            int[] permutation = new int[degree];
            for (int j = 1; j < degree; ++j)
            {
                permutation[j] = j;
            }

            permutation[i] = i + 1;
            permutation[i + 1] = i;
            stabilizers.Add(Permutations.CreatePermutation(permutation));

            for (int j = degree - i - 1; j > 0; j /= 2)
            {
                int image = i + j;
                permutation = new int[degree];
                for (int k = 0; k < i; ++k)
                {
                    permutation[k] = k;
                }

                int k2 = i;
                int l = 0;
                for (; l < degree - image; ++k2, ++l)
                {
                    permutation[k2] = image + l;
                }

                l = 0;
                for (; k2 < degree; ++k2)
                {
                    permutation[k2] = i + (l++);
                }

                stabilizers.Add(Permutations.CreatePermutation(permutation));
            }

            bsgs.Add(new BSGSCandidateElement(i, stabilizers, degree).AsBSGSElement());
        }

        return bsgs;
    }

    public static List<BSGSElement> CreateAntisymmetricGroupBSGS(int degree)
    {
        if (degree == 0)
        {
            throw new ArgumentException("Degree = 0.");
        }

        if (degree <= SmallDegreeThreshold)
        {
            List<BSGSElement>? bsgs = s_cachedAntisymmetricGroups[degree - 1];
            if (bsgs == null)
            {
                bsgs = ConvertToAntisymmetric(CreateSymmetricGroupBSGS(degree));
                s_cachedAntisymmetricGroups[degree - 1] = bsgs;
            }

            return bsgs;
        }

        return ConvertToAntisymmetric(CreateSymmetricGroupBSGS(degree));
    }

    private static List<BSGSElement> ConvertToAntisymmetric(List<BSGSElement> symmetricGroup)
    {
        List<BSGSCandidateElement> bsgs = AsBSGSCandidatesList(symmetricGroup);
        foreach (BSGSCandidateElement candidate in bsgs)
        {
            IList<Permutation> stabilizers = candidate.StabilizerGeneratorsReference;
            for (int i = 0; i < stabilizers.Count; ++i)
            {
                Permutation permutation = stabilizers[i];
                if (permutation.Parity == 1)
                {
                    stabilizers[i] = Permutations.CreatePermutation(true, permutation.OneLine());
                }
            }
        }

        return AsBSGSList(bsgs);
    }

    public static List<BSGSCandidateElement> AsBSGSCandidatesList(IEnumerable<BSGSElement> bsgs)
    {
        List<BSGSCandidateElement> result = [];
        foreach (BSGSElement element in bsgs)
        {
            result.Add(element.AsBSGSCandidateElement());
        }

        return result;
    }

    public static List<BSGSElement> AsBSGSList(IEnumerable<BSGSElement> candidates)
    {
        List<BSGSElement> result = [];
        foreach (BSGSElement candidate in candidates)
        {
            result.Add(candidate.AsBSGSElement());
        }

        return result;
    }

    public static int[] GetBaseAsArray<T>(IReadOnlyList<T> bsgs) where T : BSGSElement
    {
        return GetBaseAsArray(bsgs, 0);
    }

    public static int[] GetBaseAsArray<T>(IReadOnlyList<T> bsgs, int from) where T : BSGSElement
    {
        int[] baseArray = new int[bsgs.Count - from];
        for (int i = from; i < bsgs.Count; ++i)
        {
            baseArray[i - from] = bsgs[i].BasePoint;
        }

        return baseArray;
    }

    public static List<BSGSCandidateElement> Clone(IReadOnlyList<BSGSCandidateElement> bsgsCandidate)
    {
        List<BSGSCandidateElement> copy = new(bsgsCandidate.Count);
        foreach (BSGSCandidateElement element in bsgsCandidate)
        {
            copy.Add(element.Clone());
        }

        return copy;
    }

    public static StripContainer Strip(IReadOnlyList<BSGSCandidateElement> bsgs, Permutation permutation)
    {
        List<BSGSElement> converted = new(bsgs.Count);
        foreach (BSGSCandidateElement element in bsgs)
        {
            converted.Add(element);
        }

        return Strip(converted, permutation);
    }

    private static void SetAll(BitArray bitArray, IReadOnlyList<int> positions, bool value)
    {
        for (int i = 0; i < positions.Count; ++i)
        {
            bitArray.Set(positions[i], value);
        }
    }

    private static int NextSetBit(BitArray bitArray, int startIndex)
    {
        for (int i = startIndex; i < bitArray.Length; ++i)
        {
            if (bitArray.Get(i))
            {
                return i;
            }
        }

        return -1;
    }
}
