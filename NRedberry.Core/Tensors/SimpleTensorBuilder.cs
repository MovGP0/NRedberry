using System;

namespace NRedberry.Core.Tensors;

public sealed class SimpleTensorBuilder : TensorBuilder
{
    private SimpleTensor Tensor { get; }

    public SimpleTensorBuilder(SimpleTensor tensor)
    {
        Tensor = tensor ?? throw new ArgumentNullException(nameof(tensor));
    }

    public Tensor Build()
    {
        return Tensor;
    }

    public void Put(Tensor tensor)
    {
        throw new NotSupportedException("Can not put to SimpleTensor builder!");
    }

    public TensorBuilder Clone()
    {
        return this;
    }
}