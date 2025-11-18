using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Groups;

public sealed partial class PermutationGroup
{
    public PermutationGroup CentralizerOf(Permutation permutation)
    {
        return CentralizerOf(CreatePermutationGroup(permutation));
    }

    public PermutationGroup CentralizerOf(PermutationGroup subgroup)
    {
        if (subgroup.IsAbelian() && subgroup.IsTransitive(0, _internalDegree))
        {
            return subgroup;
        }

        int[] @base = GetBase();
        if (subgroup._orbits.Length != 1)
        {
            Array.Sort(@base, (a, b) => -subgroup.OrbitSize(a).CompareTo(subgroup.OrbitSize(b)));
        }

        List<BSGSCandidateElement> groupBsgs = GetBSGSCandidate();
        AlgorithmsBase.Rebase(groupBsgs, @base);

        List<BSGSCandidateElement> subgroupBsgs = subgroup.GetBSGSCandidate();
        AlgorithmsBacktrack.RebaseWithRedundancy(subgroupBsgs, @base, _internalDegree);

        var mappings = new Permutation[@base.Length - 1];
        for (int i = 1; i < @base.Length; ++i)
        {
            if (@base[i] < subgroup._internalDegree && subgroup.IndexOfOrbit(@base[i]) == subgroup.IndexOfOrbit(@base[i - 1]))
            {
                mappings[i - 1] = subgroup.Mapping(@base[i - 1], @base[i])!;
            }
        }

        var centralizerSearch = new CentralizerSearchTest(groupBsgs, subgroup, @base, mappings);
        List<BSGSCandidateElement> centralizer = subgroup.Generators.Count == 1
            ? AlgorithmsBase.Clone(subgroupBsgs)
            : new List<BSGSCandidateElement>();

        AlgorithmsBacktrack.SubgroupSearch(groupBsgs.OfType<BSGSElement>().ToList(), centralizer, centralizerSearch, centralizerSearch);
        return CreatePermutationGroupFromBSGS(AlgorithmsBase.AsBSGSList(centralizer));
    }

    public PermutationGroup Center()
    {
        if (_center == null)
        {
            if (IsSymmetric() && _internalDegree >= 3)
            {
                return _center = CreatePermutationGroup(Generators[0].Identity);
            }

            if (IsAlternating() && _internalDegree >= 4)
            {
                return _center = CreatePermutationGroup(Generators[0].Identity);
            }

            _center = CentralizerOf(this);
        }

        return _center;
    }

    public PermutationGroup Conjugate(Permutation permutation)
    {
        if (IsTrivial())
        {
            return this;
        }

        if (_bsgs == null)
        {
            var newGens = new List<Permutation>(_generators.Count);
            foreach (Permutation p in _generators)
            {
                newGens.Add(permutation.Conjugate(p));
            }

            return CreatePermutationGroup(newGens);
        }
        else
        {
            List<BSGSElement> bsgs = GetBSGS();
            var newBsgs = new List<BSGSElement>(bsgs.Count);
            foreach (BSGSElement element in bsgs)
            {
                var newStabs = new List<Permutation>(element.StabilizerGenerators.Count);
                foreach (Permutation p in element.StabilizerGenerators)
                {
                    newStabs.Add(permutation.Conjugate(p));
                }

                newBsgs.Add(new BSGSCandidateElement(permutation.NewIndexOf(element.BasePoint), newStabs, _internalDegree)
                    .AsBSGSElement());
            }

            return CreatePermutationGroupFromBSGS(newBsgs);
        }
    }

    private PermutationGroup? _center;

    private sealed class CentralizerSearchTest : IBacktrackSearchTestFunction, IIndicator<Permutation>
    {
        private readonly List<BSGSCandidateElement> _groupBsgs;
        private readonly PermutationGroup _subgroup;
        private readonly Permutation?[] _mappings;
        private readonly int[] _groupBase;

        public CentralizerSearchTest(
            List<BSGSCandidateElement> groupBsgs,
            PermutationGroup subgroup,
            int[] groupBase,
            Permutation?[] mappings)
        {
            _groupBsgs = groupBsgs;
            _subgroup = subgroup;
            _groupBase = groupBase;
            _mappings = mappings;
        }

        public bool Test(Permutation permutation, int level)
        {
            if (level == 0)
            {
                return true;
            }

            if (_subgroup._internalDegree < _groupBase[level - 1]
                && _groupBase[level - 1] != _groupBase[level])
            {
                return true;
            }

            if (_subgroup.IndexOfOrbit(_groupBase[level - 1]) != _subgroup.IndexOfOrbit(_groupBase[level]))
            {
                return true;
            }

            Permutation mapping = _mappings[level - 1]!;
            int expected = mapping.NewIndexOf(permutation.NewIndexOf(_groupBase[level - 1]));
            return permutation.NewIndexOf(_groupBase[level]) == expected;
        }

        public bool Is(Permutation permutation)
        {
            if (permutation.IsIdentity)
            {
                return false;
            }

            foreach (Permutation p in _subgroup._generators)
            {
                if (!p.Commutator(permutation).IsIdentity)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
