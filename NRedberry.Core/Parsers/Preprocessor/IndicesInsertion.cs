using NRedberry.Core.Utils;
using NRedberry.IndexGeneration;
using NRedberry.Indices;
using MathsUtils = NRedberry.Maths.MathUtils;

namespace NRedberry.Parsers.Preprocessor;

/// <summary>
/// AST transformer which inserts additional indices to specified tensors.
/// Mirrors cc.redberry.core.parser.preprocessor.IndicesInsertion.
/// </summary>
public sealed class IndicesInsertion : IParseTokenTransformer
{
    private readonly int[] _upper;
    private readonly int[] _lower;
    private readonly IIndicator<ParseTokenSimpleTensor> _indicator;

    public IndicesInsertion(SimpleIndices upper, SimpleIndices lower, IIndicator<ParseTokenSimpleTensor> indicator)
    {
        ArgumentNullException.ThrowIfNull(upper);
        ArgumentNullException.ThrowIfNull(lower);
        ArgumentNullException.ThrowIfNull(indicator);

        CheckIndices(upper, lower);

        int[] upperArray = new int[upper.Size()];
        for (int i = upper.Size() - 1; i >= 0; --i)
        {
            upperArray[i] = IndicesUtils.GetNameWithType(upper[i]);
        }

        _upper = upperArray;
        _lower = lower.AllIndices.ToArray();
        _indicator = indicator;
    }

    public ParseToken Transform(ParseToken node)
    {
        ArgumentNullException.ThrowIfNull(node);

        int[] freeIndices = node.GetIndices().GetFree().AllIndices.ToArray();
        for (int i = 0; i < freeIndices.Length; ++i)
        {
            freeIndices[i] = IndicesUtils.GetNameWithType(freeIndices[i]);
        }

        Array.Sort(freeIndices);
        for (int i = _upper.Length - 1; i >= 0; --i)
        {
            if (Array.BinarySearch(freeIndices, _upper[i]) >= 0)
            {
                throw new ArgumentException("Inconsistent indices.");
            }
        }

        for (int i = _lower.Length - 1; i >= 0; --i)
        {
            if (Array.BinarySearch(freeIndices, _lower[i]) >= 0)
            {
                throw new ArgumentException("Inconsistent indices.");
            }
        }

        ISet<int> dummyIndices = ParseUtils.GetAllIndices(node);

        int[] upper = (int[])_upper.Clone();
        int[] lower = (int[])_lower.Clone();
        Array.Sort(upper);
        Array.Sort(lower);
        int[] upperLower = MathsUtils.IntSetUnion(upper, lower);

        int[] forbidden = new int[dummyIndices.Count + upperLower.Length];
        int position = 0;
        foreach (int index in dummyIndices)
        {
            forbidden[position++] = index;
        }

        Array.Copy(upperLower, 0, forbidden, dummyIndices.Count, upperLower.Length);

        IndexGenerator generator = new(forbidden);
        List<int> from = [];
        List<int> to = [];
        for (int i = upperLower.Length - 1; i >= 0; --i)
        {
            int fromIndex = upperLower[i];
            if (dummyIndices.Contains(fromIndex))
            {
                from.Add(fromIndex);
                to.Add(generator.Generate(IndicesUtils.GetType(fromIndex)));
            }
        }

        int[] fromArray = from.ToArray();
        int[] toArray = to.ToArray();
        ArraysUtils.QuickSort(fromArray, toArray);

        IIndicesInsertionTransformer? transformer = CreateTransformer(node, _indicator);
        transformer?.Apply(new IndexMapper(fromArray, toArray), new IGWrapper(generator), upper, lower);
        return node;
    }

    private static void CheckIndices(SimpleIndices upper, SimpleIndices lower)
    {
        if (upper.Size() != lower.Size())
        {
            throw new ArgumentException("Upper indices size not equal to lower indices size.");
        }

        int size = upper.Size();
        for (int i = 0; i < size; ++i)
        {
            if (!IndicesUtils.GetState(upper[i]) || IndicesUtils.GetState(lower[i]))
            {
                throw new ArgumentException();
            }

            if (IndicesUtils.GetType(upper[i]) != IndicesUtils.GetType(lower[i]))
            {
                throw new ArgumentException();
            }

            if (i != 0 && IndicesUtils.GetType(upper[i - 1]) != IndicesUtils.GetType(upper[i]))
            {
                throw new ArgumentException("Many types.");
            }
        }
    }

    private static IIndicesInsertionTransformer? CreateTransformer(ParseToken node, IIndicator<ParseTokenSimpleTensor> indicator)
    {
        switch (node.TokenType)
        {
            case TokenType.TensorField:
            case TokenType.SimpleTensor:
            {
                var simple = (ParseTokenSimpleTensor)node;
                return indicator.Is(simple) ? new SimpleTransformer(simple) : null;
            }

            case TokenType.Product:
            case TokenType.Expression:
            case TokenType.Sum:
            {
                List<IIndicesInsertionTransformer> transformers = [];
                foreach (ParseToken child in node.Content)
                {
                    IIndicesInsertionTransformer? transformer = CreateTransformer(child, indicator);
                    if (transformer is not null)
                    {
                        transformers.Add(transformer);
                    }
                }

                if (transformers.Count == 0)
                {
                    return null;
                }

                if (transformers.Count == 1)
                {
                    return transformers[0];
                }

                return node.TokenType == TokenType.Product
                    ? new ProductTransformer(transformers.ToArray())
                    : new SumTransformer(transformers.ToArray());
            }

            default:
                return null;
        }
    }
}

internal interface IIndicesInsertionTransformer
{
    void Apply(IndexMapper indexMapper, IGWrapper generator, int[] upper, int[] lower);
}

internal sealed class IndexMapper : IIndexMapping
{
    private readonly int[] _from;
    private readonly int[] _to;

    public IndexMapper(int[] from, int[] to)
    {
        _from = from;
        _to = to;
    }

    public int Map(int from)
    {
        int position = Array.BinarySearch(_from, IndicesUtils.GetNameWithType(from));
        if (position < 0)
        {
            return from;
        }

        return IndicesUtils.GetRawStateInt(from) ^ _to[position];
    }
}

internal sealed class SimpleTransformer : IIndicesInsertionTransformer
{
    private readonly ParseTokenSimpleTensor _node;

    public SimpleTransformer(ParseTokenSimpleTensor node)
    {
        _node = node;
    }

    public void Apply(IndexMapper indexMapper, IGWrapper generator, int[] upper, int[] lower)
    {
        SimpleIndices oldIndices = _node.Indices;
        int[] newIndices = new int[oldIndices.Size() + 2 * upper.Length];
        int i;
        for (i = 0; i < oldIndices.Size(); ++i)
        {
            newIndices[i] = indexMapper.Map(oldIndices[i]);
        }

        Array.Copy(upper, 0, newIndices, oldIndices.Size(), upper.Length);
        Array.Copy(lower, 0, newIndices, oldIndices.Size() + upper.Length, lower.Length);
        for (i = 0; i < upper.Length; ++i)
        {
            newIndices[i + oldIndices.Size()] |= IndicesUtils.UpperRawStateInt;
        }

        _node.Indices = IndicesFactory.CreateSimple(null, newIndices);
    }
}

internal abstract class MultiTransformer : IIndicesInsertionTransformer
{
    protected readonly IIndicesInsertionTransformer[] Transformers;

    protected MultiTransformer(IIndicesInsertionTransformer[] transformers)
    {
        Transformers = transformers;
    }

    public abstract void Apply(IndexMapper indexMapper, IGWrapper generator, int[] upper, int[] lower);
}

internal sealed class SumTransformer : MultiTransformer
{
    public SumTransformer(IIndicesInsertionTransformer[] transformers)
        : base(transformers)
    {
    }

    public override void Apply(IndexMapper indexMapper, IGWrapper generator, int[] upper, int[] lower)
    {
        IGWrapper? generatorTemp = null;
        for (int i = 0; i < Transformers.Length - 1; ++i)
        {
            IGWrapper generatorClone = generator.Clone();
            Transformers[i].Apply(indexMapper, generatorClone, upper, lower);
            if (generatorTemp is null)
            {
                generatorTemp = generatorClone;
            }
            else
            {
                generatorTemp.Merge(generatorClone);
            }
        }

        Transformers[^1].Apply(indexMapper, generator, upper, lower);
        if (generatorTemp is not null)
        {
            generator.Merge(generatorTemp);
        }
    }
}

internal sealed class ProductTransformer : MultiTransformer
{
    public ProductTransformer(IIndicesInsertionTransformer[] transformers)
        : base(transformers)
    {
    }

    public override void Apply(IndexMapper indexMapper, IGWrapper generator, int[] upper, int[] lower)
    {
        int[] tempUpper = (int[])upper.Clone();
        int[] tempLower = new int[upper.Length];
        int i;
        int j;
        for (i = 0; i < Transformers.Length - 1; ++i)
        {
            for (j = 0; j < upper.Length; ++j)
            {
                tempLower[j] = generator.Next(IndicesUtils.GetType(lower[j]));
            }

            Transformers[i].Apply(indexMapper, generator, tempUpper, tempLower);
            Array.Copy(tempLower, 0, tempUpper, 0, tempUpper.Length);
        }

        Transformers[i].Apply(indexMapper, generator, tempUpper, lower);
    }
}

internal sealed class IGWrapper
{
    private IndexGenerator _generator;
    private int _generated;

    public IGWrapper(IndexGenerator generator)
    {
        _generator = generator;
    }

    private IGWrapper(IndexGenerator generator, int generated)
    {
        _generator = generator;
        _generated = generated;
    }

    public int Next(byte type)
    {
        ++_generated;
        return _generator.Generate(type);
    }

    public void Merge(IGWrapper? wrapper)
    {
        if (wrapper is null)
        {
            return;
        }

        if (wrapper._generated > _generated)
        {
            _generated = wrapper._generated;
            _generator = wrapper._generator;
        }
    }

    public IGWrapper Clone()
    {
        return new IGWrapper(_generator.Clone(), _generated);
    }
}
