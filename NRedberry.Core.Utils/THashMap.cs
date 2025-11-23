namespace NRedberry.Core.Utils;

/// <summary>
/// Port of cc.redberry.core.utils.THashMap.
/// </summary>
/// <typeparam name="TKey">Key type.</typeparam>
/// <typeparam name="TValue">Value type.</typeparam>
[Obsolete("use Dictionary<TKey,TValue>  instead", true)]
public class THashMap<TKey, TValue>
    where TKey : class;
