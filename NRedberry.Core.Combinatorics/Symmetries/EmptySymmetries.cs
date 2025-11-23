namespace NRedberry.Core.Combinatorics.Symmetries;

/// <summary>
/// src\main\java\cc\redberry\core\combinatorics\symmetries\EmptySymmetries.java
/// </summary>
public sealed class EmptySymmetries : DummySymmetries
{
    public EmptySymmetries(int dimension)
        : base(dimension, [new(dimension)])
    {
        if (dimension != 0 && dimension != 1)
        {
            throw new ArgumentException("Dimension must be either 0 or 1.");
        }
    }

    public override bool IsEmpty => true;

    public override IEnumerator<Symmetry> GetEnumerator()
    {
        return Basis.GetEnumerator();
    }
}
