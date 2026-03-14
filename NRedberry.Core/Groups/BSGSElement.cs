using NRedberry.Core.Combinatorics;

namespace NRedberry.Groups;

public class BSGSElement(
    int basePoint,
    IReadOnlyCollection<Permutation> stabilizerGenerators,
    SchreierVector schreierVector,
    List<int> orbitList)
{
    public int BasePoint { get; } = basePoint;

    public IList<Permutation> StabilizerGenerators { get; } = new List<Permutation>(stabilizerGenerators);

    protected SchreierVector SchreierVector { get; } = schreierVector;

    protected List<int> OrbitList { get; } = orbitList;

    public IList<Permutation> StabilizerGeneratorsReference => StabilizerGenerators;
    public List<int> OrbitListReference => OrbitList;

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
        {
            throw new ArgumentException("Specified point does not belong to orbit of this base element.");
        }

        var stabilizerGenerators = StabilizerGenerators;
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

    public int InternalDegree { get; protected internal set; } = Permutations.InternalDegree(stabilizerGenerators);

    public override string ToString()
    {
        return $"[{BasePoint}, {string.Join(", ", StabilizerGenerators)}]";
    }
}
