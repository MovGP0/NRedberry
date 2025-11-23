namespace NRedberry.Core.Utils;

/// <summary>
/// Port of cc.redberry.core.utils.SoftReferenceWrapper.
/// </summary>
/// <typeparam name="T">The referent type.</typeparam>
[Obsolete("Use WeakReference<T> instead", true)]
public class SoftReferenceWrapper<T>
    where T : class;
