namespace NRedberry.Core.Utils;

/// <summary>
/// Port of cc.redberry.core.utils.TensorWrapperWithEquals.
/// </summary>
public sealed class TensorWrapperWithEquals
{
    private object _tensor;

    /// <summary>
    /// Initializes a new instance of the wrapper.
    /// </summary>
    /// <param name="tensor">The tensor to wrap.</param>
    public TensorWrapperWithEquals(object tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        _tensor = tensor;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is TensorWrapperWithEquals other
            && Equals(_tensor, other._tensor);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return _tensor.GetHashCode();
    }

    /// <summary>
    /// Gets the wrapped tensor instance.
    /// </summary>
    /// <returns>The wrapped tensor.</returns>
    public object GetTensor()
    {
        return _tensor;
    }
}
