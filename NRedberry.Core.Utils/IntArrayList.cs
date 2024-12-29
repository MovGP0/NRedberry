using System.Collections;
using System.Text;

namespace NRedberry.Core.Utils;

public sealed class IntArrayList : IList<int>
{
    internal int[] Data => data;

    private int[] data;
    private int size;

    public IntArrayList()
    {
        data = new int[10];
    }

    public IntArrayList(int initialCapacity)
    {
        if (initialCapacity < 0)
            throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Capacity must be non-negative.");
        data = new int[initialCapacity];
    }

    public IntArrayList(IntArrayList list)
    {
        data = (int[])list.data.Clone();
        size = list.size;
    }

    public IntArrayList(int[] data)
    {
        this.data = (int[])data.Clone();
        size = data.Length;
    }

    private IntArrayList(int[] data, int size)
    {
        this.data = data;
        this.size = size;
    }

    public int Count => size;
    public bool IsReadOnly => false;

    public int this[int index]
    {
        get
        {
            if (index < 0 || index >= size)
                throw new ArgumentOutOfRangeException(nameof(index));
            return data[index];
        }
        set
        {
            if (index < 0 || index >= size)
                throw new ArgumentOutOfRangeException(nameof(index));
            data[index] = value;
        }
    }

    public void Add(int item)
    {
        EnsureCapacity(size + 1);
        data[size++] = item;
    }

    public void Clear()
    {
        size = 0;
    }

    public bool Contains(int item)
    {
        return IndexOf(item) >= 0;
    }

    public void CopyTo(int[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0 || arrayIndex > array.Length)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (array.Length - arrayIndex < size)
            throw new ArgumentException("Destination array is not large enough.");
        Array.Copy(data, 0, array, arrayIndex, size);
    }

    public IEnumerator<int> GetEnumerator()
    {
        for (int i = 0; i < size; i++)
        {
            yield return data[i];
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int IndexOf(int item)
    {
        for (int i = 0; i < size; i++)
        {
            if (data[i] == item)
                return i;
        }
        return -1;
    }

    public void Insert(int index, int item)
    {
        if (index < 0 || index > size)
            throw new ArgumentOutOfRangeException(nameof(index));
        EnsureCapacity(size + 1);
        Array.Copy(data, index, data, index + 1, size - index);
        data[index] = item;
        size++;
    }

    public bool Remove(int item)
    {
        int index = IndexOf(item);
        if (index < 0) return false;
        RemoveAt(index);
        return true;
    }

    public void RemoveAt(int index)
    {
        if (index < 0 || index >= size)
            throw new ArgumentOutOfRangeException(nameof(index));
        int numMoved = size - index - 1;
        if (numMoved > 0)
            Array.Copy(data, index + 1, data, index, numMoved);
        size--;
    }

    private void EnsureCapacity(int minCapacity)
    {
        int oldCapacity = data.Length;
        if (minCapacity > oldCapacity)
        {
            int newCapacity = (oldCapacity * 3) / 2 + 1;
            if (newCapacity < minCapacity)
                newCapacity = minCapacity;
            Array.Resize(ref data, newCapacity);
        }
    }

    public int[] ToArray()
    {
        int[] result = new int[size];
        Array.Copy(data, 0, result, 0, size);
        return result;
    }

    public IntArrayList Clone()
    {
        return new IntArrayList((int[])data.Clone(), size);
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;
        IntArrayList other = (IntArrayList)obj;
        if (size != other.size)
            return false;
        for (int i = 0; i < size; i++)
        {
            if (data[i] != other.data[i])
                return false;
        }
        return true;
    }

    public override int GetHashCode()
    {
        int hash = 5;
        for (int i = 0; i < size; i++)
            hash = 31 * hash + data[i];
        return hash;
    }

    public override string ToString()
    {
        if (size == 0)
            return "[]";
        StringBuilder builder = new StringBuilder();
        builder.Append('[');
        for (int i = 0; i < size; i++)
        {
            builder.Append(data[i]);
            if (i < size - 1)
                builder.Append(", ");
        }
        builder.Append(']');
        return builder.ToString();
    }

    public void RemoveAfter(int point)
    {
        if (point < 0 || point > size)
            throw new IndexOutOfRangeException(nameof(point));
        size = point;
    }
}
