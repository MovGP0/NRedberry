using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics.Symmetries
{
    public sealed class FullSymmetries : DummySymmetries
    {
        public FullSymmetries(int dimension, IList<Symmetry> basis) : base(dimension, basis)
        {
        }
    }
}