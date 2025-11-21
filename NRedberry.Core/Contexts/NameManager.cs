using System.Collections.Concurrent;
using NRedberry.Indices;
using NRedberry.Parsers;

namespace NRedberry.Contexts;

/// <summary>
/// Object of this class represents a namespace of simple tensor and tensor fields in Redberry.
/// It is responsible for generation of unique name descriptors({ @link NameDescriptor}) and integer
/// identifiers for simple tensors and fields from raw data.These identifiers are the same for tensors
/// with the same mathematical nature.They are generated randomly in order to obtain the uniform distribution
/// through Redberry session.Each session of Redberry holds only one instance of this class, it can be obtained
/// through <see cref="Tensors.Tensors.CC.GetNameManager()"/>.
/// </summary>
public sealed class NameManager
{
    private long seed;

    public long Seed => seed;

    private Random random;
    private readonly ReaderWriterLockSlim readWriteLock = new();
    private readonly ConcurrentDictionary<int, NameDescriptor> fromId = new();
    private readonly HashSet<string> stringNames = [];
    private readonly StringGenerator stringGenerator = new();
    private readonly Dictionary<NameAndStructureOfIndices, NameDescriptor> fromStructure = new();

    private readonly string[] kroneckerAndMetricNames = ["d", "g"];

    public string DiracDeltaName { get; set; } = "DiracDelta";

    private readonly List<int> kroneckerAndMetricIds = [];

    public NameManager(long? seed, string kronecker, string metric)
    {
        if (seed == null)
        {
            random = new Random();
            this.seed = random.NextInt64();
            random = new Random((int)this.seed);
        }
        else
        {
            this.seed = seed.Value;
            random = new Random((int)this.seed);
        }

        kroneckerAndMetricNames[0] = kronecker;
        kroneckerAndMetricNames[1] = metric;
    }

    public bool IsKroneckerOrMetric(int name)
    {
        return kroneckerAndMetricIds.BinarySearch(name) >= 0;
    }

    public string KroneckerName
    {
        get => kroneckerAndMetricNames[0];
        set
        {
            kroneckerAndMetricNames[0] = value;
            Rebuild();
        }
    }

    public string MetricName
    {
        get => kroneckerAndMetricNames[1];
        set
        {
            kroneckerAndMetricNames[1] = value;
            Rebuild();
        }
    }

    private void Rebuild()
    {
        readWriteLock.EnterWriteLock();
        try
        {
            fromStructure.Clear();
            foreach (var descriptor in fromId.Values)
            {
                foreach (var key in descriptor.GetKeys())
                {
                    fromStructure[key] = descriptor;
                }
            }
        }
        finally
        {
            readWriteLock.ExitWriteLock();
        }
    }

    private NameDescriptor CreateDescriptor(string sname, StructureOfIndices[] structuresOfIndices, int id)
    {
        if (structuresOfIndices.Length != 1)
            return new NameDescriptorForTensorFieldImpl(sname, structuresOfIndices, id, sname.Equals(DiracDeltaName) && structuresOfIndices.Length == 3);

        var its = structuresOfIndices[0];
        if (its.Size != 2)
            return new NameDescriptorForSimpleTensor(sname, structuresOfIndices, id);

        for (byte b = 0; b < IndexTypeExtensions.Length; ++b)
        {
            if (its.TypeCount(b) == 2)
            {
                if (CC.IsMetric(b))
                {
                    if (sname.Equals(kroneckerAndMetricNames[0]) || sname.Equals(kroneckerAndMetricNames[1]))
                    {
                        var descriptor = new NameDescriptorForMetricAndKronecker(kroneckerAndMetricNames, b, id);
                        descriptor.GetSymmetries().Add(b, false, 1, 0);
                        return descriptor;
                    }
                }
                else
                {
                    if (sname.Equals(kroneckerAndMetricNames[1]))
                        throw new ParserException("Metric is not specified for non metric index type.");

                    if (sname.Equals(kroneckerAndMetricNames[0]))
                    {
                        if (!its.GetTypeData(b).States[0] || its.GetTypeData(b).States[1])
                            throw new ParserException("Illegal Kronecker indices states.");

                        return new NameDescriptorForMetricAndKronecker(kroneckerAndMetricNames, b, id);
                    }
                }
            }
        }

        return new NameDescriptorForSimpleTensor(sname, structuresOfIndices, id);
    }

    /// <remarks>USE IT ONLY INSIDE LOCK!!!</remarks>
    private void RegisterDescriptor(NameDescriptor descriptor)
    {
        fromId[descriptor.Id] = descriptor;
        foreach (var key in descriptor.GetKeys())
        {
            fromStructure[key] = descriptor;
        }

        descriptor.RegisterInNameManager(this);
    }

    public NameDescriptor MapNameDescriptor(string sname, params StructureOfIndices[] structureOfIndices)
    {
        var key = new NameAndStructureOfIndices(sname, structureOfIndices);
        bool rLocked = true;
        readWriteLock.EnterReadLock();
        try
        {
            if (!fromStructure.TryGetValue(key, out var knownND))
            {
                readWriteLock.ExitReadLock();
                rLocked = false;
                readWriteLock.EnterWriteLock();
                try
                {
                    if (!fromStructure.TryGetValue(key, out knownND))
                    {
                        int name = GenerateNewName();
                        var descriptor = CreateDescriptor(sname, structureOfIndices, name);
                        if (descriptor is NameDescriptorForMetricAndKronecker)
                        {
                            kroneckerAndMetricIds.Add(name);
                            kroneckerAndMetricIds.Sort();
                        }

                        RegisterDescriptor(descriptor);
                        stringNames.Add(sname);
                        return descriptor;
                    }

                    readWriteLock.EnterReadLock();
                    rLocked = true;
                }
                finally
                {
                    readWriteLock.ExitWriteLock();
                }
            }

            return knownND;
        }
        finally
        {
            if (rLocked)
                readWriteLock.ExitReadLock();
        }
    }

    internal NameDescriptorForTensorFieldDerivative CreateDescriptorForFieldDerivative(NameDescriptorForTensorFieldImpl field, int[] orders)
    {
        readWriteLock.EnterWriteLock();
        try
        {
            var result = new NameDescriptorForTensorFieldDerivative(GenerateNewName(), orders, field);
            RegisterDescriptor(result);
            return result;
        }
        finally
        {
            readWriteLock.ExitWriteLock();
        }
    }

    public void Reset()
    {
        readWriteLock.EnterWriteLock();
        try
        {
            kroneckerAndMetricIds.Clear();
            stringNames.Clear();
            fromId.Clear();
            fromStructure.Clear();
            seed = random.NextInt64();
            random = new Random((int)seed);
        }
        finally
        {
            readWriteLock.ExitWriteLock();
        }
    }

    public void Reset(long seed)
    {
        readWriteLock.EnterWriteLock();
        try
        {
            kroneckerAndMetricIds.Clear();
            stringNames.Clear();
            fromId.Clear();
            fromStructure.Clear();
            this.seed = seed;
            random = new Random((int)seed);
        }
        finally
        {
            readWriteLock.ExitWriteLock();
        }
    }

    private int GenerateNewName()
    {
        int name;
        do
        {
            name = random.Next();
        } while (fromId.ContainsKey(name));
        return name;
    }

    public NameDescriptor GetNameDescriptor(int nameId)
    {
        readWriteLock.EnterReadLock();
        try
        {
            fromId.TryGetValue(nameId, out var descriptor);
            return descriptor;
        }
        finally
        {
            readWriteLock.ExitReadLock();
        }
    }

    public NameDescriptor GenerateNewSymbolDescriptor()
    {
        bool rLocked = true;
        readWriteLock.EnterReadLock();
        try
        {
            int newNameId = GenerateNewName();
            string name;
            do
            {
                name = stringGenerator.NextString();
            } while (stringNames.Contains(name));
            stringNames.Add(name);
            var nd = new NameDescriptorForSimpleTensor(name, [StructureOfIndices.Empty], newNameId);
            readWriteLock.ExitReadLock();
            rLocked = false;
            readWriteLock.EnterWriteLock();
            try
            {
                RegisterDescriptor(nd);
                readWriteLock.EnterReadLock();
                rLocked = true;
            }
            finally
            {
                readWriteLock.ExitWriteLock();
            }

            return nd;
        }
        finally
        {
            if (rLocked)
                readWriteLock.ExitReadLock();
        }
    }

    public int Size()
    {
        readWriteLock.EnterWriteLock();
        try
        {
            return fromId.Count;
        }
        finally
        {
            readWriteLock.ExitWriteLock();
        }
    }

    public long GetSeed() => seed;

    public Random GetRandomGenerator() => random;

    private sealed class StringGenerator
    {
        private long count;

        public string NextString() => $"{DEFAULT_VAR_SYMBOL_PREFIX}{count++}";
    }

    public const string DEFAULT_VAR_SYMBOL_PREFIX = "rc";
}
