using System.Text;

namespace NRedberry.Core.Utils;

public sealed class IntArrayList
{
    internal int[] Data;
    private int _size = 0;

    public IntArrayList()
    {
        Data = new int[10];
    }

    public IntArrayList(int initialCapacity)
    {
        Data = new int[initialCapacity];
    }

    public IntArrayList(IntArrayList list)
    {
        Data = (int[])list.Data.Clone();
        _size = list._size;
    }

    public IntArrayList(int[] data)
    {
        Data = data;
        _size = data.Length;
    }

    private IntArrayList(int[] data, int size)
    {
        Data = data;
        _size = size;
    }

    public void Clear()
    {
        _size = 0;
    }

    public void EnsureCapacity(int minCapacity)
    {
        int oldCapacity = Data.Length;
        if (minCapacity > oldCapacity)
        {
            int newCapacity = (oldCapacity * 3) / 2 + 1;
            if (newCapacity < minCapacity)
                newCapacity = minCapacity;
            Array.Resize(ref Data, newCapacity);
        }
    }

    public void Add(int num)
    {
        EnsureCapacity(_size + 1);
        Data[_size++] = num;
    }

    public void Add(int position, int num)
    {
        if (position < 0 || position >= _size)
            throw new IndexOutOfRangeException();
        EnsureCapacity(_size + 1);
        Array.Copy(Data, position, Data, position + 1, _size - position);
        Data[position] = num;
        _size++;
    }

    public void AddAll(params int[] arr)
    {
        int arrLen = arr.Length;
        EnsureCapacity(arrLen + _size);
        Array.Copy(arr, 0, Data, _size, arrLen);
        _size += arrLen;
    }

    public void AddAll(IntArray intArray)
    {
        AddAll(intArray.InnerArray);
    }

    public void AddAll(IntArrayList intArrayList)
    {
        int arrLen = intArrayList.Size;
        EnsureCapacity(arrLen + _size);
        Array.Copy(intArrayList.Data, 0, Data, _size, arrLen);
        _size += arrLen;
    }

    public void Set(int position, int num)
    {
        if (position < 0 || position >= _size)
            throw new IndexOutOfRangeException();
        Data[position] = num;
    }

    public void Sort()
    {
        Array.Sort(Data, 0, _size);
    }

    public void Sort(IntArrayList cosort)
    {
        if (_size != cosort._size)
            throw new ArgumentException();
        ArraysUtils.QuickSort(Data, 0, _size, cosort.Data);
    }

    public void Push(int value)
    {
        Add(value);
    }

    public int Peek()
    {
        return Data[_size - 1];
    }

    public int Pop()
    {
        return Data[--_size];
    }

    public void Add(int[] src, int fromIndex, int length)
    {
        EnsureCapacity(_size + length);
        Array.Copy(src, fromIndex, Data, _size, length);
        _size += length;
    }

    public int Get(int i)
    {
        if (i < 0 || i >= _size)
            throw new IndexOutOfRangeException();
        return Data[i];
    }

    public bool ReplaceFirst(int from, int to)
    {
        for (int i = 0; i < _size; ++i)
        {
            if (Data[i] == from)
            {
                Data[i] = to;
                return true;
            }
        }
        return false;
    }

    public bool ReplaceAll(int from, int to)
    {
        bool replaced = false;
        for (int i = 0; i < _size; ++i)
        {
            if (Data[i] == from)
            {
                Data[i] = to;
                replaced = true;
            }
        }
        return replaced;
    }

    public int First()
    {
        return Get(0);
    }

    public int Last()
    {
        return Get(_size - 1);
    }

    public int[] ToArray()
    {
        int[] result = new int[_size];
        Array.Copy(Data, 0, result, 0, _size);
        return result;
    }

    public int Size => _size;

    public bool Contains(int value)
    {
        return IndexOf(value) >= 0;
    }

    public void RemoveAfter(int point)
    {
        if (point < 0)
            throw new IndexOutOfRangeException();
        _size = point;
    }

    public int Remove(int index)
    {
        int oldValue = Data[index];
        int numMoved = _size - index - 1;
        if (numMoved > 0)
            Array.Copy(Data, index + 1, Data, index, numMoved);
        --_size;
        return oldValue;
    }

    public bool RemoveElement(int element)
    {
        int i = IndexOf(element);
        if (i < 0) return false;
        Remove(i);
        return true;
    }

    public bool RemoveAll(IntArrayList c)
    {
        return RemoveAll(c, 0, c._size);
    }

    public bool RemoveAll(IntArrayList c, int cbegin, int cend)
    {
        return RemoveAll(c.Data, cbegin, cend);
    }

    public bool RemoveAll(int[] c)
    {
        return RemoveAll(c, 0, c.Length);
    }

    public bool RemoveAll(int[] c, int cbegin, int cend)
    {
        int r = 0, w = 0;
        bool modified = false;
        for (; r < _size; r++)
        {
            modified = false;
            for (int i = cbegin; i < cend; ++i)
            {
                if (c[i] == Data[r])
                {
                    modified = true;
                    break;
                }
            }
            if (!modified)
                Data[w++] = Data[r];
        }

        if (w != _size)
        {
            _size = w;
            modified = true;
        }

        return modified;
    }

    public void SumWith(int value)
    {
        for (int i = 0; i < _size; ++i)
            Data[i] += value;
    }

    public int IndexOf(int value)
    {
        for (int i = 0; i < _size; ++i)
            if (Data[i] == value)
                return i;
        return -1;
    }

    public bool IsEmpty()
    {
        return _size == 0;
    }

    public IntArrayList Clone()
    {
        return new IntArrayList((int[])Data.Clone(), _size);
    }

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        if (GetType() != obj.GetType())
            return false;
        IntArrayList other = (IntArrayList)obj;
        if (_size != other._size)
            return false;
        for (int i = 0; i < _size; ++i)
        {
            if (Data[i] != other.Data[i])
                return false;
        }
        return true;
    }

    public override int GetHashCode()
    {
        int hash = 5;
        for (int i = 0; i < _size; ++i)
            hash = 31 * hash + Data[i];
        return hash;
    }

    public override string ToString()
    {
        int iMax = Size - 1;
        if (iMax == -1)
            return "[]";
        StringBuilder b = new StringBuilder();
        b.Append('[');
        for (int i = 0; ; i++)
        {
            b.Append(Data[i]);
            if (i == iMax)
                return b.Append(']').ToString();
            b.Append(", ");
        }
    }
}