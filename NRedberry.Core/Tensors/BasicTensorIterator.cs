namespace NRedberry.Tensors;

public sealed class BasicTensorIterator(Tensor tensor) : IEnumerator<Tensor>
{
    private Tensor Tensor { get; } = tensor ?? throw new ArgumentNullException(nameof(tensor));
    private int Position { get; set; } = -1;
    private int Size => Tensor.Size;

    public object Current => Tensor[Position];

    Tensor IEnumerator<Tensor>.Current => Tensor[Position];

    public bool MoveNext()
    {
        if (!HasNext())
            return false;
        ++Position;
        return true;
    }

    private bool HasNext()
    {
        return Position < Size - 1;
    }

    public void Reset()
    {
        Position = -1;
    }

    #region IDisposable

    private bool IsDisposed { get; set; }

    public void Dispose()
    {
        if (!IsDisposed)
            Dispose(true);
    }

    public void Dispose(bool disposing)
    {
        IsDisposed = true;
    }

    ~BasicTensorIterator()
    {
        if (!IsDisposed)
            Dispose(false);
    }

    #endregion
}
