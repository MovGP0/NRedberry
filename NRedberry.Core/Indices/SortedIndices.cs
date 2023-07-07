using System;
using System.Collections.Generic;
using System.Linq;

namespace NRedberry.Core.Indices;

public class SortedIndices : AbstractIndices
{
    private readonly int firstLower;

    private SortedIndices(long[] data, int firstLower) : base(data)
    {
        this.firstLower = firstLower;
    }

    public SortedIndices(long[] data) : base(data)
    {
        Array.Sort(this.Data);
        firstLower = Array.BinarySearch(this.Data, 0);
        TestConsistentWithException();
    }

    protected override UpperLowerIndices CalculateUpperLower()
    {
        long[] upper = Data.Take((int)firstLower).ToArray();
        long[] lower = Data.Skip((int)firstLower).ToArray();
        return new UpperLowerIndices(upper, lower);
    }

    public override int Size(IndexType type)
    {
        long type_ = type.GetType_();
        int size = 0;

        int lowerPosition = Array.BinarySearch(Data, 0, (int)firstLower, (type_ << 24) | 0x80000000);
        if (lowerPosition < 0) lowerPosition = ~lowerPosition;
        int upperPosition = Array.BinarySearch(Data, lowerPosition, (int)firstLower, ((type_ + 1) << 24) | 0x80000000);
        if (upperPosition < 0) upperPosition = ~upperPosition;
        size += upperPosition - lowerPosition;

        lowerPosition = Array.BinarySearch(Data, firstLower, Data.Length, type_ << 24);
        if (lowerPosition < 0) lowerPosition = ~lowerPosition;
        upperPosition = Array.BinarySearch(Data, lowerPosition, Data.Length, (type_ + 1) << 24);
        if (upperPosition < 0) upperPosition = ~upperPosition;
        size += upperPosition - lowerPosition;
        return size;
    }

    public override long this[IndexType type, long position] => throw new NotImplementedException();

    public override IIndices GetFree()
    {
        List<long> list = new List<long>();
        long u, l;
        long iLower = firstLower, iUpper = 0;
        for (; iUpper < firstLower && iLower < Data.Length; ++iLower, ++iUpper)
        {
            u = Data[iUpper] & 0x7FFFFFFF; //taking name with type
            l = Data[iLower];
            if (u < l)
            {
                list.Add(Data[iUpper]);
                --iLower;
            }
            else if (l < u)
            {
                list.Add(l);
                --iUpper;
            }
        }
        list.AddRange(Data.Skip((int)iUpper).Take((int)(firstLower - iUpper)));
        list.AddRange(Data.Skip((int)iLower).Take((int)(Data.Length - iLower)));
        return IndicesFactory.Create(list.ToArray());
    }

    public override IIndices GetOfType(IndexType type)
    {
        // TODO: implement this
        throw new NotImplementedException();
    }

    public override long[] GetSortedData()
    {
        return Data;
    }

    public override IIndices GetInverted()
    {
        // TODO: implement this
        throw new NotImplementedException();
    }

    public override void TestConsistentWithException()
    {
        // TODO: implement this
        throw new NotImplementedException();
    }

    public override IIndices ApplyIndexMapping(IIndexMapping mapping)
    {
        // TODO: implement this
        throw new NotImplementedException();
    }

    public override short[] GetDiffIds()
    {
        return new short[Data.Length];
    }
}
