namespace NRedberry.Core.Utils;

/// <summary>
/// Port of cc.redberry.core.utils.SoftReferenceWrapper.
/// </summary>
/// <typeparam name="T">The referent type.</typeparam>
public class SoftReferenceWrapper<T>
    where T : class
{
    /// <summary>
    /// Initializes a new instance without an initial reference.
    /// </summary>
    public SoftReferenceWrapper()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Initializes a new instance with an existing weak reference.
    /// </summary>
    /// <param name="reference">The weak reference to wrap.</param>
    public SoftReferenceWrapper(WeakReference<T>? reference)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Initializes a new instance with the specified referent.
    /// </summary>
    /// <param name="referent">The referent to wrap.</param>
    public SoftReferenceWrapper(T? referent)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the wrapped weak reference.
    /// </summary>
    /// <returns>The wrapped weak reference.</returns>
    public WeakReference<T>? GetReference()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Replaces the wrapped weak reference.
    /// </summary>
    /// <param name="reference">The new reference to wrap.</param>
    public void ResetReference(WeakReference<T>? reference)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Replaces the wrapped weak reference with one created from the provided referent.
    /// </summary>
    /// <param name="referent">The referent to wrap.</param>
    public void ResetReferent(T? referent)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the referent if it is still available.
    /// </summary>
    /// <returns>The referent, or null if it has been collected.</returns>
    public T? GetReferent()
    {
        throw new NotImplementedException();
    }
}
