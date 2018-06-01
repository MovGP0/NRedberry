using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace NRedberry.Core.Combinatorics
{
    ///<summary>
    /// Parent interface for combinatoric iterators
    ///</summary>
    public interface IIntCombinatorialGenerator : IEnumerable<int[]>, IEnumerator<int[]>
    {
    }

    public interface IOutputPortUnsafe<out T>
    {
        T Take();
    }

    /// <summary>
    /// This interface is common for all combinatorial iterators.
    /// </summary>
    public interface IIntCombinatorialPort : IOutputPortUnsafe<int[]>
    {
        /// <summary>
        /// Resets the iteration
        /// </summary>
        void Reset();

        /**
         * Returns the reference to the current iteration element.
         *
         * @return the reference to the current iteration element
         */
        int[] GetReference();

        /**
         * Calculates and returns the next combination or null, if no more combinations exist.
         *
         * @return the next combination or null, if no more combinations exist
         */
        //int[] Take();
    }

    public sealed class IntPermutationsGenerator : IIntCombinatorialGenerator
    {
        public IEnumerator<int[]> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }

        public int[] Current { get; }

        object IEnumerator.Current => Current;

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public sealed class IntCombinationPermutationGenerator : IIntCombinatorialPort, IIntCombinatorialGenerator
    {
        public int[] Take()
        {
            throw new NotImplementedException();
        }

        public bool MoveNext()
        {
            throw new NotImplementedException();
        }

        void IEnumerator.Reset()
        {
            throw new NotImplementedException();
        }

        public int[] Current { get; }

        object IEnumerator.Current => Current;

        void IIntCombinatorialPort.Reset()
        {
            throw new NotImplementedException();
        }

        public int[] GetReference()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<int[]> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// This class provides factory and utility methods for combinatorics infrastructure.
    /// </summary>
    public static class Combinatorics
    {
        ///<summary>
        /// <p>Returns an {@link IntCombinatorialGenerator} object, which allows to iterate over
        /// all possible unique combinations with permutations (i.e. {0,1} and {1,0} both appears for <paramref name="k"/>=2) of
        /// <paramref name="k"/> numbers, which can be chosen from the set of {@code n} numbers, numbered in the order
        /// 0,1,2,...,<paramref name="n"/>. The total number of such combinations will be {@code n!/(n-k)!}.</p>
        ///
        /// <p>For example, for {@code k=2} and {@code n=3}, this method will produce an iterator over
        /// the following arrays: [0,1], [1,0], [0,2], [2,0], [1,2], [2,1].</p>
        /// </summary>
        /// <param name="n">number of elements in the set</param>
        /// <param name="k">sample size</param>
        /// <returns>
        /// an iterator over all combinations (with permutations) to choose k numbers from n numbers.
        /// </returns>
        public static IIntCombinatorialGenerator CreateIntGenerator(int n, int k)
        {
            throw new NotImplementedException();
            //if (n < k)
            //    throw new ArgumentException();

            //return n == k
            //    ? new IntPermutationsGenerator(n)
            //    : new IntCombinationPermutationGenerator(n, k);
        }

        /**
         * Checks whether specified permutation written in one-line notation is identity.
         *
         * @param permutation permutation in one-line notation
         * @return {@code true} if permutation is identity, {@code false} if not
         */
        public static bool IsIdentity(int[] permutation)
        {
            return !permutation.Where((t, i) => t != i).Any();
        }

        /**
         * Checks whether specified permutation is identity.
         *
         * @param permutation permutation
         * @return {@code true} if permutation is identity, {@code false} if not
         */
        public static bool IsIdentity(Permutation permutation)
        {
            throw new NotImplementedException();
            // return IsIdentity(permutation.permutation);
        }

        /**
         * Checks whether specified symmetry is identity, i.e. it
         * represents an identity permutation and have positive sign.
         *
         * @param symmetry symmetry
         * @return {@code true} if symmetry is identity and have positive sign, and {@code false} if not
         */
        public static bool IsIdentity(Symmetry symmetry)
        {
            throw new NotImplementedException();
            // return !symmetry.IsAntiSymmetry() && IsIdentity(symmetry.Permutation);
        }

        /**
         * Returns an identity permutation written in one-line notation,
         * i.e. an array of length {@code dimension} filled with consecutive numbers.
         *
         * @param dimension dimension
         * @return identity permutation written in one-line notation
         */
        public static int[] CreateIdentity(int dimension)
        {
            int[] perm = new int[dimension];
            for (int i = 0; i < dimension; ++i)
                perm[i] = i;
            return perm;
        }

        /**
         * Creates transposition of first two elements written in one-line notation
         * with specified dimension, i.e. an array of form [1,0,2,3,4,...,{@code dimension - 1}].
         *
         * @param dimension dimension of the resulting permutation, e.g. the array length
         * @return transposition permutation in one-line notation
         */
        public static int[] CreateTransposition(int dimension)
        {
            if (dimension < 0)
                throw new ArgumentException("Dimension is negative.");
            if (dimension > 1)
                return CreateTransposition(dimension, 0, 1);
            return new int[dimension];
        }

        /**
         * Creates transposition in one-line notation
         *
         * @param dimension dimension of the resulting permutation, e.g. the array length
         * @param position1 first position
         * @param position2 second position
         * @return transposition
         */
        public static int[] CreateTransposition(int dimension, int position1, int position2)
        {
            if (dimension < 0)
                throw new ArgumentException("Dimension is negative.");
            if (position1 < 0 || position2 < 0)
                throw new ArgumentException("Negative index.");
            if (position1 >= dimension || position2 >= dimension)
                throw new IndexOutOfRangeException();

            int[] transposition = new int[dimension];
            int i = 1;
            for (; i < dimension; ++i)
                transposition[i] = i;
            i = transposition[position1];
            transposition[position1] = transposition[position2];
            transposition[position2] = i;
            return transposition;
        }

        /**
         * Creates cycle permutation written in one-line notation,
         * i.e. an array of form [{@code dimension-1},0,1, ...,{@code dimension-2}].
         *
         * @param dimension dimension of the resulting permutation, e.g. the array length
         * @return cycle permutation in one-line notation
         */
        public static int[] CreateCycle(int dimension)
        {
            if (dimension < 0)
                throw new ArgumentException("Negative dimension");

            int[] cycle = new int[dimension];
            for (int i = 0; i < dimension - 1; ++i)
                cycle[i + 1] = i;
            cycle[0] = dimension - 1;
            return cycle;
        }

        /**
         * Returns the inverse permutation for the specified one.
         *
         * <p>One-line notation for permutations is used.</p>
         *
         * @param permutation permutation in one-line notation
         * @return inverse permutation to the specified one
         */
        public static int[] Inverse(int[] permutation)
        {
            int[] inverse = new int[permutation.Length];
            for (int i = 0; i < permutation.Length; ++i)
                inverse[permutation[i]] = i;
            return inverse;
        }

        /**
         * Shuffles the specified array according to the specified permutation and returns the result.
         * The inputting array will be cloned.
         *
         * @param array       array
         * @param permutation permutation in one-line notation
         * @param <T>         any type
         * @return new shuffled array
         * @throws ArgumentException if array length not equals to permutation length
         * @throws ArgumentException if permutation is not consistent with one-line notation
         */
        public static T[] Shuffle<T>(T[] array, int[] permutation)
        {
            if (array.Length != permutation.Length) throw new ArgumentException();
            if (!TestPermutationCorrectness(permutation)) throw new ArgumentException();

            var newArray = new T[array.Length];
            for (var i = 0; i < permutation.Length; ++i)
            {
                newArray[i] = array[permutation[i]];
            }

            return newArray;
        }

        /**
         * Reorder the specified array according to the specified permutation and returns the result.
         * The inputting array will be cloned.
         *
         * @param array       array
         * @param permutation permutation in one-line notation
         * @return new shuffled array
         * @throws ArgumentException if array length not equals to permutation length
         * @throws ArgumentException if permutation is not consistent with one-line notation
         */
        public static int[] Reorder(int[] array, int[] permutation)
        {
            if (array.Length != permutation.Length)
                throw new ArgumentException();
            if (!TestPermutationCorrectness(permutation))
                throw new ArgumentException();
            int[] newArray = new int[array.Length];
            for (int i = 0; i < permutation.Length; ++i)
                newArray[i] = array[permutation[i]];
            return newArray;
        }

        /**
         * Tests whether the specified array satisfies the one-line notation for permutations
         *
         * @param permutation array to be tested
         * @return {@code true} if specified array satisfies the one-line
         *         notation for permutations and {@code false} if not
         */
        public static bool TestPermutationCorrectness(int[] permutation)
        {
            //TODO cloning just for error testing?
            int[] _permutation = new int[permutation.Length];
            Array.Copy(permutation, 0, _permutation, 0, permutation.Length);
            Array.Sort(_permutation);
            for (int i = 0; i < _permutation.Length; ++i)
                if (_permutation[i] != i)
                    return false;
            return true;
        }

        /**
         * Check that fromIndex and toIndex are in range, and throw an appropriate
         * exception if they are not.
         */
        private static void RangeCheck(int arrayLen, int fromIndex, int toIndex)
        {
            if (fromIndex > toIndex) throw new ArgumentException($"fromIndex({fromIndex}) > toIndex({toIndex})");
            if (fromIndex < 0) throw new IndexOutOfRangeException(nameof(fromIndex));
            if (toIndex > arrayLen) throw new IndexOutOfRangeException(nameof(toIndex));
        }

        /**
         * Check that all positions are less than dimension, and throw an
         * appropriate exception if they aren't.
         */
        private static void RangeCheck(int dimension, params int[] positions)
        {
            if (dimension < 0) throw new ArgumentException("Dimension is negative.");

            foreach (var i in positions)
            {
                if (i < 0) throw new ArgumentException("Negative index " + i + ".");
                if (i >= dimension) throw new IndexOutOfRangeException();
            }
        }
    }
}