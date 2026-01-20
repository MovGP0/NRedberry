using NRedberry.Contexts;
using NRedberry.Groups;
using NRedberry.IndexGeneration;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Concurrent;
using NRedberry.Tensors;
using NRedberry.Transformations;
using NRedberry.Transformations.Expand;
using NRedberry.Transformations.Powerexpand;
using NRedberry.Transformations.Symmetrization;
using TensorOps = NRedberry.Tensors.Tensors;

namespace NRedberry.Transformations.Collect;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.collect.CollectTransformation.
/// </summary>
public sealed class CollectTransformation : ITransformation
{
    private readonly HashSet<int> _patternsNames;
    private readonly ITransformation _powerExpand;
    private readonly ITransformation[] _transformations;
    private readonly bool _expandSymbolic;

    public CollectTransformation(SimpleTensor[] patterns, ITransformation[] transformations, bool expandSymbolic)
    {
        ArgumentNullException.ThrowIfNull(patterns);
        ArgumentNullException.ThrowIfNull(transformations);

        _patternsNames = new HashSet<int>();
        _powerExpand = new PowerUnfoldTransformation(patterns);
        foreach (SimpleTensor t in patterns)
        {
            _patternsNames.Add(t.Name);
        }

        _transformations = transformations;
        _expandSymbolic = expandSymbolic;
    }

    public CollectTransformation(SimpleTensor[] patterns, ITransformation[] transformations)
        : this(patterns, transformations, true)
    {
    }

    public CollectTransformation(SimpleTensor[] patterns)
        : this(patterns, [])
    {
    }

    public CollectTransformation(SimpleTensor[] patterns, CollectOptions options)
        : this(patterns, [options.Simplifications ?? throw new ArgumentNullException(nameof(options))], options.ExpandSymbolic)
    {
    }

    public CollectTransformation(SimpleTensor[] patterns, bool expandSymbolic)
        : this(patterns, [], expandSymbolic)
    {
    }

    public Tensor Transform(Tensor t)
    {
        ArgumentNullException.ThrowIfNull(t);

        return t is Expression ? Transformation.ApplyToEachChild(t, this) : Transform1(t);
    }

    public sealed class CollectOptions
    {
        public ITransformation Simplifications { get; set; } = Transformation.Identity;
        public bool ExpandSymbolic { get; set; } = true;
    }

    private Tensor Transform1(Tensor t)
    {
        SumBuilder notMatched = new();
        Dictionary<int, List<Split>> map = new();
        IOutputPort<Tensor> port = ExpandPort.CreatePort(t, _expandSymbolic);
        Tensor? current;
        while ((current = port.Take()) is not null)
        {
            Split toAdd = SplitTerm(current);
            if (toAdd.Factors.Length == 0)
            {
                notMatched.Put(current);
                continue;
            }

            if (!map.TryGetValue(toAdd.HashCode, out List<Split>? nodes))
            {
                nodes = new List<Split> { toAdd };
                map[toAdd.HashCode] = nodes;
                continue;
            }

            bool matched = false;
            foreach (Split baseSplit in nodes)
            {
                int[]? match = MatchFactors(baseSplit.Factors, toAdd.Factors);
                if (match is null)
                {
                    continue;
                }

                Tensor[] toAddFactors = Permutations.Permute(toAdd.Factors, match);
                Mapping? mapping = IndexMappings.CreateBijectiveProductPort(toAddFactors, baseSplit.Factors).Take();
                if (mapping is null)
                {
                    continue;
                }

                baseSplit.Summands.Add(
                    ApplyIndexMapping.ApplyIndexMappingAutomatically(toAdd.Summands[0], mapping, baseSplit.Forbidden));
                matched = true;
                break;
            }

            if (!matched)
            {
                nodes.Add(toAdd);
            }
        }

        Tensor reduced = Transformation.ApplySequentially(notMatched.Build(), _transformations);
        notMatched = new SumBuilder();
        notMatched.Put(reduced);
        foreach (List<Split> splits in map.Values)
        {
            foreach (Split split in splits)
            {
                notMatched.Put(split.ToTensor(_transformations));
            }
        }

        return notMatched.Build();
    }

    private bool Match(Tensor t)
    {
        if (t is SimpleTensor)
        {
            return _patternsNames.Contains(t.GetHashCode());
        }

        if (TensorUtils.IsPositiveIntegerPower(t))
        {
            return _patternsNames.Contains(t[0].GetHashCode());
        }

        return false;
    }

    private Split SplitTerm(Tensor tensor)
    {
        Tensor[] factors;
        Tensor summand;

        if (tensor is SimpleTensor || TensorUtils.IsPositiveIntegerPowerOfSimpleTensor(tensor))
        {
            if (Match(tensor))
            {
                factors = new[] { tensor };
                summand = Complex.One;
            }
            else
            {
                return new Split(Array.Empty<Tensor>(), tensor);
            }
        }
        else if (tensor is Product || TensorUtils.IsPositiveIntegerPowerOfProduct(tensor))
        {
            tensor = _powerExpand.Transform(tensor);

            bool containsMatch = false;
            if (tensor is not Product productElements)
            {
                return new Split(Array.Empty<Tensor>(), tensor);
            }

            foreach (Tensor t in productElements)
            {
                if (Match(t))
                {
                    containsMatch = true;
                    break;
                }
            }

            if (!containsMatch)
            {
                return new Split(Array.Empty<Tensor>(), tensor);
            }

            Product product = productElements;

            List<Tensor> factorsList = [];
            summand = tensor;
            foreach (Tensor t in product)
            {
                if (!Match(t))
                {
                    continue;
                }

                factorsList.Add(t);
                if (summand is Product summandProduct)
                {
                    summand = summandProduct.Remove(t);
                }
                else
                {
                    summand = Complex.One;
                }
            }

            factors = factorsList.ToArray();
        }
        else
        {
            return new Split(Array.Empty<Tensor>(), tensor);
        }

        HashSet<int> freeIndices = new(IndicesUtils.GetIndicesNames(tensor.Indices.GetFree()));

        Indices.Indices factorIndices = new IndicesBuilder().Append(factors).Indices;
        HashSet<int> dummies = new(
            IndicesUtils.GetIntersections(
                factorIndices.UpperIndices.ToArray(),
                factorIndices.LowerIndices.ToArray()));

        List<Tensor> kroneckers = [];
        IndexGenerator generator = new(TensorUtils.GetAllIndicesNamesT(tensor).ToArray());
        for (int i = 0; i < factors.Length; ++i)
        {
            List<int> from = [];
            List<int> to = [];
            SimpleIndices currentFactorIndices = IndicesFactory.CreateSimple(null, factors[i].Indices);

            for (int j = currentFactorIndices.Size() - 1; j >= 0; --j)
            {
                int index = currentFactorIndices[j];
                if (freeIndices.Contains(IndicesUtils.GetNameWithType(index)))
                {
                    int newIndex = IndicesUtils.SetRawState(
                        IndicesUtils.GetRawStateInt(index),
                        generator.Generate(IndicesUtils.GetType(index)));
                    from.Add(index);
                    to.Add(newIndex);
                    kroneckers.Add(Context.Get().CreateKronecker(index, IndicesUtils.InverseIndexState(newIndex)));
                }
                else if (IndicesUtils.GetState(index) && dummies.Contains(IndicesUtils.GetNameWithType(index)))
                {
                    int newIndex = IndicesUtils.SetRawState(
                        IndicesUtils.GetRawStateInt(index),
                        generator.Generate(IndicesUtils.GetType(index)));
                    from.Add(index);
                    to.Add(newIndex);
                    kroneckers.Add(Context.Get().CreateKronecker(index, IndicesUtils.InverseIndexState(newIndex)));
                }
            }

            factors[i] = ApplyDirectMapping(factors[i], new StateSensitiveMapping(from.ToArray(), to.ToArray()));
        }

        kroneckers.Add(summand);
        summand = TensorOps.Multiply(kroneckers.ToArray());
        summand = EliminateMetricsTransformation.Eliminate(summand);

        return new Split(factors, summand);
    }

    private static int[]? MatchFactors(Tensor[] a, Tensor[] b)
    {
        if (a.Length != b.Length)
        {
            return null;
        }

        int begin = 0;
        int length = a.Length;
        int[] permutation = new int[length];
        Array.Fill(permutation, -1);

        for (int i = 1; i <= length; ++i)
        {
            if (i == length || a[i].GetHashCode() != b[i - 1].GetHashCode())
            {
                if (i - 1 != begin)
                {
                    for (int n = begin; n < i; ++n)
                    {
                        bool matched = false;
                        for (int j = begin; j < i; ++j)
                        {
                            if (permutation[j] == -1 && MatchSimpleTensors(a[n], b[j]))
                            {
                                permutation[j] = n;
                                matched = true;
                                break;
                            }
                        }

                        if (!matched)
                        {
                            return null;
                        }
                    }
                }
                else
                {
                    if (!MatchSimpleTensors(a[i - 1], b[i - 1]))
                    {
                        return null;
                    }

                    permutation[i - 1] = i - 1;
                }

                begin = i;
            }
        }

        return Permutations.Inverse(permutation);
    }

    private static bool MatchSimpleTensors(Tensor a, Tensor b)
    {
        if (a.GetType() != b.GetType())
        {
            return false;
        }

        if (a.GetHashCode() != b.GetHashCode())
        {
            return false;
        }

        if (TensorUtils.IsPositiveIntegerPowerOfSimpleTensor(a))
        {
            return TensorUtils.IsPositiveIntegerPowerOfSimpleTensor(b)
                && a[1].Equals(b[1])
                && MatchSimpleTensors(a[0], b[0]);
        }

        if (a is TensorField)
        {
            for (int i = a.Size - 1; i >= 0; --i)
            {
                if (!IndexMappings.PositiveMappingExists(a[i], b[i]))
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static Tensor ApplyDirectMapping(Tensor t, DirectIndexMapping mapping)
    {
        if (t is SimpleTensor st)
        {
            SimpleIndices newIndices = (SimpleIndices)st.SimpleIndices.ApplyIndexMapping(mapping);
            if (t is TensorField field)
            {
                return new TensorField(field.Name, newIndices, field.Arguments, field.ArgumentIndices);
            }

            return new SimpleTensor(st.Name, newIndices);
        }

        return t;
    }

    private sealed class Split
    {
        public Split(Tensor[] factors, Tensor summand)
        {
            Factors = factors;
            Summands = new List<Tensor> { summand };
            Array.Sort(Factors);
            HashCode = GetArrayHashCode(Factors);
            Forbidden = IndicesUtils.GetIndicesNames(new IndicesBuilder().Append(Factors).Indices);
        }

        public Tensor[] Factors { get; }
        public List<Tensor> Summands { get; }
        public int HashCode { get; }
        public int[] Forbidden { get; }

        public Tensor ToTensor(ITransformation[] transformations)
        {
            Tensor sum = Transformation.ApplySequentially(
                TensorOps.Sum(Summands.ToArray()),
                transformations);
            Tensor[] ms = new Tensor[Factors.Length + 1];
            ms[^1] = sum;
            Array.Copy(Factors, 0, ms, 0, Factors.Length);
            return TensorOps.Multiply(ms);
        }

        public override string ToString()
        {
            return $"{TensorOps.Multiply(Factors)} : {TensorOps.Sum(Summands.ToArray())}";
        }

        private static int GetArrayHashCode(Tensor[] items)
        {
            int result = 1;
            foreach (Tensor item in items)
            {
                result = unchecked(result * 31 + item.GetHashCode());
            }

            return result;
        }
    }

    private abstract class DirectIndexMapping : IIndexMapping
    {
        protected DirectIndexMapping(int[] from, int[] to)
        {
            Array.Sort(from, to);
            From = from;
            To = to;
        }

        protected int[] From { get; }
        protected int[] To { get; }

        public abstract int Map(int from);
    }

    private sealed class StateSensitiveMapping : DirectIndexMapping
    {
        public StateSensitiveMapping(int[] from, int[] to)
            : base(from, to)
        {
        }

        public override int Map(int from)
        {
            int index = Array.BinarySearch(From, from);
            if (index >= 0)
            {
                return To[index];
            }

            return from;
        }
    }
}
