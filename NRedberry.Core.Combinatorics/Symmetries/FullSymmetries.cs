namespace NRedberry.Core.Combinatorics.Symmetries;

/// <summary>
/// src\main\java\cc\redberry\core\combinatorics\symmetries\FullSymmetries.java
/// </summary>
public class FullSymmetries : DummySymmetries
{
    public FullSymmetries(int dimension)
        : base(dimension, [
            new(dimension),
            new(Combinatorics.CreateTransposition(dimension), false),
            new(Combinatorics.CreateCycle(dimension), false)
        ])
    {
    }

    public override IEnumerator<Symmetry> GetEnumerator()
    {
        return new PermutationsGenerator<Symmetry>(Dimension);
    }

    public new bool IsEmpty()
    {
        return false;
    }
}