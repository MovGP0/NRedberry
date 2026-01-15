namespace NRedberry.Tensors;

internal sealed class TensorFieldFactory : TensorFactory
{
    private readonly TensorField _field;

    public TensorFieldFactory(TensorField field)
    {
        ArgumentNullException.ThrowIfNull(field);
        _field = field;
    }

    public Tensor Create(params Tensor[] tensors)
    {
        ArgumentNullException.ThrowIfNull(tensors);

        if (tensors.Length != _field.Size)
        {
            throw new ArgumentException("Wrong arguments count.");
        }

        bool changedSignature = false;
        for (int i = tensors.Length - 1; i >= 0; --i)
        {
            ArgumentNullException.ThrowIfNull(tensors[i]);
            if (!tensors[i].Indices.GetFree().EqualsRegardlessOrder(_field.GetArgIndices(i)))
            {
                if (TensorUtils.IsZeroOrIndeterminate(tensors[i]))
                {
                    changedSignature = true;
                }
                else
                {
                    throw new ArgumentException(
                        $"Free indices of putted tensor {tensors[i].Indices.GetFree()} differs from field argument binding indices {_field.GetArgIndices(i)}!");
                }
            }
        }

        if (changedSignature)
        {
            return TensorField.Create(_field.GetStringName(), _field.SimpleIndices, tensors);
        }

        return new TensorField(_field, tensors);
    }
}
