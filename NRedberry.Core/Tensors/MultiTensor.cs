using NRedberry.Maths;
using NRedberry.Numbers;

namespace NRedberry.Tensors;

public abstract class MultiTensor(Indices.Indices indices) : Tensor
{
    public override Indices.Indices Indices { get; } = indices;

    public Tensor Remove(Tensor tensor)
    {
        for (int l = 0, size = Size; l < size; ++l)
        {
            if (this[l] == tensor)
            {
                return Remove(l);
            }
        }

        return tensor;
    }

    public Tensor Remove(int[] positions)
    {
        if (positions.Length == 0)
        {
            return this;
        }

        int size = Size;
        foreach (int i in positions)
        {
            if (i >= size || i < 0)
            {
                throw new IndexOutOfRangeException();
            }
        }

        int[] p = MathUtils.GetSortedDistinct((int[])positions.Clone());
        if (p.Length == size)
        {
            return GetNeutral();
        }

        return Remove1(p);
    }

    protected abstract Tensor Remove1(int[] positions);
    public abstract Tensor Remove(int position);

    public Tensor Select(int[] positions)
    {
        if (positions.Length == 0)
        {
            return GetNeutral();
        }

        if (positions.Length == 1)
        {
            return this[positions[0]];
        }

        int[] p = MathUtils.GetSortedDistinct((int[])positions.Clone());
        if (p.Length == Size)
        {
            return this;
        }

        return Select1(p);
    }

    protected abstract Complex GetNeutral();
    protected abstract Tensor Select1(int[] positions);
}
