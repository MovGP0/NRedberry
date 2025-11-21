namespace NRedberry.Core.Combinatorics.Symmetries;

/// <summary>
/// src\main\java\cc\redberry\core\combinatorics\symmetries\DummySymmetries.java
/// </summary>
public abstract class DummySymmetries(int dimension, List<Symmetry> basis) : AbstractSymmetries(dimension, basis)
{
    public override bool Add(Symmetry symmetry)
    {
        throw new NotImplementedException();
    }

    public override bool AddUnsafe(Symmetry symmetry)
    {
        return Add(symmetry);
    }

    public override Symmetries Clone()
    {
        return this;
    }
}
