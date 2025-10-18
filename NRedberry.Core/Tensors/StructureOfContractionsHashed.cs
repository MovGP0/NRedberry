namespace NRedberry.Core.Tensors;

public sealed class StructureOfContractionsHashed
{
    public static StructureOfContractionsHashed EmptyInstance = new(new TensorContraction(-1, []));

    public TensorContraction FreeContraction { get; }
    private TensorContraction[] Contractions { get; }

    public StructureOfContractionsHashed(TensorContraction freeContraction, params TensorContraction[] contractions)
    {
        FreeContraction = freeContraction;
        Contractions = contractions;
    }

    public TensorContraction this[int i] => Contractions[i];
}