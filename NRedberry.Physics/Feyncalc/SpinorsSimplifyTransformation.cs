using System.Collections.Immutable;
using NRedberry.Contexts;
using NRedberry.Graphs;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations;
using NRedberry.Transformations.Substitutions;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;
using TensorCC = NRedberry.Tensors.CC;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Port of cc.redberry.physics.feyncalc.SpinorsSimplifyTransformation.
/// </summary>
public sealed class SpinorsSimplifyTransformation : AbstractTransformationWithGammas
{
    private readonly Dictionary<Holder, Tensor> _cache = [];

    private readonly SimpleTensor? u;
    private readonly SimpleTensor? v;
    private readonly SimpleTensor? uBar;
    private readonly SimpleTensor? vBar;
    private readonly SimpleTensor momentum;
    private readonly Tensor mass;
    private readonly ITransformation uSubs;
    private readonly ITransformation vSubs;
    private readonly ITransformation uBarSubs;
    private readonly ITransformation vBarSubs;
    private readonly ITransformation p2;
    private readonly ITransformation simplifyG5;
    private readonly ITransformation ortogonality;
    private readonly ITransformation diracSimplify;

    public SpinorsSimplifyTransformation(SpinorsSimplifyOptions options)
        : base(options)
    {
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(options.Momentum);
        ArgumentNullException.ThrowIfNull(options.Mass);

        CheckSpinorNotation(options.U, false);
        CheckSpinorNotation(options.V, false);
        CheckSpinorNotation(options.UBar, true);
        CheckSpinorNotation(options.VBar, true);

        u = options.U;
        v = options.V;
        uBar = options.UBar;
        vBar = options.VBar;
        momentum = options.Momentum;
        mass = options.Mass;

        uSubs = CreateSubs(u, false);
        uBarSubs = CreateBarSubs(uBar, false);
        vSubs = CreateSubs(v, true);
        vBarSubs = CreateBarSubs(vBar, true);
        p2 = CreateP2Subs();
        simplifyG5 = options.Gamma5 is null
            ? Transformation.Identity
            : new SimplifyGamma5Transformation(options);
        diracSimplify = options.DoDiracSimplify
            ? new DiracSimplifyTransformation(options)
            : Transformation.Identity;

        List<ITransformation> ortho = [];
        Expression[]? ort = CreateOrtIdentities(uBar, v);
        if (ort is not null)
        {
            ortho.AddRange(ort);
        }

        ort = CreateOrtIdentities(vBar, u);
        if (ort is not null)
        {
            ortho.AddRange(ort);
        }

        ortogonality = new TransformationCollection(ortho);
    }

    public string ToString(OutputFormat outputFormat)
    {
        _ = outputFormat;
        return "SpinorsSimplify";
    }

    private void CheckSpinorNotation(SimpleTensor? spinor, bool bar)
    {
        if (spinor is null)
        {
            return;
        }

        Indices.Indices matrixIndices = spinor.Indices.GetOfType(MatrixType);
        if (matrixIndices.Size() != 1 || bar == IndicesUtils.GetState(matrixIndices[0]))
        {
            throw new ArgumentException("Illegal notation for spinor " + spinor);
        }
    }

    private ITransformation CreateSubs(SimpleTensor? spinor, bool negate)
    {
        if (spinor is null)
        {
            return Transformation.Identity;
        }

        int dummy = IndicesUtils.SetState(false, spinor.Indices[MatrixType, 0]);
        int free = IndicesUtils.SetState(true, dummy + 1);
        SimpleTensor gamma = TensorApi.SimpleTensor(
            GammaName,
            IndicesFactory.CreateSimple(
                null,
                free,
                dummy,
                IndicesUtils.SetType(MetricType, 0)));
        SimpleTensor mom = SetMetricIndices(
            momentum,
            IndicesFactory.CreateSimple(
                null,
                IndicesUtils.SetState(true, IndicesUtils.SetType(MetricType, 0))));
        SimpleTensor rhs = SetMatrixIndices0(spinor, free);
        Tensor right = negate
            ? TensorApi.Negate(TensorApi.Multiply(mass, rhs))
            : TensorApi.Multiply(mass, rhs);
        return TensorApi.Expression(TensorApi.Multiply(spinor, gamma, mom), right);
    }

    private ITransformation CreateBarSubs(SimpleTensor? spinor, bool negate)
    {
        if (spinor is null)
        {
            return Transformation.Identity;
        }

        int dummy = spinor.Indices[MatrixType, 0];
        int free = dummy + 1;
        SimpleTensor gamma = TensorApi.SimpleTensor(
            GammaName,
            IndicesFactory.CreateSimple(
                null,
                IndicesUtils.SetState(true, dummy),
                free,
                IndicesUtils.SetType(MetricType, 0)));
        SimpleTensor mom = SetMetricIndices(
            momentum,
            IndicesFactory.CreateSimple(
                null,
                IndicesUtils.SetState(true, IndicesUtils.SetType(MetricType, 0))));
        SimpleTensor rhs = SetMatrixIndices0(spinor, free);
        Tensor right = negate
            ? TensorApi.Negate(TensorApi.Multiply(mass, rhs))
            : TensorApi.Multiply(mass, rhs);
        return TensorApi.Expression(TensorApi.Multiply(spinor, gamma, mom), right);
    }

    private ITransformation CreateP2Subs()
    {
        return TensorApi.Expression(
            TensorApi.Multiply(momentum, SetMetricIndices(momentum, momentum.Indices.GetInverted())),
            TensorApi.Pow(mass, 2));
    }

    private Expression[]? CreateOrtIdentities(SimpleTensor? left, SimpleTensor? right)
    {
        if (left is null || right is null)
        {
            return null;
        }

        int dummy = IndicesUtils.SetState(false, left.Indices[MatrixType, 0]);
        Tensor lhs0 = TensorApi.Multiply(
            SetMatrixIndices0(left, dummy),
            SetMatrixIndices0(right, IndicesUtils.InverseIndexState(dummy)));
        Tensor lhs1 = TensorApi.Multiply(
            SetMatrixIndices0(left, dummy),
            TensorApi.SimpleTensor(
                GammaName,
                IndicesFactory.CreateSimple(
                    null,
                    IndicesUtils.InverseIndexState(dummy),
                    dummy + 1,
                    IndicesUtils.SetType(MetricType, 0))),
            SetMetricIndices(
                momentum,
                IndicesFactory.CreateSimple(
                    null,
                    IndicesUtils.SetState(true, IndicesUtils.SetType(MetricType, 0)))),
            SetMatrixIndices0(right, IndicesUtils.InverseIndexState(dummy + 1)));

        return
        [
            TensorApi.Expression(lhs0, Complex.Zero),
            TensorApi.Expression(lhs1, Complex.Zero)
        ];
    }

    private static SimpleTensor SetMetricIndices(SimpleTensor tensor, Indices.Indices indices)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(indices);

        int[] newIndices = tensor.Indices.AllIndices.ToArray();
        int j = 0;
        for (int i = 0; i < newIndices.Length; ++i)
        {
            if (TensorCC.IsMetric(IndicesUtils.GetType_(newIndices[i])))
            {
                newIndices[i] = indices[j++];
            }
        }

        return TensorApi.SimpleTensor(tensor.Name, IndicesFactory.CreateSimple(null, newIndices));
    }

    private SimpleTensor SetMatrixIndices0(SimpleTensor tensor, params int[] indices)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(indices);

        int[] newIndices = new int[tensor.Indices.Size()];
        int j = 0;
        for (int i = 0; i < tensor.Indices.Size(); ++i)
        {
            if (IndicesUtils.GetType_(tensor.Indices[i]) == MatrixType.GetType_())
            {
                newIndices[i] = indices[j++];
            }
            else
            {
                newIndices[i] = tensor.Indices[i];
            }
        }

        return TensorApi.SimpleTensor(tensor.Name, IndicesFactory.CreateSimple(null, newIndices));
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        tensor = ortogonality.Transform(tensor);
        SubstitutionIterator iterator = new(tensor);
        Tensor current;
        while ((current = iterator.Next()) is not null)
        {
            if (current is not Product product)
            {
                continue;
            }

            if (current.Indices.Size(MatrixType) == 0)
            {
                continue;
            }

            if (!ContainsGammaOr5Matrices(current))
            {
                continue;
            }

            current = simplifyG5.Transform(current);
            product = (Product)current;
            ProductContent productContent = product.Content;
            StructureOfContractions structure = productContent.StructureOfContractions;

            List<int> changed = [];
            PrimitiveSubgraph[] partition = PrimitiveSubgraphPartition.CalculatePartition(productContent, MatrixType);

            List<Tensor> simplified = [];
            List<int> matched = [];
            List<int> momentums = [];

            foreach (PrimitiveSubgraph subgraph in partition)
            {
                matched.Clear();
                if (subgraph.GraphType != GraphType.Line)
                {
                    continue;
                }

                SpinorType? left = IsSpinor(productContent[subgraph.GetPosition(0)]);
                SpinorType? right = IsSpinor(productContent[subgraph.GetPosition(subgraph.Size - 1)]);
                if (left is null && right is null)
                {
                    continue;
                }

                momentums.Clear();
                for (int i = 0; i < subgraph.Size; ++i)
                {
                    if (!IsGamma(productContent[subgraph.GetPosition(i)]))
                    {
                        continue;
                    }

                    int momentumIndex = WithMomentum(subgraph.GetPosition(i), productContent, structure);
                    if (momentumIndex == -1)
                    {
                        continue;
                    }

                    Tensor contraction = productContent[momentumIndex];
                    if (contraction.Indices.Size(MatrixType) == 0)
                    {
                        momentums.Add(productOfGammasOffset(product) + momentumIndex);
                        if (IndexMappings.AnyMappingExists(momentum, contraction))
                        {
                            matched.Add(i);
                        }
                    }
                }

                if (matched.Count == 0)
                {
                    continue;
                }

                matched.Sort();

                int gammaSize = subgraph.Size;
                Tensor spinors = product.Select(momentums.ToArray());
                if (left is not null)
                {
                    spinors = TensorApi.Multiply(spinors, productContent[subgraph.GetPosition(0)]);
                    --gammaSize;
                }

                if (right is not null)
                {
                    spinors = TensorApi.Multiply(spinors, productContent[subgraph.GetPosition(subgraph.Size - 1)]);
                    --gammaSize;
                }

                Tensor? moved = null;
                if (right is null
                    || (left is not null && matched[0] < subgraph.Size - matched[^1]))
                {
                    Tensor[] gammas = new Tensor[gammaSize];
                    int i = 1;
                    for (; i <= matched[0]; ++i)
                    {
                        Tensor tensorAtPosition = productContent[subgraph.GetPosition(i)];
                        if (!IsGammaOrGamma5(tensorAtPosition))
                        {
                            gammas = [];
                            break;
                        }

                        gammas[i - 1] = tensorAtPosition;
                    }

                    if (gammas.Length != 0)
                    {
                        for (; i <= gammaSize; ++i)
                        {
                            gammas[i - 1] = productContent[subgraph.GetPosition(i)];
                        }

                        moved = Move(gammas, matched[0] - 1, true);
                        moved = moved is Sum movedSum
                            ? FastTensors.MultiplySumElementsOnFactorAndResolveDummies(movedSum, spinors)
                            : TensorApi.Multiply(moved, spinors);

                        moved = left == SpinorType.UBar
                            ? uBarSubs.Transform(moved)
                            : vBarSubs.Transform(moved);
                        simplified.Add(moved);

                        changed.AddRange(momentums);
                        for (int i2 = 0; i2 < subgraph.Size; ++i2)
                        {
                            changed.Add(productOfGammasOffset(product) + subgraph.GetPosition(i2));
                        }
                    }
                }

                if (moved is null)
                {
                    Tensor[] gammas = new Tensor[gammaSize];
                    int leftOffset = left is null ? 0 : 1;
                    int i = subgraph.Size - 2;
                    for (; i >= matched[^1]; --i)
                    {
                        Tensor tensorAtPosition = productContent[subgraph.GetPosition(i)];
                        if (!IsGammaOrGamma5(tensorAtPosition))
                        {
                            gammas = [];
                            break;
                        }

                        gammas[i - leftOffset] = tensorAtPosition;
                    }

                    if (gammas.Length != 0)
                    {
                        for (; i >= leftOffset; --i)
                        {
                            gammas[i - leftOffset] = productContent[subgraph.GetPosition(i)];
                        }

                        moved = Move(gammas, matched[^1] - leftOffset, false);
                        moved = moved is Sum movedSum
                            ? FastTensors.MultiplySumElementsOnFactor(movedSum, spinors)
                            : TensorApi.Multiply(moved, spinors);

                        moved = right == SpinorType.U
                            ? uSubs.Transform(moved)
                            : vSubs.Transform(moved);
                        simplified.Add(moved);

                        changed.AddRange(momentums);
                        for (int i2 = 0; i2 < subgraph.Size; ++i2)
                        {
                            changed.Add(productOfGammasOffset(product) + subgraph.GetPosition(i2));
                        }
                    }
                }
            }

            if (changed.Count == 0)
            {
                continue;
            }

            simplified.Add(product.Remove(changed.ToArray()));
            Tensor simple = ExpandAndEliminate.Transform(TensorApi.MultiplyAndRenameConflictingDummies(simplified));
            simple = diracSimplify.Transform(simple);
            simple = TraceOfOne.Transform(simple);
            simple = DeltaTrace.Transform(simple);
            simple = p2.Transform(simple);
            iterator.SafeSet(Transform(simple));
        }

        return iterator.Result();
    }

    private Tensor Move(Tensor[] gammas, int index, bool left)
    {
        if (gammas.Length == 1)
        {
            return gammas[0];
        }

        if ((index == 0 && left) || (index == gammas.Length - 1 && !left))
        {
            return TensorApi.Multiply(gammas);
        }

        Tensor gammaPart;
        Tensor rest;
        if (left)
        {
            gammaPart = Move0(gammas[..(index + 1)], index, true);
            rest = TensorApi.Multiply(gammas[(index + 1)..]);
        }
        else
        {
            gammaPart = Move0(gammas[index..], 0, false);
            rest = TensorApi.Multiply(gammas[..index]);
        }

        if (gammaPart is Sum sum)
        {
            gammaPart = FastTensors.MultiplySumElementsOnFactorAndResolveDummies(sum, rest);
        }
        else
        {
            gammaPart = TensorApi.MultiplyAndRenameConflictingDummies(gammaPart, rest);
        }

        return EliminateMetricsTransformation.Eliminate(gammaPart);
    }

    private Tensor Move0(Tensor[] gammas, int index, bool left)
    {
        if (gammas.Length == 1)
        {
            return gammas[0];
        }

        if ((index == 0 && left) || (index == gammas.Length - 1 && !left))
        {
            return TensorApi.Multiply(gammas);
        }

        int numberOfGammas = gammas.Length;
        List<int> iFrom = new(numberOfGammas + 2);
        List<int> iTo = new(numberOfGammas + 2);
        List<int> g5s = [];

        for (int i = 0; i < numberOfGammas; ++i)
        {
            if (IsGamma5(gammas[i]))
            {
                g5s.Add(i);
            }
            else
            {
                iFrom.Add(IndicesUtils.SetType(MetricType, i));
                iTo.Add(gammas[i].Indices[MetricType, 0]);
            }
        }

        iFrom.Add(IndicesUtils.SetState(true, IndicesUtils.SetType(MatrixType, 0)));
        iTo.Add(gammas[0].Indices.GetOfType(MatrixType).UpperIndices[0]);
        iFrom.Add(IndicesUtils.SetType(MatrixType, numberOfGammas));
        iTo.Add(gammas[numberOfGammas - 1].Indices.GetOfType(MatrixType).LowerIndices[0]);

        Holder key = new(index, numberOfGammas, [..g5s], left);
        if (!_cache.TryGetValue(key, out Tensor? tensor))
        {
            tensor = left
                ? ToLeft0(CreateLine(numberOfGammas, g5s), index)
                : ToRight0(CreateLine(numberOfGammas, g5s), index);
            _cache[key] = tensor;
        }

        return EliminateMetricsTransformation.Eliminate(
            ApplyIndexMapping.ApplyIndexMappingAutomatically(
                tensor,
                new Mapping(iFrom.ToArray(), iTo.ToArray())));
    }

    private Tensor ToLeft0(Tensor[] gammas, int index)
    {
        if (index == 0)
        {
            return TensorApi.Multiply(gammas);
        }

        if (gammas.Length == 1)
        {
            return gammas[0];
        }

        if (IsGamma5(gammas[index]) && IsGamma5(gammas[index - 1]))
        {
            SwapAdj(gammas, index - 1);
            return ToLeft0(gammas, index - 1);
        }

        if (IsGamma5(gammas[index]) || IsGamma5(gammas[index - 1]))
        {
            SwapAdj(gammas, index - 1);
            return TensorApi.Negate(ToLeft0(gammas, index - 1));
        }

        SumBuilder builder = new();

        Tensor metric = TensorApi.Multiply(
            Complex.Two,
            Context.Get().CreateMetricOrKronecker(
                gammas[index - 1].Indices[MetricType, 0],
                gammas[index].Indices[MetricType, 0]));
        Tensor[] cutAdjacent = CutAdj(gammas, index - 1);
        Tensor adjacent;
        if (cutAdjacent.Length == 0)
        {
            adjacent = Context.Get().CreateMetricOrKronecker(
                gammas[index - 1].Indices.GetOfType(MatrixType).UpperIndices[0],
                gammas[index].Indices.GetOfType(MatrixType).LowerIndices[0]);
        }
        else if (cutAdjacent.Length == 1)
        {
            adjacent = cutAdjacent[0];
        }
        else
        {
            adjacent = TensorApi.Multiply(cutAdjacent);
        }

        adjacent = adjacent is Sum adjacentSum
            ? FastTensors.MultiplySumElementsOnFactor(adjacentSum, metric)
            : TensorApi.Multiply(adjacent, metric);
        builder.Put(adjacent);

        SwapAdj(gammas, index - 1);
        builder.Put(TensorApi.Negate(Move0(gammas, index - 1, true)));
        return builder.Build();
    }

    private Tensor ToRight0(Tensor[] gammas, int index)
    {
        if (index == gammas.Length - 1)
        {
            return TensorApi.Multiply(gammas);
        }

        if (gammas.Length == 1)
        {
            return gammas[0];
        }

        if (IsGamma5(gammas[index]) && IsGamma5(gammas[index + 1]))
        {
            SwapAdj(gammas, index);
            return ToRight0(gammas, index + 1);
        }

        if (IsGamma5(gammas[index]) || IsGamma5(gammas[index + 1]))
        {
            SwapAdj(gammas, index);
            return TensorApi.Negate(ToRight0(gammas, index + 1));
        }

        SumBuilder builder = new();

        Tensor metric = TensorApi.Multiply(
            Complex.Two,
            Context.Get().CreateMetricOrKronecker(
                gammas[index].Indices[MetricType, 0],
                gammas[index + 1].Indices[MetricType, 0]));
        Tensor[] cutAdjacent = CutAdj(gammas, index);
        Tensor adjacent;
        if (cutAdjacent.Length == 0)
        {
            adjacent = Context.Get().CreateMetricOrKronecker(
                gammas[index].Indices.GetOfType(MatrixType).UpperIndices[0],
                gammas[index + 1].Indices.GetOfType(MatrixType).LowerIndices[0]);
        }
        else if (cutAdjacent.Length == 1)
        {
            adjacent = cutAdjacent[0];
        }
        else
        {
            adjacent = TensorApi.Multiply(cutAdjacent);
        }

        adjacent = adjacent is Sum adjacentSum
            ? FastTensors.MultiplySumElementsOnFactor(adjacentSum, metric)
            : TensorApi.Multiply(adjacent, metric);
        builder.Put(adjacent);

        SwapAdj(gammas, index);
        builder.Put(TensorApi.Negate(Move0(gammas, index + 1, false)));
        return builder.Build();
    }

    private Tensor[] CreateLine(int length, List<int> gamma5Positions)
    {
        Tensor[] gammas = new Tensor[length];
        int matrixIndex = IndicesUtils.SetType(MatrixType, 0);
        int upper = matrixIndex;
        int metricIndex = 0;
        int j = 0;
        for (int i = 0; i < length; ++i)
        {
            if (j < gamma5Positions.Count && gamma5Positions[j] == i)
            {
                gammas[i] = TensorApi.SimpleTensor(
                    Gamma5Name,
                    IndicesFactory.CreateSimple(
                        null,
                        IndicesUtils.SetState(true, upper),
                        upper = ++matrixIndex));
                ++j;
            }
            else
            {
                gammas[i] = TensorApi.SimpleTensor(
                    GammaName,
                    IndicesFactory.CreateSimple(
                        null,
                        IndicesUtils.SetState(true, upper),
                        upper = ++matrixIndex,
                        IndicesUtils.SetType(MetricType, metricIndex++)));
            }
        }

        return gammas;
    }

    private int WithMomentum(int gamma, ProductContent productContent, StructureOfContractions structure)
    {
        Indices.Indices indices = productContent[gamma].Indices;
        int j = 0;
        for (; j < indices.Size(); ++j)
        {
            if (MetricType.GetType_() == IndicesUtils.GetType_(indices[j]))
            {
                break;
            }
        }

        return StructureOfContractions.GetToTensorIndex(structure.contractions[gamma][j]);
    }

    private SpinorType? IsSpinor(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (u is not null && IndexMappings.AnyMappingExists(tensor, u))
        {
            return SpinorType.U;
        }

        if (v is not null && IndexMappings.AnyMappingExists(tensor, v))
        {
            return SpinorType.V;
        }

        if (uBar is not null && IndexMappings.AnyMappingExists(tensor, uBar))
        {
            return SpinorType.UBar;
        }

        if (vBar is not null && IndexMappings.AnyMappingExists(tensor, vBar))
        {
            return SpinorType.VBar;
        }

        return null;
    }

    private static int productOfGammasOffset(Product product)
    {
        return product.IndexlessData.Length + (product.Factor == Complex.One ? 0 : 1);
    }

    private enum SpinorType
    {
        U,
        V,
        UBar,
        VBar
    }

    private sealed class Holder(int index, int length, ImmutableArray<int> gamma5Positions, bool left)
        : IEquatable<Holder>
    {
        public int Index { get; } = index;

        public int Length { get; } = length;

        public ImmutableArray<int> Gamma5Positions { get; } = gamma5Positions;

        public bool Left { get; } = left;

        public bool Equals(Holder? other)
        {
            return other is not null
                && Index == other.Index
                && Length == other.Length
                && Left == other.Left
                && Gamma5Positions.AsSpan().SequenceEqual(other.Gamma5Positions.AsSpan());
        }

        public override bool Equals(object? obj)
        {
            return obj is Holder other && Equals(other);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Index);
            hashCode.Add(Length);
            hashCode.Add(Left);
            foreach (int position in Gamma5Positions)
            {
                hashCode.Add(position);
            }

            return hashCode.ToHashCode();
        }
    }
}
