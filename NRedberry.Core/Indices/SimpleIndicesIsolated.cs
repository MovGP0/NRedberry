namespace NRedberry.Indices;

public sealed class SimpleIndicesIsolated : SimpleIndicesAbstract
{
    public SimpleIndicesIsolated(int[] data, IndicesSymmetries? symmetries)
        : base(data, symmetries!)
    {
    }

    public SimpleIndicesIsolated(bool notResort, int[] data, IndicesSymmetries? symmetries)
        : base(notResort, data, symmetries)
    {
    }

    protected override SimpleIndices Create(int[] data, IndicesSymmetries? symmetries)
    {
        return new SimpleIndicesIsolated(true, data, symmetries is null ? null : symmetries.Clone());
    }

    public new IndicesSymmetries Symmetries
    {
        get
        {
            if (symmetries is null)
            {
                symmetries = IndicesSymmetries.Create(new StructureOfIndices(this));
            }

            return symmetries;
        }
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            if (!value.StructureOfIndices.IsStructureOf(this))
            {
                throw new ArgumentException("Illegal symmetries instance.");
            }

            symmetries = value;
        }
    }
}
