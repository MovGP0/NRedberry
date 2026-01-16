using NRedberry.Numbers;

namespace NRedberry.Tensors;

/*
 * Original: ./core/src/main/java/cc/redberry/core/tensor/SumBuilderSplitingScalars.java
 */

public sealed class SumBuilderSplitingScalars : AbstractSumBuilder
{
    internal SumBuilderSplitingScalars(
        Dictionary<int, List<FactorNode>> summands,
        Complex complex,
        Indices.Indices? indices,
        int[]? sortedFreeIndices)
        : base(summands, complex, indices, sortedFreeIndices)
    {
    }

    public SumBuilderSplitingScalars()
    {
    }

    public SumBuilderSplitingScalars(int initialCapacity)
        : base(initialCapacity)
    {
    }

    protected override Split Split(Tensor tensor)
    {
        return NRedberry.Tensors.Split.SplitScalars(tensor);
    }

    public override TensorBuilder Clone()
    {
        var summands = new Dictionary<int, List<FactorNode>>(Summands.Count);
        foreach (var pair in Summands)
        {
            var vals = pair.Value;
            var cloned = new List<FactorNode>(vals.Count);
            for (int i = 0; i < vals.Count; i++)
            {
                cloned.Add(vals[i].Clone());
            }

            summands[pair.Key] = cloned;
        }

        int[]? sortedNames = SortedNames is null ? null : (int[])SortedNames.Clone();
        return new SumBuilderSplitingScalars(summands, Complex, Indices, sortedNames);
    }
}
