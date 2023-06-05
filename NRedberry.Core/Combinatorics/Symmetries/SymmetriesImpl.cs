﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NRedberry.Core.Combinatorics.Symmetries;

public class SymmetriesImpl : AbstractSymmetries
{
    public SymmetriesImpl(int dimension) : base(dimension, new List<Symmetry>())
    {
        this.Basis.Add(new Symmetry(dimension));
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
        if (symmetry.Dimension() != this.Dimension())
            throw new ArgumentException();
        var it = new PermutationsSpanIterator<Symmetry>(Basis);
        //TODO BOTTLENECK review
        while (it.MoveNext())
        {
            Symmetry? s = it.Current;
            if (s.Equals(symmetry))
                return false;
        }
        this.Basis.Add(symmetry);
        //BOTTLENECK
        //checking consistense
        it = new PermutationsSpanIterator<Symmetry>(Basis);
        while (it.MoveNext()) ;
        return true;
    }

    public override bool AddUnsafe(Symmetry symmetry)
    {
        this.Basis.Add(symmetry);
        return true;
    }

    public override IEnumerator<Symmetry> GetEnumerator()
    {
        return new PermutationsSpanIterator<Symmetry>(this.Basis);
    }

    public bool IsEmpty()
    {
        return false;
    }

    public List<Symmetry> GetBasisSymmetries()
    {
        return new List<Symmetry>(Basis); // Return a new list to preserve encapsulation
    }

    public override Symmetries Clone()
    {
        return new SymmetriesImpl(Dimension(), new List<Symmetry>(Basis));
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        foreach (Symmetry s in Basis)
            sb.AppendLine(s.ToString());
        return sb.ToString();
    }
}