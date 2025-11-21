namespace NRedberry.Tensors;

public sealed class StructureOfContractionsHashed(
    TensorContraction freeContraction,
    params TensorContraction[] contractions)
{
    public static StructureOfContractionsHashed EmptyInstance = new(new TensorContraction(-1, []));

    public TensorContraction FreeContraction { get; } = freeContraction;
    private TensorContraction[] Contractions { get; } = contractions;

    public TensorContraction this[int i] => Contractions[i];
}
