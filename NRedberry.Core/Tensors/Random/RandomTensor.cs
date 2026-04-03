using System.Collections;
using NRedberry.Contexts;
using NRedberry.Groups;
using NRedberry.IndexGeneration;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using IndicesType = NRedberry.Indices.Indices;

namespace NRedberry.Tensors.Random;

public sealed class RandomTensor
{
    private static readonly byte[] s_types = IndexTypeMethods.GetBytes();

    private System.Random _random;
    private readonly int[] _minIndices;
    private readonly int[] _maxIndices;
    private readonly bool _withSymmetries;
    private readonly bool _generateNewDescriptors;
    private readonly List<NameDescriptor> _namespace;
    private readonly int _initialNamespaceSize;
    private long _seed;
    private int _nextNameIndex;

    public RandomTensor()
        : this(true)
    {
    }

    public RandomTensor(bool generateNewDescriptors)
        : this(
            2,
            5,
            [0, 0, 0, 0],
            [4, 4, 4, 4],
            true,
            generateNewDescriptors)
    {
    }

    public RandomTensor(
        int minDifferentDescriptors,
        int maxDifferentDescriptors,
        int[] minIndices,
        int[] maxIndices,
        bool withSymmetries,
        bool generateNewDescriptors)
        : this(
            minDifferentDescriptors,
            maxDifferentDescriptors,
            minIndices,
            maxIndices,
            withSymmetries,
            generateNewDescriptors,
            CreateSeededRandom(out long seed),
            seed)
    {
    }

    public RandomTensor(
        int minDifferentDescriptors,
        int maxDifferentDescriptors,
        int[] minIndices,
        int[] maxIndices,
        bool withSymmetries,
        bool generateNewDescriptors,
        long seed)
        : this(
            minDifferentDescriptors,
            maxDifferentDescriptors,
            minIndices,
            maxIndices,
            withSymmetries,
            generateNewDescriptors,
            new System.Random(unchecked((int)seed)),
            seed)
    {
    }

    public RandomTensor(
        int minDifferentDescriptors,
        int maxDifferentDescriptors,
        int[] minIndices,
        int[] maxIndices,
        bool withSymmetries,
        bool generateNewDescriptors,
        System.Random random)
        : this(
            minDifferentDescriptors,
            maxDifferentDescriptors,
            minIndices,
            maxIndices,
            withSymmetries,
            generateNewDescriptors,
            random,
            random.NextInt64())
    {
    }

    public enum TensorType
    {
        Product,
        Sum
    }

    public sealed class Parameters(int minSumSize, int maxSumSize, int minProductSize, int maxProductSize)
    {
        public int MinSumSize { get; } = minSumSize;
        public int MaxSumSize { get; } = maxSumSize;
        public int MinProductSize { get; } = minProductSize;
        public int MaxProductSize { get; } = maxProductSize;
    }

    public System.Random GetRandom()
    {
        return _random;
    }

    public long GetSeed()
    {
        return _seed;
    }

    public int GetInitialNamespaceSize()
    {
        return _initialNamespaceSize;
    }

    public int GetNamespaceSize()
    {
        return _namespace.Count;
    }

    public void ClearNamespace()
    {
        _namespace.Clear();
    }

    public void Reset()
    {
        Reset(_random.NextInt64());
    }

    public void Reset(long seed)
    {
        _seed = seed;
        _random = new System.Random(unchecked((int)seed));
        _nextNameIndex = 0;
        _namespace.Clear();
        GenerateDescriptors();
    }

    public int NextInt(int n)
    {
        return n == 0 ? 0 : _random.Next(n);
    }

    public void AddToNamespace(params Tensor[] tensors)
    {
        ArgumentNullException.ThrowIfNull(tensors);

        foreach (SimpleTensor tensor in TensorUtils.GetAllDiffSimpleTensors(tensors))
        {
            NameDescriptor descriptor = tensor.GetNameDescriptor();
            if (!_namespace.Contains(descriptor))
            {
                _namespace.Add(descriptor);
            }
        }
    }

    public NameDescriptor NextNameDescriptor()
    {
        if (_namespace.Count == 0)
        {
            throw new InvalidOperationException("The random tensor namespace is empty.");
        }

        return _namespace[NextInt(_namespace.Count)];
    }

    public SimpleTensor NextSimpleTensor(SimpleIndices indices)
    {
        ArgumentNullException.ThrowIfNull(indices);

        NameDescriptor descriptor = NextNameDescriptor(indices.StructureOfIndices);
        return new SimpleTensor(descriptor.Id, IndicesFactory.CreateSimple(descriptor.GetSymmetries(), indices));
    }

    public SimpleTensor NextSimpleTensor()
    {
        NameDescriptor descriptor = NextNameDescriptor();
        int[] indices = NextIndices(descriptor.GetStructureOfIndices());
        return new SimpleTensor(
            descriptor.Id,
            IndicesFactory.CreateSimple(descriptor.GetSymmetries(), indices));
    }

    public Tensor NextProduct(int minProductSize, IndicesType indices)
    {
        ArgumentNullException.ThrowIfNull(indices);
        if (minProductSize < 2)
        {
            throw new ArgumentException("Product size must be at least 2.", nameof(minProductSize));
        }

        return NextProductTree(1, new Parameters(0, 0, minProductSize, minProductSize), indices);
    }

    public Tensor NextProduct(int minProductSize)
    {
        NameDescriptor descriptor = NextNameDescriptor();
        return NextProduct(
            minProductSize,
            IndicesFactory.CreateSimple(null, NextIndices(descriptor.GetStructureOfIndices())));
    }

    public Tensor NextSum(Parameters parameters, IndicesType indices)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        ArgumentNullException.ThrowIfNull(indices);
        return NextSumTree(2, parameters, indices);
    }

    public Tensor NextSum(int sumSize, int productSize, IndicesType indices)
    {
        return NextSumTree(2, new Parameters(sumSize, sumSize, productSize, productSize), indices);
    }

    public Tensor NextTensorTree(int depth, Parameters parameters, IndicesType indices)
    {
        return NextTensorTree(_random.Next(2) == 0 ? TensorType.Product : TensorType.Sum, depth, parameters, indices);
    }

    public Tensor NextTensorTree(TensorType head, int depth, Parameters parameters, IndicesType indices)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        ArgumentNullException.ThrowIfNull(indices);

        IndicesType free = indices.GetFree();
        if (depth <= 0)
        {
            return NextSimpleTensor(IndicesFactory.CreateSimple(null, free));
        }

        return head switch
        {
            TensorType.Product => NextProductTree(depth, parameters, free),
            TensorType.Sum => NextSumTree(depth, parameters, free),
            _ => throw new InvalidOperationException()
        };
    }

    public Tensor NextTensorTree(int depth, int productSize, int sumSize, IndicesType indices)
    {
        return NextTensorTree(depth, new Parameters(sumSize, sumSize, productSize, productSize), indices);
    }

    public Tensor NextTensorTree(TensorType head, int depth, int productSize, int sumSize, IndicesType indices)
    {
        return NextTensorTree(head, depth, new Parameters(sumSize, sumSize, productSize, productSize), indices);
    }

    public Tensor NextSumTree(int depth, int productSize, int sumSize, IndicesType indices)
    {
        return NextSumTree(depth, new Parameters(sumSize, sumSize, productSize, productSize), indices);
    }

    public Tensor NextProductTree(int depth, int productSize, int sumSize, IndicesType indices)
    {
        return NextProductTree(depth, new Parameters(sumSize, sumSize, productSize, productSize), indices);
    }

    public Tensor NextProductTree(int depth, Parameters parameters, IndicesType indices)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        ArgumentNullException.ThrowIfNull(indices);

        indices = indices.GetFree();
        int productSize = GetRandomValue(parameters.MinProductSize, parameters.MaxProductSize);
        if (productSize < 1)
        {
            productSize = 1;
        }

        List<Tensor> factors = [];
        StructureOfIndices requiredStructure = StructureOfIndices.Create(IndicesFactory.CreateSimple(null, indices));
        int[] requiredCounts = GetTypeCounts(indices);
        int[] totalCounts = new int[IndexTypeMethods.TypesCount];

        for (int i = 0; i < productSize; i++)
        {
            Tensor factor = NextTensorTree(TensorType.Sum, depth - 1, parameters);
            factors.Add(factor);
            AddCounts(totalCounts, GetTypeCounts(factor.Indices.GetFree()));
        }

        for (byte type = 0; type < IndexTypeMethods.TypesCount; type++)
        {
            TypeData requiredTypeData = requiredStructure.GetTypeData(type);
            if (requiredTypeData.Length == 0)
            {
                continue;
            }

            while (totalCounts[type] < requiredTypeData.Length)
            {
                Tensor factor = NextTensorTree(TensorType.Sum, depth - 1, parameters);
                factors.Add(factor);
                AddCounts(totalCounts, GetTypeCounts(factor.Indices.GetFree()));
            }

            if (((totalCounts[type] - requiredTypeData.Length) & 1) != 0)
            {
                Tensor factor = NextTensorTree(
                    TensorType.Sum,
                    depth - 1,
                    parameters,
                    IndicesFactory.CreateSimple(null, NextIndices(CreateSingleTypeStructure(type, 1))));
                factors.Add(factor);
                AddCounts(totalCounts, GetTypeCounts(factor.Indices.GetFree()));
            }
        }

        Dictionary<byte, Stack<int>> pool = BuildIndexPool(indices.GetFree(), totalCounts);
        HashSet<int> forbidden = [];
        foreach (Stack<int> values in pool.Values)
        {
            foreach (int index in values)
            {
                forbidden.Add(IndicesUtils.GetNameWithType(index));
            }
        }

        List<Tensor> mappedFactors = new(factors.Count);
        foreach (Tensor factor in factors)
        {
            IndicesType free = factor.Indices.GetFree();
            int[] oldFree = free.AllIndices.ToArray();
            int[] newFree = TakeIndicesForFactor(free, pool);
            Tensor mapped = ApplyIndexMapping.Apply(factor, new Mapping(oldFree, newFree), forbidden.ToArray());
            mapped = ApplyIndexMapping.RenameDummy(mapped, forbidden.ToArray());
            forbidden.UnionWith(TensorUtils.GetAllIndicesNamesT(mapped));
            mappedFactors.Add(mapped);
        }

        return NRedberry.Tensors.Tensors.Multiply(mappedFactors);
    }

    public Tensor NextSumTree(int depth, Parameters parameters, IndicesType indices)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        ArgumentNullException.ThrowIfNull(indices);

        int sumSize = GetRandomValue(parameters.MinSumSize, parameters.MaxSumSize);
        if (sumSize < 1)
        {
            sumSize = 1;
        }

        Tensor[] summands = new Tensor[sumSize];
        for (int i = 0; i < sumSize; i++)
        {
            summands[i] = NextTensorTree(TensorType.Product, depth - 1, parameters, indices);
        }

        return NRedberry.Tensors.Tensors.Sum(summands);
    }

    public int[] NextIndices(StructureOfIndices structureOfIndices)
    {
        ArgumentNullException.ThrowIfNull(structureOfIndices);

        int[] indices = new int[structureOfIndices.Size];
        int position = 0;
        for (byte type = 0; type < IndexTypeMethods.TypesCount; type++)
        {
            TypeData typeData = structureOfIndices.GetTypeData(type);
            if (typeData.Length == 0)
            {
                continue;
            }

            int[] permutation = NextPermutation(typeData.Length);
            if (typeData.States is not null)
            {
                for (int i = 0; i < typeData.Length; i++)
                {
                    indices[position++] = IndicesUtils.CreateIndex(permutation[i], type, typeData.States[i]);
                }
            }
            else
            {
                int[] metricIndices = new int[typeData.Length];
                int contracted = NextInt(indices.Length / 2);
                if (contracted == 0)
                {
                    contracted = 1;
                }

                int i;
                for (i = 0; i < metricIndices.Length / contracted; i++)
                {
                    metricIndices[i] = IndicesUtils.SetType(type, i);
                }

                if (i - contracted < 0)
                {
                    contracted = i;
                }

                for (; i < metricIndices.Length; i++)
                {
                    metricIndices[i] = IndicesUtils.CreateIndex(i - contracted, type, true);
                }

                Shuffle(metricIndices);
                Array.Copy(metricIndices, 0, indices, position, metricIndices.Length);
                position += metricIndices.Length;
            }
        }

        return indices;
    }

    public int[] NextPermutation(int dimension)
    {
        return Permutations.RandomPermutation(dimension, _random);
    }

    public void Shuffle(int[] target)
    {
        ArgumentNullException.ThrowIfNull(target);
        Permutations.Shuffle(target, _random);
    }

    private RandomTensor(
        int minDifferentDescriptors,
        int maxDifferentDescriptors,
        int[] minIndices,
        int[] maxIndices,
        bool withSymmetries,
        bool generateNewDescriptors,
        System.Random random,
        long seed)
    {
        ArgumentNullException.ThrowIfNull(minIndices);
        ArgumentNullException.ThrowIfNull(maxIndices);
        if (minIndices.Length > IndexTypeMethods.TypesCount || maxIndices.Length > IndexTypeMethods.TypesCount)
        {
            throw new ArgumentException("Index count arrays exceed the number of supported index types.");
        }

        _random = random ?? throw new ArgumentNullException(nameof(random));
        _seed = seed;
        _withSymmetries = withSymmetries;
        _generateNewDescriptors = generateNewDescriptors;
        _minIndices = PadCounts(minIndices);
        _maxIndices = PadCounts(maxIndices);
        _initialNamespaceSize = minDifferentDescriptors + Math.Max(0, (maxDifferentDescriptors - minDifferentDescriptors) / 2);
        _namespace = new List<NameDescriptor>(_initialNamespaceSize);
        GenerateDescriptors();
    }

    private static System.Random CreateSeededRandom(out long seed)
    {
        seed = System.Random.Shared.NextInt64();
        return new System.Random(unchecked((int)seed));
    }

    private void GenerateDescriptors()
    {
        if (!_generateNewDescriptors)
        {
            return;
        }

        for (int i = 0; i < _initialNamespaceSize; i++)
        {
            int[] counts = new int[IndexTypeMethods.TypesCount];
            for (int type = 0; type < IndexTypeMethods.TypesCount; type++)
            {
                counts[type] = _minIndices[type] + NextInt(Math.Max(1, _maxIndices[type] - _minIndices[type]));
            }

            StructureOfIndices structure = CreateStructure(counts);
            NameDescriptor descriptor = CC.NameManager.MapNameDescriptor(NextName(), structure);
            if (_withSymmetries)
            {
                AddRandomSymmetries(descriptor);
            }

            _namespace.Add(descriptor);
        }
    }

    private void AddRandomSymmetries(NameDescriptor descriptor)
    {
        if (!descriptor.GetSymmetries().IsTrivial())
        {
            return;
        }

        StructureOfIndices structure = descriptor.GetStructureOfIndices();
        for (byte type = 0; type < IndexTypeMethods.TypesCount; type++)
        {
            TypeData typeData = structure.GetTypeData(type);
            if (typeData.Length < 2)
            {
                continue;
            }

            int count = _random.Next(4);
            for (int i = 0; i < count; i++)
            {
                descriptor.GetSymmetries().AddSymmetry(IndexTypeMethods.GetType(type), NextPermutation(typeData.Length));
            }
        }
    }

    private string NextName()
    {
        return $"RT{_nextNameIndex++}";
    }

    private NameDescriptor NextNameDescriptor(StructureOfIndices structure)
    {
        foreach (NameDescriptor descriptor in _namespace)
        {
            if (descriptor.GetStructureOfIndices().Equals(structure))
            {
                return descriptor;
            }
        }

        if (!_generateNewDescriptors)
        {
            throw new ArgumentException($"No descriptor for such structure: {structure}");
        }

        NameDescriptor created = CC.NameManager.MapNameDescriptor(NextName(), structure);
        if (_withSymmetries)
        {
            AddRandomSymmetries(created);
        }

        _namespace.Add(created);
        return created;
    }

    private Tensor NextTensorTree(TensorType head, int depth, Parameters parameters)
    {
        NameDescriptor descriptor = NextNameDescriptor();
        return NextTensorTree(
            head,
            depth,
            parameters,
            IndicesFactory.CreateSimple(null, NextIndices(descriptor.GetStructureOfIndices())));
    }

    private static int[] PadCounts(int[] source)
    {
        int[] result = new int[IndexTypeMethods.TypesCount];
        Array.Copy(source, result, source.Length);
        return result;
    }

    private StructureOfIndices CreateStructure(int[] counts)
    {
        BitArray[] states = new BitArray[IndexTypeMethods.TypesCount];
        for (byte type = 0; type < IndexTypeMethods.TypesCount; type++)
        {
            if (!CC.IsMetric(type))
            {
                BitArray bits = new(counts[type]);
                for (int i = 0; i < counts[type]; i++)
                {
                    bits[i] = _random.Next(2) == 0;
                }

                states[type] = bits;
            }
        }

        return StructureOfIndices.Create(counts, states);
    }

    private static StructureOfIndices CreateSingleTypeStructure(byte type, int count)
    {
        int[] counts = new int[IndexTypeMethods.TypesCount];
        BitArray[] states = new BitArray[IndexTypeMethods.TypesCount];
        counts[type] = count;
        for (byte currentType = 0; currentType < IndexTypeMethods.TypesCount; currentType++)
        {
            if (CC.IsMetric(currentType))
            {
                continue;
            }

            states[currentType] = currentType == type
                ? new BitArray(count)
                : BitArrayExtensions.Empty;
        }

        return StructureOfIndices.Create(counts, states);
    }

    private static int[] GetTypeCounts(IndicesType indices)
    {
        StructureOfIndices structure = StructureOfIndices.Create(IndicesFactory.CreateSimple(null, indices));
        int[] counts = new int[IndexTypeMethods.TypesCount];
        Array.Copy(structure.TypesCounts, counts, counts.Length);
        return counts;
    }

    private static void AddCounts(int[] target, int[] source)
    {
        for (int i = 0; i < target.Length; i++)
        {
            target[i] += source[i];
        }
    }

    private Dictionary<byte, Stack<int>> BuildIndexPool(IndicesType freeIndices, int[] totalCounts)
    {
        Dictionary<byte, Stack<int>> pool = new();
        IndexGenerator generator = new(freeIndices.AllIndices.ToArray());
        Dictionary<byte, List<int>> values = new();
        for (byte type = 0; type < IndexTypeMethods.TypesCount; type++)
        {
            values[type] = [];
        }

        foreach (int index in freeIndices.AllIndices)
        {
            values[IndicesUtils.GetType(index)].Add(index);
        }

        int[] requiredCounts = GetTypeCounts(freeIndices);
        for (byte type = 0; type < IndexTypeMethods.TypesCount; type++)
        {
            int additional = totalCounts[type] - requiredCounts[type];
            for (int i = 0; i < additional / 2; i++)
            {
                int lower = IndicesUtils.SetState(false, generator.Generate(type));
                values[type].Add(lower);
                values[type].Add(IndicesUtils.InverseIndexState(lower));
            }

            int[] array = values[type].ToArray();
            Shuffle(array);
            pool[type] = new Stack<int>(array);
        }

        return pool;
    }

    private static int[] TakeIndicesForFactor(IndicesType freeIndices, Dictionary<byte, Stack<int>> pool)
    {
        StructureOfIndices structure = StructureOfIndices.Create(IndicesFactory.CreateSimple(null, freeIndices));
        int[] factorIndices = new int[structure.Size];
        int position = 0;
        for (byte type = 0; type < IndexTypeMethods.TypesCount; type++)
        {
            TypeData typeData = structure.GetTypeData(type);
            if (typeData.Length == 0)
            {
                continue;
            }

            Stack<int> values = pool[type];
            for (int i = 0; i < typeData.Length; i++)
            {
                int index = values.Pop();
                factorIndices[position++] = typeData.States is null
                    ? index
                    : IndicesUtils.SetState(typeData.States[i], index);
            }
        }

        return factorIndices;
    }

    private int GetRandomValue(int min, int max)
    {
        return min == max ? min : min + _random.Next(max - min);
    }
}
