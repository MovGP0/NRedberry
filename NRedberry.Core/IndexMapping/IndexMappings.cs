using System.Collections.Generic;
using NRedberry.Concurrent;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using TensorCC = NRedberry.Tensors.CC;

namespace NRedberry.IndexMapping;

public static class IndexMappings
{
    private static IReadOnlyDictionary<Type, IIndexMappingProviderFactory> ProviderFactories { get; } =
        new Dictionary<Type, IIndexMappingProviderFactory>
        {
            [typeof(SimpleTensor)] = ProviderSimpleTensor.SimpleTensorFactory,
            [typeof(TensorField)] = ProviderSimpleTensor.TensorFieldFactory,
            [typeof(Product)] = ProviderProduct.Factory,
            [typeof(Sum)] = ProviderSum.Factory,
            [typeof(Expression)] = ProviderSum.Factory,
            [typeof(Complex)] = ProviderComplex.Factory,
            [typeof(Power)] = ProviderPower.Instance,
            [typeof(Sin)] = ProviderFunctions.OddFactory,
            [typeof(ArcSin)] = ProviderFunctions.OddFactory,
            [typeof(Tan)] = ProviderFunctions.OddFactory,
            [typeof(ArcTan)] = ProviderFunctions.OddFactory,
            [typeof(Cos)] = ProviderFunctions.EvenFactory,
            [typeof(ArcCos)] = ProviderFunctions.EvenFactory,
            [typeof(Cot)] = ProviderFunctions.EvenFactory,
            [typeof(ArcCot)] = ProviderFunctions.EvenFactory,
            [typeof(Log)] = ProviderFunctions.Factory
        };

    public static MappingsPort CreatePort(Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        return new MappingsPort(CreatePortOfBuffers(from, to));
    }

    public static MappingsPort SimpleTensorsPort(SimpleTensor from, SimpleTensor to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        IIndexMappingProvider provider = ProviderSimpleTensor.SimpleTensorFactory.Create(
            IndexMappingProviderUtil.Singleton(new IndexMappingsBufferImpl()),
            from,
            to);
        provider.Tick();
        return new MappingsPort(new MappingsPortRemovingContracted(provider));
    }

    public static MappingsPort CreateBijectiveProductPort(Tensor[] from, Tensor[] to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        if (from.Length != to.Length)
        {
            throw new ArgumentException("From length != to length.");
        }

        if (from.Length == 0)
        {
            return new MappingsPort(IndexMappingProviderUtil.Singleton(new IndexMappingsBufferImpl()));
        }

        if (from.Length == 1)
        {
            return new MappingsPort(CreatePortOfBuffers(from[0], to[0]));
        }

        return new MappingsPort(
            new MappingsPortRemovingContracted(
                new SimpleProductMappingsPort(
                    IndexMappingProviderUtil.Singleton(new IndexMappingsBufferImpl()),
                    from,
                    to)));
    }

    public static Mapping? GetFirst(Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        IIndexMappingBuffer? buffer = CreatePortOfBuffers(from, to).Take();
        if (buffer is null)
        {
            return null;
        }

        return new Mapping(buffer);
    }

    public static bool TestMapping(Mapping mapping, Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(mapping);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        return IndexMappingsBufferTester.Test(new IndexMappingsBufferTester(mapping), from, to);
    }

    public static bool AnyMappingExists(Tensor a, Tensor b)
    {
        ArgumentNullException.ThrowIfNull(a);
        ArgumentNullException.ThrowIfNull(b);

        return MappingExists(a, b) || MappingExists(b, a);
    }

    public static bool MappingExists(Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        return GetFirstBuffer(from, to) is not null;
    }

    public static bool PositiveMappingExists(Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        IOutputPort<IIndexMappingBuffer> port = CreatePortOfBuffers(from, to);
        IIndexMappingBuffer? buffer;
        while ((buffer = port.Take()) is not null)
        {
            if (!buffer.GetSign())
            {
                return true;
            }
        }

        return false;
    }

    public static bool Equals(Tensor u, Tensor v)
    {
        ArgumentNullException.ThrowIfNull(u);
        ArgumentNullException.ThrowIfNull(v);

        if (ReferenceEquals(u, v))
        {
            return true;
        }

        NRedberry.Indices.Indices freeIndices = u.Indices.GetFree();
        if (!freeIndices.EqualsRegardlessOrder(v.Indices.GetFree()))
        {
            return false;
        }

        IIndexMappingBuffer tester = new IndexMappingsBufferTester(IndicesUtils.GetIndicesNames(freeIndices), false);
        IOutputPort<IIndexMappingBuffer> mappingPort = CreatePortOfBuffers(tester, u, v);
        IIndexMappingBuffer? buffer;
        while ((buffer = mappingPort.Take()) is not null)
        {
            if (!buffer.GetSign())
            {
                return true;
            }
        }

        return false;
    }

    public static bool? Compare1(Tensor u, Tensor v)
    {
        ArgumentNullException.ThrowIfNull(u);
        ArgumentNullException.ThrowIfNull(v);

        NRedberry.Indices.Indices freeIndices = u.Indices.GetFree();
        if (!freeIndices.EqualsRegardlessOrder(v.Indices.GetFree()))
        {
            return null;
        }

        IIndexMappingBuffer tester = new IndexMappingsBufferTester(IndicesUtils.GetIndicesNames(freeIndices), false);
        IIndexMappingBuffer? buffer = CreatePortOfBuffers(tester, u, v).Take();
        if (buffer is null)
        {
            return null;
        }

        return buffer.GetSign();
    }

    public static bool IsZeroDueToSymmetry(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        int[] indices = IndicesUtils.GetIndicesNames(tensor.Indices.GetFree());
        IndexMappingsBufferTester tester = new(indices, false);
        IOutputPort<IIndexMappingBuffer> mappingPort = CreatePortOfBuffers(tester, tensor, tensor);
        IIndexMappingBuffer? buffer;
        while ((buffer = mappingPort.Take()) is not null)
        {
            if (buffer.GetSign())
            {
                return true;
            }
        }

        return false;
    }

    public static ISet<Mapping> GetAllMappings(Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        return GetAllMappings(CreatePort(from, to));
    }

    private static ISet<Mapping> GetAllMappings(IOutputPort<Mapping> outputPort)
    {
        ArgumentNullException.ThrowIfNull(outputPort);

        HashSet<Mapping> mappings = [];
        Mapping? mapping;
        while ((mapping = outputPort.Take()) is not null)
        {
            mappings.Add(mapping);
        }

        return mappings;
    }

    internal static IOutputPort<IIndexMappingBuffer> CreatePortOfBuffers(Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        return CreatePortOfBuffers(new IndexMappingsBufferImpl(), from, to);
    }

    internal static IOutputPort<IIndexMappingBuffer> CreatePortOfBuffers(IIndexMappingBuffer buffer, Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(buffer);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        IIndexMappingProvider provider = CreatePort(IndexMappingProviderUtil.Singleton(buffer), from, to);
        provider.Tick();
        return new MappingsPortRemovingContracted(provider);
    }

    internal static IIndexMappingBuffer? GetFirstBuffer(Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        return CreatePortOfBuffers(from, to).Take();
    }

    private static Tensor? ExtractNonComplexFactor(Tensor tensor)
    {
        if (tensor is not Product product)
        {
            return null;
        }

        return product.Factor.IsMinusOne() ? product[1] : null;
    }

    internal static IIndexMappingProvider CreatePort(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        if (from.GetHashCode() != to.GetHashCode())
        {
            return IndexMappingProviderUtil.EmptyProvider;
        }

        if (from.GetType() != to.GetType())
        {
            Tensor? nonComplex;
            if (from is Product && to is not Product)
            {
                if (from.Size != 2)
                {
                    return IndexMappingProviderUtil.EmptyProvider;
                }

                nonComplex = ExtractNonComplexFactor(from);
                return nonComplex is not null
                    ? new MinusIndexMappingProviderWrapper(CreatePort(provider, nonComplex, to))
                    : IndexMappingProviderUtil.EmptyProvider;
            }

            if (to is Product && from is not Product)
            {
                if (to.Size != 2)
                {
                    return IndexMappingProviderUtil.EmptyProvider;
                }

                nonComplex = ExtractNonComplexFactor(to);
                return nonComplex is not null
                    ? new MinusIndexMappingProviderWrapper(CreatePort(provider, from, nonComplex))
                    : IndexMappingProviderUtil.EmptyProvider;
            }

            return IndexMappingProviderUtil.EmptyProvider;
        }

        if (!ProviderFactories.TryGetValue(from.GetType(), out IIndexMappingProviderFactory? factory))
        {
            throw new InvalidOperationException("Unsupported tensor type: " + from.GetType());
        }

        return factory.Create(provider, from, to);
    }
}

internal sealed record class IndexMappingsFromToHolder(int[] From, int[] To, bool Sign);
