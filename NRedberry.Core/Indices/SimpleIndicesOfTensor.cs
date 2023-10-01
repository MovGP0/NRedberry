using System;

namespace NRedberry.Core.Indices;

public sealed class SimpleIndicesOfTensor : SimpleIndicesAbstract
{
    public SimpleIndicesOfTensor(int[] data, IndicesSymmetries symmetries)
        : base(data, symmetries)
    {
    }

    public SimpleIndicesOfTensor(bool notResort, int[] data, IndicesSymmetries symmetries)
        : base(notResort, data, symmetries)
    {
    }

    public SimpleIndices Create(int[] data, IndicesSymmetries symmetries)
    {
        return new SimpleIndicesOfTensor(true, data, symmetries);
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