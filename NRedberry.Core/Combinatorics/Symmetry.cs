namespace NRedberry.Core.Combinatorics
{
    public class Symmetry : Permutation
    {
        public Symmetry(int dimension) : base(dimension)
        {
        }

        public Symmetry(int[] permutation) : base(permutation)
        {
        }

        protected Symmetry(int[] permutation, bool notClone) : base(permutation, notClone)
        {
        }
    }
}
