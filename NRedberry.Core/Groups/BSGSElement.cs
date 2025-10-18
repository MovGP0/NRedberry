using System.Collections.Immutable;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Groups;

using System;
using System.Collections.Generic;

public class BSGSElement
{
    public readonly int BasePoint;
    protected readonly IList<Permutation> StabilizerGenerators;
    protected readonly SchreierVector SchreierVector;
    protected readonly IntArrayList OrbitList;

    public BSGSElement(
        int basePoint,
        IReadOnlyCollection<Permutation> stabilizerGenerators,
        SchreierVector schreierVector,
        IntArrayList orbitList)
    {
        BasePoint = basePoint;
        StabilizerGenerators = stabilizerGenerators.ToList();
        SchreierVector = schreierVector;
        OrbitList = orbitList;
        InternalDegree = Permutations.InternalDegree(stabilizerGenerators);
    }

    public IList<Permutation> GetStabilizerGeneratorsReference()
    {
        return StabilizerGenerators;
    }

    public Permutation GetTransversalOf(int point)
    {
        var transversal = GetInverseTransversalOf(point).Inverse();
        if (transversal.NewIndexOf(BasePoint) != point)
        {
            throw new InvalidOperationException("Invariant violated: Transversal does not map the base point correctly.");
        }
        return transversal;
    }

    public Permutation GetInverseTransversalOf(int point)
    {
        if (SchreierVector[point] == -2)
            throw new ArgumentException("Specified point does not belong to orbit of this base element.");

        var stabilizerGenerators = StabilizerGenerators.ToImmutableArray();
        var temp = Permutations.CreateIdentityPermutation(SchreierVector.Length);
        while (SchreierVector[temp.NewIndexOf(point)] != -1)
        {
            var perm = SchreierVector[temp.NewIndexOf(point)];
            temp = temp.CompositionWithInverse(stabilizerGenerators[perm]);
        }
        return temp;
    }

    public virtual BSGSElement AsBSGSElement()
    {
        return this;
    }

    public bool BelongsToOrbit(int point)
    {
        return SchreierVector[point] != -2;
    }

    public virtual BSGSCandidateElement AsBSGSCandidateElement()
    {
        return new BSGSCandidateElement(BasePoint, [..StabilizerGenerators], SchreierVector.Length);
    }

    public int OrbitSize => OrbitList.Count;

    public int GetOrbitPoint(int index)
    {
        return OrbitList[index];
    }

    public int InternalDegree { get; set; }

    public override string ToString()
    {
        return $"[{BasePoint}, {string.Join(", ", StabilizerGenerators)}]";
    }
}
