using NRedberry.Indices;

namespace NRedberry.Contexts;

public abstract class NameDescriptorForTensorField(
    StructureOfIndices[] indexTypeStructures,
    int id,
    int[] orders,
    string name,
    bool isDiracDelta)
    : NameDescriptor(indexTypeStructures, id)
{
    public int[] Orders { get; } = orders;
    public string Name { get; } = name;

    private int[][]? _indicesPartitionMapping;

    public bool IsDiracDelta { get; } = isDiracDelta;

    public int[] GetDerivativeOrders()
    {
        return (int[])Orders.Clone();
    }

    public int GetDerivativeOrder(int arg)
    {
        return Orders[arg];
    }

    private void EnsurePartitionInitialized()
    {
        if (_indicesPartitionMapping != null)
            return;

        if (!IsDerivative())
        {
            int[][] ret = new int[IndexTypeStructures.Length][];
            Array.Fill(ret, [], 1, ret.Length - 1);
            ret[0] = Enumerable.Range(0, IndexTypeStructures[0].Size).ToArray();
            _indicesPartitionMapping = ret;
        }

        var parent = GetParent();

        var partition = new StructureOfIndices[Orders.Sum() + 1];
        partition[0] = parent.GetStructureOfIndices();
        int i;
        int j;
        int totalOrder = 1;
        for (i = 0; i < IndexTypeStructures.Length - 1; ++i)
        {
            for (j = Orders[i] - 1; j >= 0; --j)
                partition[totalOrder++] = parent.GetArgStructuresOfIndices(i);
        }

        _indicesPartitionMapping = IndexTypeStructures[0].GetPartitionMappings(partition);
    }

    public int[][] GetIndicesPartitionMapping()
    {
        EnsurePartitionInitialized();
        return _indicesPartitionMapping.Select(a => (int[])a.Clone()).ToArray(); // Deep clone
    }

    public abstract NameDescriptorForTensorField GetParent();

    public abstract bool IsDerivative();

    public abstract NameDescriptorForTensorField GetDerivative(params int[] orders);
}
