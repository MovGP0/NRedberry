using NRedberry.Numbers;

namespace NRedberry.Tensors;

public sealed class SumBuilder : AbstractSumBuilder
{
    public SumBuilder(int initialCapacity)
        : base(initialCapacity)
    {
    }

    public SumBuilder()
    {
    }

    public SumBuilder(
        Dictionary<int, List<FactorNode>> summands,
        Complex complex,
        Indices.Indices indices,
        int[] sortedFreeIndices)
        : base(summands, complex, indices, sortedFreeIndices)
    {
    }

    public override void Put(Tensor tensor)
    {
        throw new System.NotImplementedException();
    }

    public override Tensor Build()
    {
        throw new System.NotImplementedException();
    }

    protected override Split Split(Tensor tensor)
    {
        return NRedberry.Tensors.Split.SplitIndexless(tensor);
    }

    public override TensorBuilder Clone()
    {
        var summands = new Dictionary<int, List<FactorNode>>(Summands);
        foreach (var pair in summands)
        {
            var vals = pair.Value;
            for (int i = vals.Count - 1; i >= 0; --i)
            {
                vals[i] = (FactorNode)vals[i].Clone();
            }
        }

        return new SumBuilder(summands, Complex, Indices, (int[])SortedNames.Clone());
    }
}
