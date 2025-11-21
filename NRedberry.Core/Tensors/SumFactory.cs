using NRedberry.Numbers;

namespace NRedberry.Tensors;

public sealed class SumFactory : TensorFactory
{
    public static readonly SumFactory Factory = new();

    private SumFactory()
    {
    }

    public Tensor Create(params Tensor[] tensors)
    {
        if (tensors.Length == 0)
            return Complex.Zero;
        if (tensors.Length == 1)
            return tensors[0];

        var builder = new SumBuilder(tensors.Length);
        foreach (var t in tensors)
        {
            builder.Put(t);
        }

        return builder.Build();
    }

    public Tensor Create(Tensor tensor)
    {
        throw new System.NotImplementedException();
    }
}
