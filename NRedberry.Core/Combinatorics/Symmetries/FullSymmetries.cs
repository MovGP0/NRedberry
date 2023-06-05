using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics.Symmetries;

public class FullSymmetries : DummySymmetries
{
    public FullSymmetries(int dimension)
        : base(dimension, new List<Symmetry> {
            new Symmetry(dimension),
            new Symmetry(Combinatorics.CreateTransposition(dimension), false),
            new Symmetry(Combinatorics.CreateCycle(dimension), false)
        })
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