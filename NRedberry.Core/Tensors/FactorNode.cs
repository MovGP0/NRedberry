namespace NRedberry.Tensors;

public sealed class FactorNode
{
    public Tensor Factor { get; }

    private readonly TensorBuilder _builder;

    public int[] FactorForbiddenIndices { get; }

    public FactorNode(Tensor factor, TensorBuilder builder)
    {
        ArgumentNullException.ThrowIfNull(factor);
        ArgumentNullException.ThrowIfNull(builder);

        Factor = ApplyIndexMapping.OptimizeDummies(factor);
        _builder = builder;
        FactorForbiddenIndices = TensorUtils.GetAllIndicesNamesT(Factor).ToArray();
    }

    private FactorNode(Tensor factor, TensorBuilder builder, int[] factorForbiddenIndices)
    {
        Factor = factor;
        _builder = builder;
        FactorForbiddenIndices = factorForbiddenIndices;
    }

    public void Put(Tensor summand, Tensor factor)
    {
        var allowed = TensorUtils.GetAllDummyIndicesT(factor);
        allowed.ExceptWith(FactorForbiddenIndices);
        summand = ApplyIndexMapping.RenameDummy(summand, FactorForbiddenIndices, allowed.ToArray());
        _builder.Put(summand);
    }

    public void Put(Tensor t)
    {
        var t1 = ApplyIndexMapping.RenameDummy(t, FactorForbiddenIndices);
        _builder.Put(t1);
    }

    public Tensor Build()
    {
        return _builder.Build();
    }

    public FactorNode Clone()
    {
        // FactorForbiddenIndices are immutable
        return new FactorNode(Factor, _builder.Clone(), FactorForbiddenIndices);
    }
}
