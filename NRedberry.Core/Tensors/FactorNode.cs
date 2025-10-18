namespace NRedberry.Core.Tensors;

public sealed class FactorNode
{
    public Tensor Factor { get; }
    private readonly TensorBuilder Builder;
    public int[] FactorForbiddenIndices { get; private set; }

    public FactorNode(Tensor factor, TensorBuilder builder)
    {
        Factor = factor;
        Builder = builder;
        FactorForbiddenIndices = TensorUtils.GetAllIndicesNamesT(Factor).ToArray();
    }

    private FactorNode(Tensor factor, TensorBuilder builder, int[] factorForbiddenIndices)
    {
        Factor = factor;
        Builder = builder;
        FactorForbiddenIndices = factorForbiddenIndices;
    }

    public void Put(Tensor summand, Tensor factor)
    {
        // var allowed = TensorUtils.GetAllDummyIndicesT(factor);
        // allowed.ExceptWith(FactorForbiddenIndices);
        // summand = ApplyIndexMapping.RenameDummy(summand, FactorForbiddenIndices, allowed.ToArray());
        // Builder.Put(summand);
        throw new NotImplementedException();
    }

    public void Put(Tensor t)
    {
        var t1 = ApplyIndexMapping.RenameDummy(t, FactorForbiddenIndices);
        Builder.Put(t1);
    }

    public Tensor Build()
    {
        return Builder.Build();
    }

    public FactorNode Clone()
    {
        // FactorForbiddenIndices are immutable
        return new FactorNode(Factor, Builder.Clone(), FactorForbiddenIndices);
    }
}
