namespace NRedberry.Indices;

internal sealed class EmptySimpleIndices : EmptyIndices, SimpleIndices
{
    public static readonly EmptySimpleIndices emptySimpleIndicesInstance = new();

    private EmptySimpleIndices()
    {
    }

    public SimpleIndices Inverted => this;
    public SimpleIndices Free => this;

    public SimpleIndices OfType(IndexType type) => this;

    public SimpleIndices Upper => this;
    public SimpleIndices Lower => this;

    public IndicesSymmetries Symmetries
    {
        get => IndicesSymmetries.EmptySymmetries;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (value.StructureOfIndices.Size != 0)
            {
                throw new InvalidOperationException("Symmetries dimensions are not equal to indices size.");
            }
        }
    }

    public new SimpleIndices ApplyIndexMapping(IIndexMapping mapping) => this;

    public override bool Equals(object? obj) => obj is EmptyIndices;

    public bool EqualsWithSymmetries(SimpleIndices indices) => indices == emptySimpleIndicesInstance;

    public override int GetHashCode() => 453679;

    public StructureOfIndices StructureOfIndices => StructureOfIndices.Empty;
}
