using System.Text;

namespace NRedberry.Core.Combinatorics.Symmetries;

/// <summary>
/// src\main\java\cc\redberry\core\combinatorics\symmetries\SymmetriesImpl.java
/// </summary>
public class SymmetriesImpl : AbstractSymmetries
{
    public SymmetriesImpl(int dimension)
        : base(dimension, [])
    {
        Basis.Add(new Symmetry(dimension));
    }

    public SymmetriesImpl(int dimension, List<Symmetry> basis)
        : base(dimension, basis)
    {
    }

    public override bool Add(Symmetry symmetry)
    {
        ArgumentNullException.ThrowIfNull(symmetry);
        if (symmetry.Length != Dimension)
        {
            throw new ArgumentException("Symmetry dimension mismatch.");
        }

        if (Basis.Any(existing => existing.Equals(symmetry)))
        {
            return false;
        }

        Basis.Add(symmetry);
        return true;
    }

    public override bool AddUnsafe(Symmetry symmetry)
    {
        Basis.Add(symmetry);
        return true;
    }

    public override IEnumerator<Symmetry> GetEnumerator()
    {
        return new PermutationsSpanIterator<Symmetry>(Basis);
    }

    public bool IsEmpty => false;

    public List<Symmetry> BasisSymmetries => [..Basis];

    public override Symmetries Clone() => new SymmetriesImpl(Dimension, [.. Basis]);

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var s in Basis)
        {
            sb.AppendLine(s.ToString());
        }

        return sb.ToString();
    }
}
