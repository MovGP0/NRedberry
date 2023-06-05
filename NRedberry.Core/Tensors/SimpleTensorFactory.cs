using System;

namespace NRedberry.Core.Tensors;

public sealed class SimpleTensorFactory : ITensorFactory
{
    private SimpleTensor SimpleTensor { get; }

    public SimpleTensorFactory(SimpleTensor simpleTensor)
    {
        SimpleTensor = simpleTensor ?? throw new ArgumentNullException(nameof(simpleTensor));
    }

    public Tensor Create(params Tensor[] tensors)
    {
        if (tensors.Length != 0) throw new NotSupportedException("Don't provide tensors here.");
        return SimpleTensor;
    }
}