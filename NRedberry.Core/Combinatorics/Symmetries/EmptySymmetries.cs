using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics.Symmetries
{
    public sealed class EmptySymmetries : DummySymmetries
    {
        public EmptySymmetries(int dimension, IList<Symmetry> basis) : base(dimension, basis)
        {
        }
    }
}