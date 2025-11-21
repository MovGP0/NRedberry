namespace NRedberry.Core.Combinatorics.Symmetries;

/// <summary>
/// src\main\java\cc\redberry\core\combinatorics\symmetries\FullSymmetries.java
/// </summary>
public class FullSymmetries(int dimension)
    : DummySymmetries(
        dimension,
        [
            new(dimension),
            new(Combinatorics.CreateTransposition(dimension), false),
            new(Combinatorics.CreateCycle(dimension), false)
        ])
{
    public override IEnumerator<Symmetry> GetEnumerator() => new PermutationsGenerator<Symmetry>(Dimension);

    public new bool IsEmpty => false;
}
