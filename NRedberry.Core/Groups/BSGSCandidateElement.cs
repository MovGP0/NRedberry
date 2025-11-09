using System.Collections.Immutable;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Groups;

public sealed class BSGSCandidateElement : BSGSElement
{
    public BSGSCandidateElement(
        int basePoint,
        List<Permutation> stabilizerGenerators)
        : this(basePoint, stabilizerGenerators, CalculateSVCapacity(stabilizerGenerators))
    {
        InternalDegree = SchreierVector.Length;
    }

    public BSGSCandidateElement(
        int basePoint,
        List<Permutation> stabilizerGenerators,
        int schreierVectorCapacity)
        : base(basePoint, stabilizerGenerators, new SchreierVector(schreierVectorCapacity), [])
    {
        OrbitList.Add(basePoint);
        RecalculateOrbitAndSchreierVector();
    }

    private BSGSCandidateElement(
        int basePoint,
        List<Permutation> stabilizerGenerators,
        SchreierVector schreierVector,
        List<int> orbitList)
        : base(basePoint, stabilizerGenerators, schreierVector, orbitList)
    {
    }

    private static int CalculateSVCapacity(List<Permutation> stabilizerGenerators)
    {
        int capacity = -1;
        foreach (var p in stabilizerGenerators)
        {
            capacity = Math.Max(capacity, p.Degree);
        }

        return capacity;
    }

    public void AddStabilizer(Permutation stabilizer)
    {
        InternalDegree = Math.Max(InternalDegree, stabilizer.Degree);
        StabilizerGenerators.Add(stabilizer);
        RecalculateOrbitAndSchreierVector();
    }

    private void RecalculateOrbitAndSchreierVector()
    {
        OrbitList.RemoveAfter(1);
        SchreierVector.Reset();
        SchreierVector[BasePoint] = -1;

        for (int orbitIndex = 0; orbitIndex < OrbitList.Count; ++orbitIndex)
        {
            var stabilizerGenerators = StabilizerGenerators.ToImmutableArray();
            foreach (var stabilizer in stabilizerGenerators)
            {
                int imageOfPoint = stabilizer.NewIndexOf(OrbitList[orbitIndex]);
                if (SchreierVector[imageOfPoint] == -2)
                {
                    OrbitList.Add(imageOfPoint);
                    SchreierVector[imageOfPoint] = stabilizerGenerators.IndexOf(stabilizer);
                }
            }
        }
    }

    public List<Permutation> GetStabilizersOfThisBasePoint()
    {
        var basePointStabilizers = new List<Permutation>();
        foreach (var stabilizer in StabilizerGenerators)
        {
            if (stabilizer.NewIndexOf(BasePoint) == BasePoint)
                basePointStabilizers.Add(stabilizer);
        }

        return basePointStabilizers;
    }

    public override BSGSElement AsBSGSElement()
    {
        return new BSGSElement(BasePoint, [..StabilizerGenerators], SchreierVector, OrbitList.Clone());
    }

    public override BSGSCandidateElement AsBSGSCandidateElement()
    {
        return this;
    }

    public BSGSCandidateElement Clone()
    {
        return new BSGSCandidateElement(BasePoint, [..StabilizerGenerators], SchreierVector.Clone(), OrbitList.Clone());
    }
}
