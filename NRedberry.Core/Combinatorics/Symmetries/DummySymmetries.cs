using System.Collections.Generic;

namespace NRedberry.Core.Combinatorics.Symmetries
{
    public class DummySymmetries : AbstractSymmetries
    {
        public DummySymmetries(int dimension, IList<Symmetry> basis) : base(dimension, basis)
        {
        }

        public override ISymmetries Clone()
        {
            throw new System.NotImplementedException();
        }

        public override IEnumerator<Symmetry> GetEnumerator()
        {
            throw new System.NotImplementedException();
        }
    }
}