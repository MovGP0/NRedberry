using System;
using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics.Symmetries;

/// <summary>
/// src\main\java\cc\redberry\core\combinatorics\symmetries\DummySymmetries.java
/// </summary>
public abstract class DummySymmetries : AbstractSymmetries
{
    protected DummySymmetries(int dimension, List<Symmetry> basis) : base(dimension, basis) { }

    public override bool Add(Symmetry symmetry)
    {
        if (symmetry.Dimension() != base.Dimension || symmetry.IsAntiSymmetry())
            throw new ArgumentException();
        return false;
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