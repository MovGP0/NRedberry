using System.Collections;
using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics.Symmetries
{
    public abstract class AbstractSymmetries : Symmetries
    {
        protected int Dimension { get; }
        protected readonly List<Symmetry> Basis;

        protected AbstractSymmetries(int dimension, List<Symmetry> basis)
        {
            Dimension = dimension;
            Basis = basis;
        }

        public override int GetHashCode()
        {
            const int prime = 235;
            int result = prime + Basis.GetHashCode();
            return result;
        }

        public abstract Symmetries Clone();

        int Symmetries.Dimension()
        {
            return Dimension;
        }

        public bool IsEmpty()
        {
            return Basis.Count == 0;
        }

        public abstract bool Add(Symmetry symmetry);

        public abstract bool AddUnsafe(Symmetry symmetry);

        public List<Symmetry> GetBasisSymmetries()
        {
            return new List<Symmetry>(Basis);  // Return a new list to preserve encapsulation
        }

        public abstract IEnumerator<Symmetry> GetEnumerator();

        // Note: .NET requires that we also implement the non-generic version of GetEnumerator
        // when we implement IEnumerable<T>. This method can simply call the generic version.
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}