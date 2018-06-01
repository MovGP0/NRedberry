using System;
using System.Linq;
using NRedberry.Core.Indices;

namespace NRedberry.Core.Combinatorics
{
    public class Permutation : IComparable<Permutation>, IEquatable<Permutation>
    {
        protected int[] permutation;

        /**
         * Constructs identity permutation with specified dimension.
         *
         * @param dimension dimension of permutation
         */
        public Permutation(int dimension)
        {
            permutation = new int[dimension];
            for (int i = 0; i < dimension; ++i)
                permutation[i] = i;
        }

        /**
         * Constructs permutation, specified by {@code permutation} integer array in
         * <i>single-line</i> notation.
         *
         * @param permutation <i>single-line</i> notated integer array, representing
         *                    a permutation.
         * @throws IllegalArgumentException if array is inconsistent with
         *                                  <i>one-line</i> notation
         */
        public Permutation(int[] permutation)
        {
            if (!Combinatorics.TestPermutationCorrectness(permutation))
                throw new ArgumentException("Wrong permutation input: input array is not consistent with one-line notation");
            this.permutation = (int[])permutation.Clone();
        }

        protected Permutation(int[] permutation, bool notClone)
        {
            this.permutation = permutation;
        }

        /**
         * Returns identity permutation.
         *
         * @return identity permutation
         */
        public Permutation GetOne()
        {
            return new Permutation(permutation.Length);
        }

        protected int[] CompositionArray(Permutation element)
        {
            if (permutation.Length != element.permutation.Length)
                throw new ArgumentException("different dimensions of compositing combinatorics");
            int[] perm = new int[permutation.Length];
            for (int i = 0; i < permutation.Length; ++i)
                perm[i] = element.permutation[permutation[i]];
            return perm;
        }

        /**
         * Returns new permutation, witch is a 'left' composition with specified
         * permutation. So, if this permutation A and specified permutation is B, it
         * returns A*B.
         *
         * @param element is a right multiplicand permutation
         * @return composition of element and this combinatorics
         * @throws IllegalArgumentException if element has different dimension than
         *                                  this one
         */
        public Permutation Composition(Permutation element)
        {
            return new Permutation(CompositionArray(element), true);
        }

        /**
         * @param array array to permute
         * @return permuted array copy
         */
        public int[] Permute(int[] array)
        {
            if (array.Length != permutation.Length) throw new ArgumentException("Wrong lenght");

            int[] copy = new int[permutation.Length];
            for (int i = 0; i < permutation.Length; ++i)
                copy[permutation[i]] = array[i];
            return copy;
        }

        protected int[] CalculateInverse()
        {
            var inverse = new int[permutation.Length];
            for (var i = 0; i < permutation.Length; ++i)
                inverse[permutation[i]] = i;
            return inverse;
        }

        /**
         * Returns new index of specified index, i.e. permutation[index]
         *
         * @param index old index
         * @return new index of specified index, i.e. permutation[index]
         */
        public int NewIndexOf(int index)
        {
            return permutation[index];
        }

        /**
         * Returns dimension of permutation.
         *
         * @return dimension of permutation
         */
        public int Dimension()
        {
            return permutation.Length;
        }

        /**
         * Due to immutability of {@code Permuatation} this method returns simple
         * wrapper of integer array, representing one-line notated permutation.
         *
         * @return IntArray representing integer array - one-line notated
         *         permutation
         * @see IntArray
         */
        public IntArray GetPermutation()
        {
            return new IntArray(permutation);
        }

        /**
         * Returns inverse permutation of this one.
         *
         * @return inverse permutation of this one
         */
        public Permutation Inverse()
        {
            return new Permutation(CalculateInverse(), true);
        }

        /**
         * Returns true if {@code obj} has the same class and represents the same
         * permutation and false in the other case.
         *
         * @param obj object to be compared with this
         * @return true if {@code obj} has the same class and represents the same
         *         permutation and false in the other case
         */
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Permutation) obj);
        }

        ///<summary>
        /// Returns hash code of this permutation.
        /// </summary>
        /// <returns>
        /// hash code of this permutation
        /// </returns>
        public override int GetHashCode()
        {
            return (permutation != null ? permutation.GetHashCode() : 0);
        }

        ///<summary>
        /// Returns the string representation of this permutation in one-line
        /// notation.
        /// </summary>
        /// <returns>
        /// the string representation of this permutation in one-line notatin
        /// </returns>
        public override string ToString()
        {
            return permutation.ToString();
        }

        /**
         * Compares this permutation with other. The algorithm sequentially compares
         * integers {@code i1} and {@code i2} in arrays, representing this
         * permutation and other permutation relatively. If on some step {@code i1 > i2}
         * returns 1, if one some step {@code i2 > i1 } returns -1, and if on all
         * steps
         * {@code i1 == i2} returns 0 (combinatorics are equals).
         *
         * @param t permutation to compare
         * @return 1 if this one is "greater" -1 if t is "greater", 0 if this and t
         *         equals.
         * @throws ArgumentException if dimensions of this and t are not '
         *                                  equals
         */

        public int CompareTo(Permutation t)
        {
            if (t.permutation.Length != permutation.Length)
                throw new ArgumentException("different dimensions of comparing combinatorics");

            for (var i = 0; i < permutation.Length; ++i)
            {
                if (permutation[i] < t.permutation[i])
                    return -1;
                if (permutation[i] > t.permutation[i])
                    return 1;
            }

            return 0;
        }

        public bool Compare(int[] permutation)
        {
            if (permutation is null) return false;
            return this.permutation.SequenceEqual(permutation);
        }

        public bool Equals(Permutation other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(permutation, other.permutation);
        }
    }
}