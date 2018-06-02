using System;
using System.Collections;
using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics.Symmetries
{
    public abstract class AbstractSymmetries : ISymmetries, ICloneable
    {
        protected int Dimension { get; set; }
        protected IList<Symmetry> Basis { get; }

        protected AbstractSymmetries(int dimension, IList<Symmetry> basis)
        {
            Dimension = dimension;
            Basis = basis;
        }

        public override int GetHashCode()
        {
            return 235 + Basis.GetHashCode();
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public abstract ISymmetries Clone();

        public abstract IEnumerator<Symmetry> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
