using System.Collections;

namespace NRedberry.Core.Utils;

/// <summary>
/// Port of cc.redberry.core.utils.THashSet.
/// </summary>
/// <typeparam name="T">The tensor type.</typeparam>
public sealed class THashSet<T> : IEnumerable<T>
    where T : class
{
    /// <summary>
    /// Initializes an empty set.
    /// </summary>
    public THashSet()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Initializes an empty set with the specified capacity.
    /// </summary>
    /// <param name="initialCapacity">Initial capacity.</param>
    public THashSet(int initialCapacity)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Initializes an empty set with the specified capacity and load factor.
    /// </summary>
    /// <param name="initialCapacity">Initial capacity.</param>
    /// <param name="loadFactor">Desired load factor.</param>
    public THashSet(int initialCapacity, float loadFactor)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Initializes a set seeded with the provided tensors.
    /// </summary>
    /// <param name="tensors">Initial tensors.</param>
    public THashSet(IEnumerable<T> tensors)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds a tensor to the set.
    /// </summary>
    /// <param name="tensor">Tensor to add.</param>
    /// <returns><c>true</c> if the set changed.</returns>
    public bool Add(T tensor)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds all tensors from the provided collection.
    /// </summary>
    /// <param name="tensors">Collection to merge.</param>
    /// <returns><c>true</c> if the set changed.</returns>
    public bool AddAll(IEnumerable<T> tensors)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes all tensors.
    /// </summary>
    public void Clear()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Determines whether the set contains the specified object.
    /// </summary>
    /// <param name="item">Object to test.</param>
    /// <returns><c>true</c> if present.</returns>
    public bool Contains(object? item)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Determines whether all items are contained within the set.
    /// </summary>
    /// <param name="items">Items to test.</param>
    /// <returns><c>true</c> if the set contains every element.</returns>
    public bool ContainsAll(IEnumerable<object?> items)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Indicates whether the set is empty.
    /// </summary>
    /// <returns><c>true</c> when empty.</returns>
    public bool IsEmpty()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns an enumerator over tensors in the set.
    /// </summary>
    /// <returns>Enumerator.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    /// <summary>
    /// Removes the specified object.
    /// </summary>
    /// <param name="item">Item to remove.</param>
    /// <returns><c>true</c> if the set changed.</returns>
    public bool Remove(object? item)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes all matching items from the set.
    /// </summary>
    /// <param name="items">Items to remove.</param>
    /// <returns><c>true</c> if the set changed.</returns>
    public bool RemoveAll(IEnumerable<object?> items)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retains only the items present in the provided collection.
    /// </summary>
    /// <param name="items">Items to retain.</param>
    /// <returns><c>true</c> if the set changed.</returns>
    public bool RetainAll(IEnumerable<object?> items)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the number of elements in the set.
    /// </summary>
    /// <returns>Element count.</returns>
    public int Size()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the contents as an object array.
    /// </summary>
    /// <returns>Array copy.</returns>
    public object[] ToArray()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Copies the contents into the provided array.
    /// </summary>
    /// <typeparam name="TElement">Array element type.</typeparam>
    /// <param name="array">Target array.</param>
    /// <returns>The populated array.</returns>
    public TElement[] ToArray<TElement>(TElement[] array)
    {
        throw new NotImplementedException();
    }
}
