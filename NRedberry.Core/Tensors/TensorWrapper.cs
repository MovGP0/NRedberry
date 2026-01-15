using NRedberry.Utils;

namespace NRedberry.Tensors;

/// <summary>
/// Technical class.
/// </summary>
public sealed class TensorWrapper : Tensor, IComparable<TensorWrapper>
{
    private readonly Tensor _innerTensor;
    private readonly int _hashWithIndices;

    public TensorWrapper(Tensor tensor)
    {
        _innerTensor = tensor ?? throw new ArgumentNullException(nameof(tensor));
        _hashWithIndices = TensorHashCalculator.HashWithIndices(tensor);
    }

    public static TensorWrapper Wrap(Tensor tensor)
    {
        return new TensorWrapper(tensor);
    }

    public override Tensor this[int i]
    {
        get
        {
            if (i != 0)
            {
                throw new IndexOutOfRangeException();
            }

            return _innerTensor;
        }
    }

    public override int Size
    {
        get { return 1; }
    }

    public override Indices.Indices Indices
    {
        get { throw new NotSupportedException("Not supported yet."); }
    }

    public override TensorBuilder GetBuilder()
    {
        return new TensorWrapperBuilder();
    }

    public override TensorFactory? GetFactory()
    {
        return TensorWrapperFactory.Instance;
    }

    public override int GetHashCode()
    {
        return -1;
    }

    public override string ToString(OutputFormat outputFormat)
    {
        return "@[" + _innerTensor.ToString(outputFormat) + "]";
    }

    public int CompareTo(TensorWrapper? other)
    {
        if (other is null)
        {
            return -1;
        }

        var i = _innerTensor.CompareTo(other._innerTensor);

        return i != 0
            ? i
            : _hashWithIndices.CompareTo(other._hashWithIndices);
    }
}

internal sealed class TensorWrapperBuilder : TensorBuilder
{
    private Tensor? _innerTensor;

    public TensorWrapperBuilder()
    {
    }

    public TensorWrapperBuilder(Tensor? innerTensor)
    {
        _innerTensor = innerTensor;
    }

    public Tensor Build()
    {
        if (_innerTensor is null)
        {
            throw new InvalidOperationException("No elements added.");
        }

        return new TensorWrapper(_innerTensor);
    }

    public void Put(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        if (_innerTensor is not null)
        {
            throw new TensorException("Wrapper have only one element!");
        }

        _innerTensor = tensor;
    }

    public TensorBuilder Clone()
    {
        return new TensorWrapperBuilder(_innerTensor);
    }
}

internal sealed class TensorWrapperFactory : TensorFactory
{
    public static TensorWrapperFactory Instance { get; } = new();

    private TensorWrapperFactory()
    {
    }

    public Tensor Create(params Tensor[] tensors)
    {
        ArgumentNullException.ThrowIfNull(tensors);
        if (tensors.Length != 1)
        {
            throw new ArgumentException("TensorWrapper requires exactly one tensor.", nameof(tensors));
        }

        ArgumentNullException.ThrowIfNull(tensors[0]);
        return new TensorWrapper(tensors[0]);
    }
}
