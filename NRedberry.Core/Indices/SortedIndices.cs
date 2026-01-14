using NRedberry.Core.Utils;

namespace NRedberry.Indices;

public class SortedIndices : AbstractIndices
{
    private readonly int firstLower;

    private SortedIndices(int[] data, int firstLower)
        : base(data)
    {
        this.firstLower = firstLower;
    }

    public SortedIndices(int[] data)
        : base(data)
    {
        Array.Sort(Data);
        firstLower = ArraysUtils.BinarySearch1(Data, 0);
        TestConsistentWithException();
    }

    protected override UpperLowerIndices CalculateUpperLower()
    {
        int[] upper = new int[firstLower];
        int[] lower = new int[Data.Length - firstLower];
        Array.Copy(Data, 0, upper, 0, upper.Length);
        Array.Copy(Data, firstLower, lower, 0, lower.Length);
        return new UpperLowerIndices(upper, lower);
    }

    public override int Size(IndexType type)
    {
        int type_ = type.GetType_();
        int size = 0;

        int lowerPosition = Array.BinarySearch(Data, 0, firstLower, (type_ << 24) | IndicesUtils.UpperRawStateInt);
        if (lowerPosition < 0)
        {
            lowerPosition = ~lowerPosition;
        }

        int upperPosition = Array.BinarySearch(
            Data,
            lowerPosition,
            firstLower,
            ((type_ + 1) << 24) | IndicesUtils.UpperRawStateInt);
        if (upperPosition < 0)
        {
            upperPosition = ~upperPosition;
        }

        size += upperPosition - lowerPosition;

        lowerPosition = Array.BinarySearch(Data, firstLower, Data.Length - firstLower, type_ << 24);
        if (lowerPosition < 0)
        {
            lowerPosition = ~lowerPosition;
        }

        upperPosition = Array.BinarySearch(Data, lowerPosition, Data.Length - lowerPosition, (type_ + 1) << 24);
        if (upperPosition < 0)
        {
            upperPosition = ~upperPosition;
        }

        size += upperPosition - lowerPosition;
        return size;
    }

    public override int this[IndexType type, int position]
    {
        get
        {
            int type_ = type.GetType_();

            int lowerPosition = Array.BinarySearch(Data, 0, firstLower, (type_ << 24) | IndicesUtils.UpperRawStateInt);
            if (lowerPosition < 0)
            {
                lowerPosition = ~lowerPosition;
            }

            int upperPosition = Array.BinarySearch(
                Data,
                lowerPosition,
                firstLower,
                ((type_ + 1) << 24) | IndicesUtils.UpperRawStateInt);
            if (upperPosition < 0)
            {
                upperPosition = ~upperPosition;
            }

            if (lowerPosition + position < upperPosition)
            {
                return Data[lowerPosition + position];
            }

            position -= upperPosition - lowerPosition;

            lowerPosition = Array.BinarySearch(Data, firstLower, Data.Length - firstLower, type_ << 24);
            if (lowerPosition < 0)
            {
                lowerPosition = ~lowerPosition;
            }

            upperPosition = Array.BinarySearch(Data, lowerPosition, Data.Length - lowerPosition, (type_ + 1) << 24);
            if (upperPosition < 0)
            {
                upperPosition = ~upperPosition;
            }

            if (lowerPosition + position < upperPosition)
            {
                return Data[lowerPosition + position];
            }

            throw new IndexOutOfRangeException();
        }
    }

    public override Indices GetFree()
    {
        List<int> list = [];
        int u;
        int l;
        int iLower = firstLower;
        int iUpper = 0;
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

        list.AddRange(Data.Skip(iUpper).Take(firstLower - iUpper));
        list.AddRange(Data.Skip(iLower).Take(Data.Length - iLower));
        return IndicesFactory.Create(list.ToArray());
    }

    public override Indices GetOfType(IndexType type)
    {
        int type_ = type.GetType_();

        int lowerPositionU = Array.BinarySearch(Data, 0, firstLower, (type_ << 24) | IndicesUtils.UpperRawStateInt);
        if (lowerPositionU < 0)
        {
            lowerPositionU = ~lowerPositionU;
        }

        int upperPositionU = Array.BinarySearch(
            Data,
            lowerPositionU,
            firstLower,
            ((type_ + 1) << 24) | IndicesUtils.UpperRawStateInt);
        if (upperPositionU < 0)
        {
            upperPositionU = ~upperPositionU;
        }

        int sizeU = upperPositionU - lowerPositionU;

        int lowerPositionL = Array.BinarySearch(Data, firstLower, Data.Length - firstLower, type_ << 24);
        if (lowerPositionL < 0)
        {
            lowerPositionL = ~lowerPositionL;
        }

        int upperPositionL = Array.BinarySearch(Data, lowerPositionL, Data.Length - lowerPositionL, (type_ + 1) << 24);
        if (upperPositionL < 0)
        {
            upperPositionL = ~upperPositionL;
        }

        int sizeL = upperPositionL - lowerPositionL;
        int total = sizeU + sizeL;
        if (total == Data.Length)
        {
            return this;
        }

        if (total == 0)
        {
            return IndicesFactory.EmptyIndices;
        }

        int[] indices = new int[total];
        Array.Copy(Data, lowerPositionU, indices, 0, sizeU);
        Array.Copy(Data, lowerPositionL, indices, sizeU, sizeL);
        return new SortedIndices(indices, sizeU);
    }

    public override int[] GetSortedData()
    {
        return Data;
    }

    public override Indices GetInverted()
    {
        int[] dataInv = new int[Data.Length];
        int fl = Data.Length - firstLower;
        int i = 0;
        for (; i < firstLower; ++i)
        {
            dataInv[fl + i] = Data[i] ^ IndicesUtils.UpperRawStateInt;
        }

        for (; i < Data.Length; ++i)
        {
            dataInv[i - firstLower] = Data[i] ^ IndicesUtils.UpperRawStateInt;
        }

        return new SortedIndices(dataInv, fl);
    }

    public override void TestConsistentWithException()
    {
        int i = 0;
        for (; i < firstLower - 1; ++i)
        {
            if (Data[i] == Data[i + 1])
            {
                throw new InconsistentIndicesException(Data[i]);
            }
        }

        for (i = firstLower; i < Data.Length - 1; ++i)
        {
            if (Data[i] == Data[i + 1])
            {
                throw new InconsistentIndicesException(Data[i]);
            }
        }
    }

    public override Indices ApplyIndexMapping(IIndexMapping mapping)
    {
        bool changed = false;
        int[] dataCopy = (int[])Data.Clone();
        for (int i = 0; i < Data.Length; ++i)
        {
            int newIndex = mapping.Map(dataCopy[i]);
            if (dataCopy[i] != newIndex)
            {
                dataCopy[i] = newIndex;
                changed = true;
            }
        }

        return changed ? new SortedIndices(dataCopy) : this;
    }

    public override short[] GetDiffIds()
    {
        return new short[Data.Length];
    }
}
