namespace NRedberry.Core.Utils;

/// <summary>
/// Port of cc.redberry.core.utils.THashMap.
/// </summary>
/// <typeparam name="TKey">Key type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
[Obsolete("use Dictionary<TKey,TValue> instead")]
public class THashMap<TKey, TValue> : Dictionary<TKey, TValue>
    where TKey : class
{
    public THashMap()
    {
    }

    public THashMap(int initialCapacity)
        : base(initialCapacity)
    {
    }

    public THashMap(int initialCapacity, float loadFactor)
        : base(initialCapacity)
    {
        _ = loadFactor;
    }
}
