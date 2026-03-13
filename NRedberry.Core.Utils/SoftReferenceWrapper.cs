namespace NRedberry.Core.Utils;

/// <summary>
/// Port of cc.redberry.core.utils.SoftReferenceWrapper.
/// </summary>
/// <typeparam name="T">The referent type.</typeparam>
[Obsolete("Use WeakReference<T> instead")]
public class SoftReferenceWrapper<T>
    where T : class
{
    private WeakReference<T>? _reference;

    public SoftReferenceWrapper()
    {
    }

    public SoftReferenceWrapper(WeakReference<T>? reference)
    {
        _reference = reference;
    }

    public SoftReferenceWrapper(T referent)
    {
        ResetReferent(referent);
    }

    public WeakReference<T>? GetReference()
    {
        return _reference;
    }

    public void ResetReference(WeakReference<T>? reference)
    {
        _reference = reference;
    }

    public void ResetReferent(T referent)
    {
        ArgumentNullException.ThrowIfNull(referent);
        _reference = new WeakReference<T>(referent);
    }

    public T? GetReferent()
    {
        if (_reference is null)
        {
            return null;
        }

        return _reference.TryGetTarget(out T? referent) ? referent : null;
    }
}
