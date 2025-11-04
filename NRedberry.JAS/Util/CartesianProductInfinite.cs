using System.Collections;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;

/// <summary>
/// Cartesian product of infinite components with iterator. Works also for finite iterables.
/// </summary>
/// <typeparam name="E">element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.util.CartesianProductInfinite
/// </remarks>
public class CartesianProductInfinite<E> : IEnumerable<List<E>>
{
    /// <summary>
    /// Data structure.
    /// </summary>
    public readonly List<IEnumerable<E>> Comps;

    /// <summary>
    /// CartesianProduct constructor.
    /// </summary>
    /// <param name="comps">components of the Cartesian product</param>
    public CartesianProductInfinite(List<IEnumerable<E>> comps)
    {
        if (comps == null || comps.Count == 0)
        {
            throw new ArgumentException("null or empty components not allowed", nameof(comps));
        }

        Comps = comps;
    }

    /// <summary>
    /// Get an iterator over subsets.
    /// </summary>
    /// <returns>an iterator.</returns>
    public IEnumerator<List<E>> GetEnumerator()
    {
        return new InfiniteProductEnumerator(Comps);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private sealed class InfiniteProductEnumerator : IEnumerator<List<E>>
    {
        private readonly List<IEnumerable<E>> components;
        private readonly List<IEnumerator<E>> iterators;
        private readonly List<List<E>> values;
        private readonly Queue<int[]> pendingIndices;
        private readonly int dimension;
        private long level;
        private bool finished;
        private List<E>? current;

        public InfiniteProductEnumerator(List<IEnumerable<E>> comps)
        {
            components = comps ?? throw new ArgumentNullException(nameof(comps));
            dimension = components.Count;
            iterators = new List<IEnumerator<E>>(dimension);
            values = new List<List<E>>(dimension);
            pendingIndices = new Queue<int[]>();
            level = 0;
            finished = false;

            for (int i = 0; i < dimension; i++)
            {
                IEnumerable<E> component = components[i] ?? throw new ArgumentNullException(nameof(comps), "Component enumerable must not be null.");
                IEnumerator<E> iterator = component.GetEnumerator();
                if (!iterator.MoveNext())
                {
                    finished = true;
                    iterator.Dispose();
                    DisposeIterators(iterators);
                    return;
                }

                iterators.Add(iterator);
                values.Add([iterator.Current]);
            }

            EnsureQueue();
        }

        public List<E> Current => current ?? throw new InvalidOperationException("Enumeration has not started or has finished.");

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (finished || pendingIndices.Count == 0)
            {
                current = null;
                return false;
            }

            int[] indices = pendingIndices.Dequeue();
            current = BuildCombination(indices);

            if (pendingIndices.Count == 0)
            {
                EnsureQueue();
            }

            return true;
        }

        public void Reset() => throw new NotSupportedException("Reset is not supported.");

        public void Dispose()
        {
            DisposeIterators(iterators);
        }

        private void EnsureQueue()
        {
            if (finished)
            {
                return;
            }

            while (pendingIndices.Count == 0)
            {
                bool added = false;
                bool fetchedAny = false;

                foreach (int[] indices in GenerateIndexTuples(dimension, level))
                {
                    if (EnsureValuesAvailable(indices, ref fetchedAny))
                    {
                        pendingIndices.Enqueue(indices);
                        added = true;
                    }
                }

                if (added)
                {
                    break;
                }

                if (!fetchedAny)
                {
                    finished = true;
                    break;
                }

                level++;
            }
        }

        private bool EnsureValuesAvailable(int[] indices, ref bool fetchedAny)
        {
            for (int i = 0; i < dimension; i++)
            {
                if (!EnsureValue(i, indices[i], ref fetchedAny))
                {
                    return false;
                }
            }

            return true;
        }

        private bool EnsureValue(int componentIndex, int targetIndex, ref bool fetchedAny)
        {
            List<E> list = values[componentIndex];
            IEnumerator<E> iterator = iterators[componentIndex];

            while (list.Count <= targetIndex)
            {
                if (!iterator.MoveNext())
                {
                    return false;
                }

                list.Add(iterator.Current);
                fetchedAny = true;
            }

            return true;
        }

        private List<E> BuildCombination(int[] indices)
        {
            List<E> result = [];
            for (int i = 0; i < dimension; i++)
            {
                result.Add(values[i][indices[i]]);
            }

            return result;
        }

        private static IEnumerable<int[]> GenerateIndexTuples(int dimension, long level)
        {
            int[] indices = new int[dimension];
            foreach (int[] tuple in GenerateRecursive(indices, 0, dimension, level))
            {
                yield return tuple;
            }
        }

        private static IEnumerable<int[]> GenerateRecursive(int[] indices, int position, int dimension, long remaining)
        {
            if (position == dimension - 1)
            {
                indices[position] = (int)remaining;
                int[] copy = new int[dimension];
                Array.Copy(indices, copy, dimension);
                yield return copy;
                yield break;
            }

            for (long i = 0; i <= remaining; i++)
            {
                indices[position] = (int)i;
                foreach (int[] tuple in GenerateRecursive(indices, position + 1, dimension, remaining - i))
                {
                    yield return tuple;
                }
            }
        }

        private static void DisposeIterators(List<IEnumerator<E>> iterators)
        {
            foreach (IEnumerator<E> iterator in iterators)
            {
                iterator.Dispose();
            }
        }
    }
}
