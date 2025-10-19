﻿using System.Text;

namespace NRedberry.Core.Combinatorics.Symmetries;

/// <summary>
/// src\main\java\cc\redberry\core\combinatorics\symmetries\SymmetriesImpl.java
/// </summary>
public class SymmetriesImpl : AbstractSymmetries
{
    public SymmetriesImpl(int dimension) : base(dimension, [])
    {
        Basis.Add(new Symmetry(dimension));
    }

    public SymmetriesImpl(int dimension, List<Symmetry> basis) : base(dimension, basis)
    {
    }

    public int Dimension()
    {
        return base.Dimension;
    }

    public override bool Add(Symmetry symmetry)
    {
        if (symmetry.Dimension() != Dimension())
            throw new ArgumentException();
        var it = new PermutationsSpanIterator<Symmetry>(Basis);
        //TODO BOTTLENECK review
        while (it.MoveNext())
        {
            var s = it.Current;
            if (s.Equals(symmetry))
                return false;
        }
        Basis.Add(symmetry);
        //BOTTLENECK
        //checking consistense
        it = new PermutationsSpanIterator<Symmetry>(Basis);
        while (it.MoveNext()) ;
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

    public bool IsEmpty()
    {
        return false;
    }

    public List<Symmetry> GetBasisSymmetries()
    {
        return [..Basis]; // Return a new list to preserve encapsulation
    }

    public override Symmetries Clone()
    {
        return new SymmetriesImpl(Dimension(), [..Basis]);
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var s in Basis)
            sb.AppendLine(s.ToString());
        return sb.ToString();
    }
}