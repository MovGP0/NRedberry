using System;

namespace NRedberry.Core.Indices;

public sealed class SimpleIndicesIsolated : SimpleIndicesAbstract
{
    public SimpleIndicesIsolated(int[] data, IndicesSymmetries symmetries) : base(data, symmetries) { }

    public SimpleIndicesIsolated(bool notResort, int[] data, IndicesSymmetries symmetries) : base(notResort, data, symmetries) { }

    protected /*override*/ SimpleIndices Create(int[] data, IndicesSymmetries symmetries)
    {
        return new SimpleIndicesIsolated(true, data, symmetries == null ? null : symmetries.Clone());
    }

    public /*override*/ IndicesSymmetries Symmetries
    {
        get
        {
            if (symmetries == null)
                symmetries = new IndicesSymmetries(new StructureOfIndices(this));
            return symmetries;
        }
        set
        {
            if (!value.StructureOfIndices.IsStructureOf(this))
                throw new ArgumentException("Illegal symmetries instance.");
            symmetries = value;
        }
    }
}