using System.Diagnostics;
using NRedberry.Core.Numbers;
using NRedberry.Core.Utils;

namespace NRedberry.Core.Tensors;

public sealed class Sum : MultiTensor
{
    internal readonly Tensor[] Data;
    private int hash;

    public Sum(Tensor[] data, Indices.Indices indices)
        : base(indices)
    {
        Debug.Assert(data.Length > 1);

        this.Data = data;
        TensorWrapper[] wrappers = new TensorWrapper[data.Length];
        int i;
        for (i = 0; i < data.Length; ++i)
        {
            wrappers[i] = new TensorWrapper(data[i]);
        }

        ArraysUtils.QuickSort(wrappers, data);
        hash = data.GetHashCode();
        throw new NotImplementedException();
    }

    public Sum(Indices.Indices indices, Tensor[] data, int hash)
        : base(indices)
    {
        Debug.Assert(data.Length > 1);
        this.Data = data;
        this.hash = hash;
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }

    public override Tensor this[int i] => throw new NotImplementedException();

    public override int Size { get; }

    public override string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override TensorBuilder GetBuilder()
    {
        throw new NotImplementedException();
    }

    public override TensorFactory? GetFactory()
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
}
