using System;
using NRedberry.Core.Indices;
using Complex = NRedberry.Core.Numbers.Complex;

namespace NRedberry.Core.Tensors;

public sealed class Product : MultiTensor
{
    private Complex Factor { get; }
    private Tensor[] IndexlessData { get; }
    private Tensor[] Data { get; }
    private WeakReference<ProductContent> contentReference;

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

    protected override Tensor Remove1(int[] positions)
    {
        throw new NotImplementedException();
    }

    public override Tensor Remove(int position)
    {
        throw new NotImplementedException();
    }

    protected override Complex GetNeutral()
    {
        throw new NotImplementedException();
    }

    protected override Tensor Select1(int[] positions)
    {
        throw new NotImplementedException();
    }

    internal Tensor[] GetAllScalars()
    {
        throw new NotImplementedException();
    }

    public ProductContent Content
    {
        get
        {
            var success = contentReference.TryGetTarget(out var content);
            if (!success)
            {
                content = CalculateContent();
                contentReference.SetTarget(content);
            }
            return content;
        }
    }
}