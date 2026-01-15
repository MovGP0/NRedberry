namespace NRedberry.Tensors;

public sealed class BasicTensorIterator : IEnumerator<Tensor>
{
    private readonly Tensor _tensor;
    private readonly int _size;
    private int _position = -1;

    public BasicTensorIterator(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        _tensor = tensor;
        _size = tensor.Size;
    }

    public Tensor Current
    {
        get
        {
            if (_position < 0 || _position >= _size)
            {
                throw new InvalidOperationException("Enumerator is not positioned on a valid element.");
            }

            return _tensor[_position];
        }
    }

    object System.Collections.IEnumerator.Current => Current;

    public bool MoveNext()
    {
        if (!HasNext())
        {
            return false;
        }

        ++_position;
        return true;
    }

    private bool HasNext()
    {
        return _position < _size - 1;
    }

    public void Reset()
    {
        _position = -1;
    }

    public void Dispose()
    {
    }
}
