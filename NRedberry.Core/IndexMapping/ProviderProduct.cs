using NRedberry.Concurrent;
using NRedberry.Core.Combinatorics;
using NRedberry.Core.Utils.Stretces;
using NRedberry.Numbers;
using NRedberry.Tensors;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/ProviderProduct.java
 */

internal sealed class ProviderProduct : IIndexMappingProvider
{
    public static IIndexMappingProviderFactory Factory { get; } = new ProviderProductFactory();

    private readonly DummyIndexMappingProvider _dummyProvider;
    private readonly IOutputPort<IIndexMappingBuffer> _outputPort;

    private ProviderProduct(IOutputPort<IIndexMappingBuffer> outputPort, Product from, Product to)
    {
        ArgumentNullException.ThrowIfNull(outputPort);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        _dummyProvider = new DummyIndexMappingProvider(outputPort);

        int begin = 0;
        ProductContent fromContent = from.Content;
        ProductContent toContent = to.Content;

        List<Pair> stretches = [];
        List<IIndexMappingProvider> providers = [];

        IIndexMappingProvider lastOutput = _dummyProvider;

        Tensor[] indexlessFrom = [..from.IndexlessData];
        Tensor[] indexlessTo = [..to.IndexlessData];

        for (int i = 1; i <= indexlessFrom.Length; ++i)
        {
            if (i != indexlessFrom.Length && indexlessFrom[i].GetHashCode() == indexlessFrom[i - 1].GetHashCode())
            {
                continue;
            }

            if (i - 1 != begin)
            {
                providers.Add(
                    lastOutput = new PermutatorProvider(
                        lastOutput,
                        indexlessFrom[begin..i],
                        indexlessTo[begin..i]));
            }

            begin = i;
        }

        begin = 0;
        for (int i = 1; i <= indexlessFrom.Length; ++i)
        {
            if (i != indexlessFrom.Length && indexlessFrom[i].GetHashCode() == indexlessFrom[i - 1].GetHashCode())
            {
                continue;
            }

            if (i - 1 == begin)
            {
                providers.Add(
                    lastOutput = IndexMappings.CreatePort(
                        lastOutput,
                        indexlessFrom[begin],
                        indexlessTo[begin]));
            }

            begin = i;
        }

        begin = 0;
        for (int i = 1; i <= fromContent.Size; ++i)
        {
            if (i != fromContent.Size
                && fromContent.StructureOfContractionsHashed[i].Equals(fromContent.StructureOfContractionsHashed[i - 1]))
            {
                continue;
            }

            if (i - 1 != begin)
            {
                stretches.Add(new Pair(fromContent.GetRange(begin, i), toContent.GetRange(begin, i)));
            }
            else
            {
                providers.Add(
                    lastOutput = IndexMappings.CreatePort(
                        lastOutput,
                        fromContent[begin],
                        toContent[begin]));
            }

            begin = i;
        }

        stretches.Sort();

        foreach (Pair pair in stretches)
        {
            providers.Add(lastOutput = new PermutatorProvider(lastOutput, pair.From, pair.To));
        }

        _outputPort = new SimpleProductMappingsPort([..providers]);
    }

    public bool Tick()
    {
        return _dummyProvider.Tick();
    }

    public IIndexMappingBuffer Take()
    {
        IIndexMappingBuffer buffer = _outputPort.Take();
        if (buffer is null)
        {
            return null!;
        }

        buffer.RemoveContracted();
        return buffer;
    }

    private static bool? CompareFactors(Complex first, Complex second)
    {
        ArgumentNullException.ThrowIfNull(first);
        ArgumentNullException.ThrowIfNull(second);

        if (first.Equals(second))
        {
            return false;
        }

        if (first.Equals(second.Negate()))
        {
            return true;
        }

        return null;
    }

    private static bool TestScalars(Tensor[] from, Tensor[] to)
    {
        if (from.Length != to.Length)
        {
            return false;
        }

        int[] hashes = new int[from.Length];
        for (int i = 0; i < from.Length; ++i)
        {
            hashes[i] = from[i].GetHashCode();
            if (hashes[i] != to[i].GetHashCode())
            {
                return false;
            }
        }

        PrecalculatedStretches precalculatedStretches = new(hashes);
        foreach (Stretch stretch in precalculatedStretches)
        {
            if (stretch.Length != 1)
            {
                continue;
            }

            if (!MappingExists(from[stretch.From], to[stretch.From]))
            {
                return false;
            }
        }

        foreach (Stretch stretch in precalculatedStretches)
        {
            if (stretch.Length <= 1)
            {
                continue;
            }

            bool found = false;
            IntPermutationsGenerator permutationsGenerator = new(stretch.Length);
            while (permutationsGenerator.MoveNext())
            {
                int[] permutation = permutationsGenerator.Current;
                bool valid = true;
                for (int i = 0; i < stretch.Length; ++i)
                {
                    if (MappingExists(from[stretch.From + i], to[stretch.From + permutation[i]]))
                    {
                        continue;
                    }

                    valid = false;
                    break;
                }

                if (!valid)
                {
                    continue;
                }

                found = true;
                break;
            }

            if (!found)
            {
                return false;
            }
        }

        return true;
    }

    private static bool MappingExists(Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        return IndexMappings.CreatePortOfBuffers(from, to).Take() is not null;
    }

    private static Tensor[] GetAllScalarsWithoutFactor(Product product)
    {
        ArgumentNullException.ThrowIfNull(product);

        Tensor[] scalars = product.Content.Scalars;
        Tensor[] result = new Tensor[product.IndexlessData.Length + scalars.Length];
        Array.Copy(product.IndexlessData, result, product.IndexlessData.Length);
        Array.Copy(scalars, 0, result, product.IndexlessData.Length, scalars.Length);
        return result;
    }

    private static Tensor GetWithoutFactor(Product product, int index)
    {
        ArgumentNullException.ThrowIfNull(product);

        return index < product.IndexlessData.Length
            ? product.IndexlessData[index]
            : product.Data[index - product.IndexlessData.Length];
    }

    internal sealed class ProviderProductFactory : IIndexMappingProviderFactory
    {
        public IIndexMappingProvider Create(IIndexMappingProvider provider, Tensor from, Tensor to)
        {
            ArgumentNullException.ThrowIfNull(provider);
            ArgumentNullException.ThrowIfNull(from);
            ArgumentNullException.ThrowIfNull(to);

            Product fromProduct = (Product)from;
            Product toProduct = (Product)to;

            if (fromProduct.IndexlessData.Length + fromProduct.Data.Length != toProduct.IndexlessData.Length + toProduct.Data.Length)
            {
                return IndexMappingProviderUtil.EmptyProvider;
            }

            bool? factorComparison = CompareFactors(fromProduct.Factor, toProduct.Factor);
            if (factorComparison is null)
            {
                return IndexMappingProviderUtil.EmptyProvider;
            }

            if (fromProduct.Factor.Equals(toProduct.Factor))
            {
                for (int i = 0; i < fromProduct.IndexlessData.Length + fromProduct.Data.Length; ++i)
                {
                    if (GetWithoutFactor(fromProduct, i).GetHashCode() == GetWithoutFactor(toProduct, i).GetHashCode())
                    {
                        continue;
                    }

                    return IndexMappingProviderUtil.EmptyProvider;
                }
            }

            ProductContent fromContent = fromProduct.Content;
            ProductContent toContent = toProduct.Content;
            if (!fromContent.StructureOfContractionsHashed.Equals(toContent.StructureOfContractionsHashed))
            {
                return IndexMappingProviderUtil.EmptyProvider;
            }

            Tensor[] fromScalars = GetAllScalarsWithoutFactor(fromProduct);
            Tensor[] toScalars = GetAllScalarsWithoutFactor(toProduct);
            if (fromScalars.Length != toScalars.Length)
            {
                return IndexMappingProviderUtil.EmptyProvider;
            }

            if (fromScalars.Length != 1 && !TestScalars(fromScalars, toScalars))
            {
                return IndexMappingProviderUtil.EmptyProvider;
            }

            ProviderProduct providerProduct = new(provider, fromProduct, toProduct);
            return factorComparison.Value
                ? new MinusIndexMappingProviderWrapper(providerProduct)
                : providerProduct;
        }
    }

    private sealed class Pair(Tensor[] from, Tensor[] to) : IComparable<Pair>
    {
        public Tensor[] From { get; } = from;
        public Tensor[] To { get; } = to;

        public int CompareTo(Pair? other)
        {
            if (other is null)
            {
                return 1;
            }

            return From.Length.CompareTo(other.From.Length);
        }
    }
}

internal sealed class PermutatorProvider : IndexMappingProviderAbstract
{
    private readonly IntPermutationsGenerator _generator;
    private readonly Tensor[] _from;
    private readonly Tensor[] _to;
    private SimpleProductMappingsPort? _currentProvider;

    public PermutatorProvider(IOutputPort<IIndexMappingBuffer> outputPort, Tensor[] from, Tensor[] to)
        : base(outputPort)
    {
        ArgumentNullException.ThrowIfNull(outputPort);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        _from = from;
        _to = to;
        _generator = new IntPermutationsGenerator(from.Length);
    }

    protected override void BeforeTick()
    {
        _generator.Reset();
        _currentProvider = null;
    }

    public override IIndexMappingBuffer? Take()
    {
        if (currentBuffer is null)
        {
            return null;
        }

        while (_currentProvider is null)
        {
            if (!_generator.MoveNext())
            {
                return null;
            }

            int[] permutation = _generator.Current;
            Tensor[] newTo = new Tensor[_to.Length];
            for (int i = 0; i < _to.Length; ++i)
            {
                newTo[i] = _to[permutation[i]];
            }

            _currentProvider = new SimpleProductMappingsPort(
                IndexMappingProviderUtil.Singleton((IIndexMappingBuffer)currentBuffer.Clone()),
                _from,
                newTo);

            IIndexMappingBuffer? buffer = _currentProvider.Take();
            if (buffer is not null)
            {
                return buffer;
            }

            _currentProvider = null;
        }

        IIndexMappingBuffer? nextBuffer = _currentProvider.Take();
        if (nextBuffer is not null)
        {
            return nextBuffer;
        }

        _currentProvider = null;
        return Take();
    }
}
