using System.Collections;

namespace NRedberry.Core.Combinatorics.Symmetries
{
    /// <summary>
    /// src\main\java\cc\redberry\core\combinatorics\symmetries\AbstractSymmetries.java
    /// </summary>
    public abstract class AbstractSymmetries(int dimension, List<Symmetry> basis) : Symmetries
    {
        protected int Dimension { get; } = dimension;

        protected readonly List<Symmetry> Basis = basis;

        public override int GetHashCode()
        {
            const int prime = 235;
            return prime + Basis.GetHashCode();
        }

        public abstract Symmetries Clone();

        int Symmetries.Dimension => Dimension;
        public bool IsEmpty => Basis.Count == 0;

        public abstract bool Add(Symmetry symmetry);
        public abstract bool AddUnsafe(Symmetry symmetry);

        public List<Symmetry> BasisSymmetries => [..Basis];

        public abstract IEnumerator<Symmetry> GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
