namespace NRedberry.Groups;

public sealed class SchreierVector : ICloneable
{
    private int[] _data;

    public SchreierVector(int[] data)
    {
        _data = data;
    }

    public SchreierVector(int initialCapacity)
    {
        if (initialCapacity < Permutations.DefaultIdentityLength)
            initialCapacity = Permutations.DefaultIdentityLength;

        _data = new int[initialCapacity];
        Array.Fill(_data, -2);
    }

    public void EnsureCapacity(int minCapacity)
    {
        int oldCapacity = _data.Length;
        if (minCapacity > oldCapacity)
        {
            int newCapacity = (oldCapacity * 3) / 2 + 1;
            if (newCapacity < minCapacity)
                newCapacity = minCapacity;

            Array.Resize(ref _data, newCapacity);
            Array.Fill(_data, -2, oldCapacity, newCapacity - oldCapacity);
        }
    }

    public int this[int position]
    {
        get => position >= _data.Length ? -2 : _data[position];
        set
        {
            if (position >= _data.Length)
                EnsureCapacity(position + 1);
            _data[position] = value;
        }
    }

    public int Length => _data.Length;

    public void Reset() => Array.Fill(_data, -2);

    public SchreierVector Clone() => new((int[])_data.Clone());

    object ICloneable.Clone() => Clone();
}
