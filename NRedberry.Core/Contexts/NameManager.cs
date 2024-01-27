using System;
using System.Collections.Generic;
using NRedberry.Core.Indices;
using NRedberry.Core.Tensors;
using NRedberry.Core.Utils;

namespace NRedberry.Contexts;

/// <summary>
/// Object of this class represents a namespace of simple tensor and tensor fields in Redberry.
/// It is responsible for generation of unique name descriptors({ @link NameDescriptor}) and integer
/// identifiers for simple tensors and fields from raw data.These identifiers are the same for tensors
/// with the same mathematical nature.They are generated randomly in order to obtain the uniform distribution
/// through Redberry session.Each session of Redberry holds only one instance of this class, it can be obtained
/// through <see cref="Tensors.CC.GetNameManager()"/>.
/// </summary>
public sealed class NameManager
{
    private long seed;
    public long Seed => seed;
    private Random random;
    private object readLock = new();
    private object writeLock = new();

    private IDictionary<int, NameDescriptor> fromId = new Dictionary<int, NameDescriptor>();
    private IDictionary<NameAndStructureOfIndices, NameDescriptor> fromStructure = new Dictionary<NameAndStructureOfIndices, NameDescriptor>();
    private readonly string[] kroneckerAndMetricNames = {"d", "g"};
    private IntArrayList kroneckerAndMetricIds = new();

    public NameManager(int seed, string kronecker, string metric)
    {
        random = new Random(seed);
        kroneckerAndMetricNames[0] = kronecker;
        kroneckerAndMetricNames[1] = metric;
    }

    public void Reset()
    {
        throw new NotImplementedException();
    }

    public void Reset(int seed)
    {
        throw new NotImplementedException();
    }

    public NameDescriptor GetNameDescriptor(int nameId)
    {
        throw new NotImplementedException();
    }

    public string GetKroneckerName()
    {
        throw new NotImplementedException();
    }

    public string GetMetricName()
    {
        throw new NotImplementedException();
    }

    public void SetMetricName(string name)
    {
        throw new NotImplementedException();
    }

    public void SetKroneckerName(string name)
    {
        throw new NotImplementedException();
    }

    public bool IsKroneckerOrMetric(int tName)
    {
        throw new NotImplementedException();
    }

    public object getKroneckerName()
    {
        throw new NotImplementedException();
    }

    public NameDescriptor mapNameDescriptor(object o, StructureOfIndices structureOfIndices)
    {
        throw new NotImplementedException();
    }

    public bool isKroneckerOrMetric(int name)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Generates a new name descriptor for a simple tensor with given structure of indices.
    /// </summary>
    /// <remarks>
    /// <b>Important:</b> run only in write lock!
    /// </remarks>
    private int GenerateNewName()
    {
        int name;
        do
            name = random.Next();
        while (fromId.ContainsKey(name));
        return name;
    }

    internal NameDescriptorForTensorFieldDerivative CreateDescriptorForFieldDerivative(
        NameDescriptorForTensorFieldImpl field,
        int[] orders)
    {
        lock (writeLock)
        {
            var result = new NameDescriptorForTensorFieldDerivative(GenerateNewName(), orders, field);
            RegisterDescriptor(result);
            return result;
        }
    }
}