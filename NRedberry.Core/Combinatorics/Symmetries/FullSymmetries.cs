using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics.Symmetries;

public class FullSymmetries : DummySymmetries
{
    public FullSymmetries(int dimension)
        : base(dimension, new List<Symmetry> {
            new(dimension),
            new(Combinatorics.CreateTransposition(dimension), false),
            new(Combinatorics.CreateCycle(dimension), false)
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