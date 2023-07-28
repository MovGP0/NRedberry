using System.Collections;
using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics.Symmetries
{
    /// <summary>
    /// src\main\java\cc\redberry\core\combinatorics\symmetries\AbstractSymmetries.java
    /// </summary>
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
        int Symmetries.Dimension => Dimension;
        public bool IsEmpty => Basis.Count == 0;
        public abstract bool Add(Symmetry symmetry);
        public abstract bool AddUnsafe(Symmetry symmetry);
        public List<Symmetry> BasisSymmetries =>  new(Basis);
        public abstract IEnumerator<Symmetry> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}