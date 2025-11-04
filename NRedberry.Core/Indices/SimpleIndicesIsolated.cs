namespace NRedberry.Core.Indices;

public sealed class SimpleIndicesIsolated : SimpleIndicesAbstract
{
    public SimpleIndicesIsolated(int[] data, IndicesSymmetries symmetries)
        : base(data, symmetries)
    {
    }

    public SimpleIndicesIsolated(bool notResort, int[] data, IndicesSymmetries symmetries)
        : base(notResort, data, symmetries)
    {
    }

    protected override SimpleIndices Create(int[] data, IndicesSymmetries symmetries)
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

    protected override UpperLowerIndices CalculateUpperLower()
    {
        throw new NotImplementedException();
    }

    public override int[] GetSortedData()
    {
        throw new NotImplementedException();
    }

    public override Indices GetFree()
    {
        throw new NotImplementedException();
    }

    public override Indices GetInverted()
    {
        throw new NotImplementedException();
    }

    public override Indices GetOfType(IndexType type)
    {
        throw new NotImplementedException();
    }

    public override void TestConsistentWithException()
    {
        throw new NotImplementedException();
    }

    public override Indices ApplyIndexMapping(IIndexMapping mapping)
    {
        throw new NotImplementedException();
    }

    public override short[] GetDiffIds()
    {
        throw new NotImplementedException();
    }

    public override int Size(IndexType type)
    {
        throw new NotImplementedException();
    }

    public override int this[IndexType type, int position] => throw new NotImplementedException();
}
