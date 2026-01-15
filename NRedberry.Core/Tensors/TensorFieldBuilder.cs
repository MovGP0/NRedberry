namespace NRedberry.Tensors;

internal sealed class TensorFieldBuilder : TensorBuilder
{
    private readonly TensorField _field;
    private int _pointer;
    private readonly Tensor[] _data;
    private bool _changedSignature;

    public TensorFieldBuilder(TensorField field)
    {
        ArgumentNullException.ThrowIfNull(field);

        _field = field;
        _data = new Tensor[field.Size];
    }

    private TensorFieldBuilder(TensorField field, Tensor[] data, int pointer, bool changedSignature)
    {
        _field = field;
        _data = data;
        _pointer = pointer;
        _changedSignature = changedSignature;
    }

    public Tensor Build()
    {
        if (_pointer != _data.Length)
        {
            throw new InvalidOperationException("Tensor field not fully constructed.");
        }

        if (_changedSignature)
        {
            return TensorField.Create(_field.GetStringName(), _field.SimpleIndices, _data);
        }

        return new TensorField(_field, _data);
    }

    public void Put(Tensor tensor)
    {
        if (_pointer == _data.Length)
        {
            throw new InvalidOperationException("No more arguments in field.");
        }

        ArgumentNullException.ThrowIfNull(tensor);

        if (!tensor.Indices.GetFree().EqualsRegardlessOrder(_field.GetArgIndices(_pointer)))
        {
            if (TensorUtils.IsZeroOrIndeterminate(tensor))
            {
                _changedSignature = true;
            }
            else
            {
                throw new ArgumentException(
                    $"Free indices of putted tensor {tensor.Indices.GetFree()} differs from field argument binding indices {_field.GetArgIndices(_pointer)}!");
            }
        }

        _data[_pointer++] = tensor;
    }

    public TensorBuilder Clone()
    {
        return new TensorFieldBuilder(_field, (Tensor[])_data.Clone(), _pointer, _changedSignature);
    }
}
