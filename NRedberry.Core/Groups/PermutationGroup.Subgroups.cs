using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Groups;

public sealed partial class PermutationGroup
{
    public PermutationGroup Union(params Permutation[] generators)
    {
        return Union((IReadOnlyList<Permutation>)generators);
    }

    public PermutationGroup Union(IReadOnlyList<Permutation> generators)
    {
        if (IsTrivial())
        {
            return CreatePermutationGroup(generators);
        }

        if (_bsgs != null && MembershipTest(generators))
        {
            return this;
        }

        var allGenerators = new List<Permutation>(Generators);
        allGenerators.AddRange(generators);
        PermutationGroup result = CreatePermutationGroup(allGenerators);
        result._base = _base;
        return result;
    }

    public PermutationGroup Union(PermutationGroup group)
    {
        if (ReferenceEquals(this, group))
        {
            return this;
        }

        if (IsTrivial())
        {
            return group;
        }

        if (group.IsTrivial())
        {
            return this;
        }

        if (_bsgs == null && group._bsgs != null)
        {
            return group.Union(Generators);
        }

        if (group._bsgs == null)
        {
            return Union(group.Generators);
        }

        if (ContainsSubgroup(group))
        {
            return this;
        }

        if (group.ContainsSubgroup(this))
        {
            return group;
        }

        int[] thisBase = GetBase();
        int[] otherBase = group.GetBase();
        Array.Sort(thisBase);
        Array.Sort(otherBase);
        int[] @base = MathUtils.IntSetUnion(thisBase, otherBase);

        var generators = new List<Permutation>(Generators.Count + group.Generators.Count);
        generators.AddRange(Generators);
        generators.AddRange(group.Generators);
        PermutationGroup r = CreatePermutationGroup(generators);
        r._base = @base;
        return r;
    }

    public PermutationGroup Intersection(PermutationGroup other)
    {
        if (IsTrivial())
        {
            return this;
        }

        if (other.IsTrivial())
        {
            return other;
        }

        if (IsSymmetric())
        {
            if (other.Degree <= Degree)
            {
                return other;
            }

            int[] points = new int[other.Degree - Degree];
            for (int i = 0; i < points.Length; ++i)
            {
                points[i] = Degree + i;
            }

            return other.PointwiseStabilizer(points);
        }

        if (other.IsSymmetric())
        {
            return other.Intersection(this);
        }

        var intersection = new List<BSGSCandidateElement>();
        AlgorithmsBacktrack.Intersection(GetBSGS(), other.GetBSGS(), intersection);
        return CreatePermutationGroupFromBSGS(AlgorithmsBase.AsBSGSList(intersection));
    }

    public bool ContainsSubgroup(PermutationGroup subgroup)
    {
        if (Degree < subgroup.Degree)
        {
            return false;
        }

        if (IsTrivial())
        {
            return subgroup.IsTrivial();
        }

        if (IsSymmetric())
        {
            return true;
        }

        if (IsAlternating())
        {
            foreach (Permutation permutation in subgroup.Generators)
            {
                if (permutation.Parity == 1)
                {
                    return false;
                }
            }

            return true;
        }

        if (subgroup.Order.CompareTo(Order) > 0)
        {
            return false;
        }

        return MembershipTest(subgroup.Generators);
    }

    public PermutationGroup NormalClosureOf(PermutationGroup subgroup)
    {
        if (subgroup.IsTrivial())
        {
            return subgroup;
        }

        if (IsAlternating() && _internalDegree > 4)
        {
            return this;
        }

        if (IsSymmetric() && _internalDegree != 4)
        {
            foreach (Permutation permutation in subgroup._generators)
            {
                if (permutation.Parity == 1)
                {
                    return this;
                }
            }

            return AlternatingGroup(_internalDegree);
        }

        var closure = subgroup.GetBSGSCandidate();
        List<Permutation> randomSource = RandomSource();
        bool completed = false;
        bool added;
        bool globalAdded = false;
        while (!completed)
        {
            var closureSource = new List<Permutation>(closure[0].StabilizerGenerators);

            RandomPermutation.Randomness(closureSource, 10, 10, Random.Shared);

            added = false;
            for (int i = 0; i < 10; ++i)
            {
                Permutation c = RandomPermutation.Random(randomSource)
                    .Conjugate(RandomPermutation.Random(closureSource));

                if (!AlgorithmsBase.MembershipTest(closure, c))
                {
                    closure[0].AddStabilizer(c);
                    added = true;
                    globalAdded = true;
                }
            }

            if (added)
            {
                AlgorithmsBase.RandomSchreierSimsAlgorithm(closure, NormalClosureConfidenceLevel, Random.Shared);
            }

            completed = true;
            foreach (Permutation generator in _generators)
            {
                foreach (Permutation cGenerator in closure[0].StabilizerGenerators)
                {
                    if (!AlgorithmsBase.MembershipTest(AlgorithmsBase.AsBSGSList(closure), generator.Conjugate(cGenerator)))
                    {
                        completed = false;
                        goto EndTest;
                    }
                }
            }

            EndTest:
            ;
        }

        if (globalAdded)
        {
            AlgorithmsBase.SchreierSimsAlgorithm(closure);
        }

        return CreatePermutationGroupFromBSGS(AlgorithmsBase.AsBSGSList(closure));
    }

    public PermutationGroup Commutator(PermutationGroup group)
    {
        var commutator = new List<Permutation>();
        foreach (Permutation a in _generators)
        {
            foreach (Permutation b in group._generators)
            {
                Permutation c = a.Commutator(b);
                if (!c.IsIdentity)
                {
                    commutator.Add(c);
                }
            }
        }

        if (commutator.Count == 0)
        {
            return TrivialGroupInstance;
        }

        return Union(group).NormalClosureOf(CreatePermutationGroup(commutator));
    }

    public PermutationGroup DerivedSubgroup()
    {
        if (_derivedSubgroup != null)
        {
            return _derivedSubgroup;
        }

        if (IsSymmetric())
        {
            return _derivedSubgroup = AlternatingGroup(_internalDegree);
        }

        if (IsAlternating() && _internalDegree > 4)
        {
            return _derivedSubgroup = this;
        }

        return _derivedSubgroup = Commutator(this);
    }

    private const double NormalClosureConfidenceLevel = 1 - 1E-6;
}
