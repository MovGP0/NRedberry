using System;
using System.Collections.Generic;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Combinatorics.Symmetries;

public class EmptySymmetries : DummySymmetries
{
    public EmptySymmetries(int dimension) : base(dimension, new List<Symmetry> { new(dimension) })
    {
        if (dimension != 0 && dimension != 1)
            throw new ArgumentException("Dimension must be either 0 or 1.");
    }

    public new bool IsEmpty()
    {
        return true;
    }

    public override IEnumerator<Symmetry> GetEnumerator()
    {
        return new SingleIterator<Symmetry>(Basis[0]);
    }
}