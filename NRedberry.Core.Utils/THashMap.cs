namespace NRedberry.Core.Utils;

/// <summary>
/// Port of cc.redberry.core.utils.THashMap.
/// </summary>
/// <typeparam name="TKey">Key type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
public class THashMap<TKey, TValue>
    where TKey : class
{
    /// <summary>
    /// Initializes a map with default capacity.
    /// </summary>
    public THashMap()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Initializes a map with the specified capacity.
    /// </summary>
    /// <param name="initialCapacity">Initial capacity.</param>
    public THashMap(int initialCapacity)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Initializes a map with the specified capacity and load factor.
    /// </summary>
    /// <param name="initialCapacity">Initial capacity.</param>
    /// <param name="loadFactor">Desired load factor.</param>
    public THashMap(int initialCapacity, float loadFactor)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds or replaces a value associated with the provided key.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="value">Value.</param>
    /// <returns>The previous value associated with the key.</returns>
    public TValue Put(TKey key, TValue value)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Retrieves a value associated with the provided key.
    /// </summary>
    /// <param name="key">Key to lookup.</param>
    /// <returns>The associated value.</returns>
    public TValue Get(object? key)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Determines whether the map contains the specified key.
    /// </summary>
    /// <param name="key">Key to check.</param>
    /// <returns><c>true</c> if the key is present.</returns>
    public bool ContainsKey(object? key)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Determines whether the map contains the specified value.
    /// </summary>
    /// <param name="value">Value to check.</param>
    /// <returns><c>true</c> if the value is present.</returns>
    public bool ContainsValue(object? value)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns whether the map has zero elements.
    /// </summary>
    /// <returns><c>true</c> when empty.</returns>
    public bool IsEmpty()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes the entry associated with the provided key.
    /// </summary>
    /// <param name="key">Key to remove.</param>
    /// <returns>The removed value.</returns>
    public TValue Remove(object? key)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the number of stored entries.
    /// </summary>
    /// <returns>Entry count.</returns>
    public int Size()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns a view of the stored values.
    /// </summary>
    /// <returns>Collection of values.</returns>
    public ICollection<TValue> Values()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes all entries from the map.
    /// </summary>
    public void Clear()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns a view of the stored keys.
    /// </summary>
    /// <returns>Set of keys.</returns>
    public ISet<TKey> KeySet()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns a view over key/value pairs.
    /// </summary>
    /// <returns>Collection of entries.</returns>
    public ISet<KeyValuePair<TKey, TValue>> EntrySet()
    {
        throw new NotImplementedException();
    }
}
