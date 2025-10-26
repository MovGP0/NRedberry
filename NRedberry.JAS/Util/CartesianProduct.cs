using System.Collections;

namespace NRedberry.Core.Transformations.Factor.Jasfactor.Edu.Jas.Util;

/// <summary>
/// Cartesian product with iterator.
/// </summary>
/// <typeparam name="E">element type</typeparam>
/// <remarks>
/// Original Java file: cc.redberry.core.transformations.factor.jasfactor.edu.jas.util.CartesianProduct
/// </remarks>
public class CartesianProduct<E> : IEnumerable<List<E>>
{
    /// <summary>
    /// Data structure.
    /// </summary>
    public readonly List<IEnumerable<E>> Comps;

    /// <summary>
    /// CartesianProduct constructor.
    /// </summary>
    /// <param name="comps">components of the Cartesian product</param>
    public CartesianProduct(List<IEnumerable<E>> comps)
    {
        Comps = comps ?? throw new ArgumentNullException(nameof(comps));
    }

    /// <summary>
    /// Get an iterator over subsets.
    /// </summary>
    /// <returns>an iterator.</returns>
    public IEnumerator<List<E>> GetEnumerator()
    {
        return new CartesianProductEnumerator(Comps);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private sealed class CartesianProductEnumerator : IEnumerator<List<E>>
    {
        private readonly List<IEnumerable<E>> comps;
        private readonly List<IEnumerator<E>> compEnumerators;
        private readonly List<E> current;
        private bool empty;
        private List<E>? snapshot;

        public CartesianProductEnumerator(List<IEnumerable<E>> comps)
        {
            this.comps = comps ?? throw new ArgumentNullException(nameof(comps));
            compEnumerators = new List<IEnumerator<E>>(comps.Count);
            current = new List<E>(comps.Count);
            empty = false;

            foreach (IEnumerable<E> component in comps)
            {
                if (component == null)
                {
                    throw new ArgumentNullException(nameof(comps), "Component enumerable must not be null");
                }

                IEnumerator<E> iterator = component.GetEnumerator();
                compEnumerators.Add(iterator);

                if (!iterator.MoveNext())
                {
                    empty = true;
                    current.Clear();
                    break;
                }

                current.Add(iterator.Current);
            }

            if (empty)
            {
                snapshot = null;
            }
        }

        public List<E> Current => snapshot ?? throw new InvalidOperationException("Enumeration has not started or has finished.");

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (empty)
            {
                snapshot = null;
                return false;
            }

            snapshot = new List<E>(current);

            int pivot = compEnumerators.Count - 1;
            for (; pivot >= 0; pivot--)
            {
                if (compEnumerators[pivot].MoveNext())
                {
                    break;
                }
            }

            if (pivot < 0)
            {
                empty = true;
                return true;
            }

            for (int j = pivot + 1; j < compEnumerators.Count; j++)
            {
                compEnumerators[j].Dispose();
                IEnumerator<E> resetEnumerator = comps[j].GetEnumerator();
                compEnumerators[j] = resetEnumerator;
                if (!resetEnumerator.MoveNext())
                {
                    empty = true;
                    return true;
                }
            }

            for (int j = pivot; j < compEnumerators.Count; j++)
            {
                current[j] = compEnumerators[j].Current;
            }

            return true;
        }

        public void Reset()
        {
            throw new NotSupportedException("Reset is not supported.");
        }

        public void Dispose()
        {
            foreach (IEnumerator<E> iterator in compEnumerators)
            {
                iterator.Dispose();
            }
        }
    }
}
