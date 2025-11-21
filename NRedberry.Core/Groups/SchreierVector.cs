namespace NRedberry.Groups;

public sealed class SchreierVector : ICloneable
{
    private int[] data;

    public SchreierVector(int[] data)
    {
        this.data = (int[])data.Clone();
    }

    public SchreierVector(int initialCapacity)
    {
        if (initialCapacity < Permutations.DefaultIdentityLength)
            initialCapacity = Permutations.DefaultIdentityLength;

        data = new int[initialCapacity];
        Array.Fill(data, -2);
    }

    public void EnsureCapacity(int minCapacity)
    {
        int oldCapacity = data.Length;
        if (minCapacity > oldCapacity)
        {
            int newCapacity = (oldCapacity * 3) / 2 + 1;
            if (newCapacity < minCapacity)
                newCapacity = minCapacity;

            Array.Resize(ref data, newCapacity);
            Array.Fill(data, -2, oldCapacity, newCapacity - oldCapacity);
        }
    }

    public int this[int position]
    {
        get => position >= data.Length ? -2 : data[position];
        set
        {
            if (position >= data.Length)
                EnsureCapacity(position + 1);
            data[position] = value;
        }
    }

    public int Length => data.Length;

    public void Reset() => Array.Fill(data, -2);

    public SchreierVector Clone() => new((int[])data.Clone());

    object ICloneable.Clone() => Clone();
}
