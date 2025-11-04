using NRedberry.Core.Numbers;

namespace NRedberry.Core.Tensors;

public abstract class AbstractSumBuilder : TensorBuilder
{
    protected readonly Dictionary<int, List<FactorNode>> Summands;
    protected Complex Complex = Complex.Zero;
    protected Indices.Indices Indices;
    protected int[] SortedNames;
    private int size;

    protected AbstractSumBuilder()
        : this(7)
    {
    }

    protected AbstractSumBuilder(int initialCapacity)
    {
        Summands = new Dictionary<int, List<FactorNode>>(initialCapacity);
    }

    protected AbstractSumBuilder(Dictionary<int, List<FactorNode>> summands, Complex complex, Indices.Indices indices, int[] sortedNames)
    {
        Summands = summands;
        Complex = complex;
        Indices = indices;
        SortedNames = sortedNames;
    }

    public abstract Tensor Build();

    protected abstract Split Split(Tensor tensor);

    public abstract void Put(Tensor tensor);

    public int Size()
    {
        return size + (Complex.IsZero() ? 0 : 1);
    }

    public int SizeOfMap()
    {
        return Summands.Count;
    }

    public static bool DEBUG_PRINT_SAME_FLAG;

    public abstract TensorBuilder Clone();
}
