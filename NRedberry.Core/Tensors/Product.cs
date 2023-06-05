using System;
using NRedberry.Core.Indices;
using Complex = NRedberry.Core.Numbers.Complex;

namespace NRedberry.Core.Tensors;

public sealed class Product : MultiTensor
{
    private Complex Factor { get; }
    private Tensor[] IndexlessData { get; }
    private Tensor[] Data { get; }
    private WeakReference<ProductContent> ContentReference { get; }

    public Product(IIndices indices) : base(indices)
    {
    }

    protected override int Hash()
    {
        throw new NotImplementedException();
    }

    public override Tensor this[int i] => throw new NotImplementedException();

    public override int Size { get; }
    public override string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override ITensorBuilder GetBuilder()
    {
        throw new NotImplementedException();
    }

    public override ITensorFactory GetFactory()
    {
        throw new NotImplementedException();
    }

    public override Tensor Remove(uint position)
    {
        throw new NotImplementedException();
    }

    protected override Complex GetNeutral()
    {
        throw new NotImplementedException();
    }

    protected override Tensor Select1(uint[] positions)
    {
        throw new NotImplementedException();
    }

    internal Tensor[] GetAllScalars()
    {
        throw new NotImplementedException();
    }
}