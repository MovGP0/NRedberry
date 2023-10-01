namespace NRedberry.Core.Tensors;

public sealed class FactorNode
{
    public Tensor Factor { get; }
    private readonly TensorBuilder builder;
    public int[] FactorForbiddenIndices { get; private set; }

    public FactorNode(Tensor factor, TensorBuilder builder)
    {
        this.Factor = ApplyIndexMapping.OptimizeDummies(factor);
        this.builder = builder;
        this.FactorForbiddenIndices = TensorUtils.GetAllIndicesNamesT(this.Factor).ToArray();
    }

    private FactorNode(Tensor factor, TensorBuilder builder, int[] factorForbiddenIndices)
    {
        this.Factor = factor;
        this.builder = builder;
        this.FactorForbiddenIndices = factorForbiddenIndices;
    }

    public void Put(Tensor summand, Tensor factor)
    {
        var allowed = TensorUtils.GetAllDummyIndicesT(factor);
        allowed.ExceptWith(FactorForbiddenIndices);
        summand = ApplyIndexMapping.RenameDummy(summand, FactorForbiddenIndices, allowed.ToArray());
        builder.Put(summand);
    }

    public Tensor Build()
    {
        return builder.Build();
    }

    public FactorNode Clone()
    {
        // FactorForbiddenIndices are immutable
        return new FactorNode(Factor, builder.Clone(), FactorForbiddenIndices);
    }
}
