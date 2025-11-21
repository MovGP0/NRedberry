namespace NRedberry.Core.Combinatorics.Symmetries;

/// <summary>
/// src\main\java\cc\redberry\core\combinatorics\symmetries\DummySymmetries.java
/// </summary>
public abstract class DummySymmetries(int dimension, List<Symmetry> basis) : AbstractSymmetries(dimension, basis)
{
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

    public override bool AddUnsafe(Symmetry symmetry) => Add(symmetry);

    public override Symmetries Clone() => this;
}
