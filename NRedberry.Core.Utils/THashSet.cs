namespace NRedberry.Core.Utils;

/// <summary>
/// Port of cc.redberry.core.utils.THashSet.
/// </summary>
/// <typeparam name="T">The tensor type.</typeparam>
[Obsolete("use HashSet<T> instead")]
public sealed class THashSet<T> : HashSet<T>
    where T : class
{
    public THashSet()
    {
    }

    public THashSet(int initialCapacity)
    {
        _ = initialCapacity;
    }

    public THashSet(int initialCapacity, float loadFactor)
    {
        _ = initialCapacity;
        _ = loadFactor;
    }

    public THashSet(IEnumerable<T> values)
        : base(values)
    {
    }
}
