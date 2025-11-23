using System.Numerics;
using System.Text;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils;

namespace NRedberry.Groups;

/// <summary>
/// Transcription of cc.redberry.core.groups.permutations.PermutationGroup.
/// </summary>
public sealed partial class PermutationGroup
{
    public static readonly PermutationGroup TrivialGroupInstance = new();

    private readonly IReadOnlyList<Permutation> _generators;
    private readonly int _internalDegree;
    private readonly int[] _positionsInOrbits;
    private readonly int[][] _orbits;

    private List<BSGSElement>? _bsgs;
    private int[]? _base;
    private BigInteger? _order;
    private InducedOrdering? _ordering;

    private bool? _isTrivial;
    private bool? _isAbelian;
    private bool? _isSymmetric;
    private bool? _isAlternating;
    private PermutationGroup? _derivedSubgroup;
    private List<Permutation>? _randomSource;

    private PermutationGroup(IReadOnlyList<Permutation> generators, int internalDegree, int _)
    {
        if (generators.Count == 0)
        {
            throw new ArgumentException("Empty generators.");
        }

        _generators = generators.ToList().AsReadOnly();
        _internalDegree = internalDegree;
        _positionsInOrbits = new int[internalDegree];
        _orbits = Permutations.Orbits(generators.ToList(), _positionsInOrbits);
    }

    private PermutationGroup(List<BSGSElement> bsgs, int internalDegree)
    {
        if (bsgs.Count == 0)
        {
            throw new ArgumentException("Empty BSGS specified.");
        }

        _bsgs = bsgs.AsReadOnly().ToList();
        _base = AlgorithmsBase.GetBaseAsArray(bsgs);
        _internalDegree = internalDegree;
        _order = AlgorithmsBase.CalculateOrder(bsgs);
        _positionsInOrbits = new int[internalDegree];
        _generators = bsgs[0].StabilizerGenerators.AsReadOnly();
        _orbits = Permutations.Orbits(bsgs[0].StabilizerGenerators.ToList(), _positionsInOrbits);
        _ordering = new InducedOrdering(_base);
    }

    private PermutationGroup()
    {
        _bsgs = new List<BSGSElement>();
        _base = Array.Empty<int>();
        _internalDegree = 1;
        _order = BigInteger.One;
        _positionsInOrbits = Array.Empty<int>();
        _generators = new List<Permutation> { Permutations.GetIdentityPermutation() }.AsReadOnly();
        _orbits = Array.Empty<int[]>();
        _ordering = new InducedOrdering(_base);
    }

    public static PermutationGroup TrivialGroup()
    {
        return TrivialGroupInstance;
    }

    public static PermutationGroup CreatePermutationGroup(params Permutation[] generators)
    {
        return CreatePermutationGroup((IReadOnlyList<Permutation>)generators);
    }

    public static PermutationGroup CreatePermutationGroup(IReadOnlyList<Permutation> generators)
    {
        int degree = Permutations.InternalDegree(generators.ToList());
        if (degree == 0)
        {
            return TrivialGroupInstance;
        }

        return new PermutationGroup(generators, degree, 0);
    }

    public static PermutationGroup CreatePermutationGroupFromBSGS(params List<BSGSElement> bsgs)
    {
        int degree = bsgs.Count == 0 ? 0 : bsgs[0].InternalDegree;
        if (degree == 0)
        {
            return TrivialGroupInstance;
        }

        return new PermutationGroup(bsgs, degree);
    }

    public static PermutationGroup SymmetricGroup(int degree)
    {
        if (degree < 0)
        {
            throw new ArgumentException("degree < 0");
        }

        if (degree == 0)
        {
            return TrivialGroupInstance;
        }

        return CreatePermutationGroup(
            Permutations.CreatePermutation(Permutations.CreateTransposition(degree, 0, Math.Min(1, degree - 1))),
            Permutations.CreatePermutation(Permutations.CreateCycle(degree)));
    }

    public static PermutationGroup AntisymmetricGroup(int degree)
    {
        if (degree < 0)
        {
            throw new ArgumentException("degree < 0");
        }

        if (degree == 0)
        {
            return TrivialGroupInstance;
        }

        return CreatePermutationGroup(
            Permutations.CreatePermutation(true, Permutations.CreateTransposition(degree, 0, Math.Min(1, degree - 1))),
            Permutations.CreatePermutation(Permutations.CreateCycle(degree)));
    }

    public static PermutationGroup AlternatingGroup(int degree)
    {
        if (degree < 0)
        {
            throw new ArgumentException("degree < 0");
        }

        if (degree == 0)
        {
            return TrivialGroupInstance;
        }

        var threeCycle = new int[degree];
        for (int i = 0; i < degree; ++i)
        {
            threeCycle[i] = i;
        }

        if (degree > 0)
        {
            threeCycle[0] = Math.Min(1, degree - 1);
        }

        if (degree > 1)
        {
            threeCycle[1] = Math.Min(2, degree - 1);
        }

        if (degree > 2)
        {
            threeCycle[2] = 0;
        }

        return CreatePermutationGroup(
            Permutations.CreatePermutation(threeCycle),
            Permutations.CreatePermutation(Permutations.CreateCycle(degree)));
    }

    public BigInteger Order
    {
        get
        {
            EnsureBsgsIsInitialized();
            return _order ?? BigInteger.Zero;
        }
    }

    public int Degree => _internalDegree;

    public IReadOnlyList<Permutation> Generators => _generators;

    public List<BSGSElement> GetBSGS()
    {
        EnsureBsgsIsInitialized();
        return _bsgs!;
    }

    public int[] GetBase()
    {
        EnsureBsgsIsInitialized();
        return _base!.ToArray();
    }

    public InducedOrdering Ordering()
    {
        EnsureBsgsIsInitialized();
        return _ordering!;
    }

    private int[] BaseArray()
    {
        EnsureBsgsIsInitialized();
        return _base!;
    }

    public List<BSGSCandidateElement> GetBSGSCandidate()
    {
        return AlgorithmsBase.AsBSGSCandidatesList(GetBSGS());
    }

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != GetType())
        {
            return false;
        }

        var other = (PermutationGroup)obj;
        if (_internalDegree != other._internalDegree)
        {
            return false;
        }

        if (_orbits.Length != other._orbits.Length)
        {
            return false;
        }

        if (!Order.Equals(other.Order))
        {
            return false;
        }

        if (Generators.Count < other.Generators.Count)
        {
            return other.MembershipTest(Generators);
        }

        return MembershipTest(other.Generators);
    }

    public override int GetHashCode()
    {
        int hash = Order.GetHashCode();
        foreach (int[] orbit in _orbits)
        {
            hash += HashFunctions.JenkinWang32shift(orbit.Length);
        }

        return hash;
    }

    public override string ToString()
    {
        var gens = Generators;
        var builder = new StringBuilder();
        builder.Append("Group( ");
        for (int i = 0; i < gens.Count; ++i)
        {
            builder.Append(gens[i]);
            if (i == gens.Count - 1)
            {
                builder.Append(" )");
            }
            else
            {
                builder.Append(", ");
            }
        }

        return builder.ToString();
    }

    public string ToStringJava()
    {
        var gens = Generators;
        var builder = new StringBuilder();
        builder.Append("PermutationGroup.createPermutationGroup( ");
        for (int i = 0; i < gens.Count; ++i)
        {
            string p = gens[i].ToString()
                .Replace("[", "{", StringComparison.Ordinal)
                .Replace("]", "}", StringComparison.Ordinal)
                .Replace("+", string.Empty, StringComparison.Ordinal)
                .Replace("-", string.Empty, StringComparison.Ordinal);

            if (gens[i].IsAntisymmetry)
            {
                builder.Append("Permutations.createPermutation(true, new int[][]").Append(p).Append(")");
            }
            else
            {
                builder.Append("Permutations.createPermutation(new int[][]").Append(p).Append(")");
            }

            if (i == gens.Count - 1)
            {
                builder.Append(" )");
            }
            else
            {
                builder.Append(", ");
            }
        }

        return builder.ToString();
    }

    private void EnsureBsgsIsInitialized()
    {
        if (_bsgs is not null)
        {
            return;
        }

        List<BSGSElement> bsgs = _base is not null
            ? AlgorithmsBase.CreateBSGSList(_base, _generators)
            : AlgorithmsBase.CreateBSGSList(_generators);

        _bsgs = bsgs;
        _base = AlgorithmsBase.GetBaseAsArray(bsgs);
        _order = AlgorithmsBase.CalculateOrder(bsgs);
        _ordering = new InducedOrdering(_base);
    }
}
