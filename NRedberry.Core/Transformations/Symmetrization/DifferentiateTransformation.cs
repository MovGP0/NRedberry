using NRedberry.Contexts;
using NRedberry.IndexGeneration;
using NRedberry.IndexMapping;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using NRedberry.Transformations.Options;
using ContextCC = NRedberry.Contexts.CC;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Port of cc.redberry.core.transformations.DifferentiateTransformation.
/// Differentiates a tensor with respect to one or more simple tensors.
/// </summary>
public sealed class DifferentiateTransformation : TransformationToStringAble
{
    private readonly SimpleTensor[] _vars;
    private readonly ITransformation[] _expandAndContract;
    private readonly bool _useDeltaFunction;

    public DifferentiateTransformation(params SimpleTensor[] vars)
        : this(true, vars)
    {
    }

    public DifferentiateTransformation(bool useDeltaFunction, params SimpleTensor[] vars)
    {
        ArgumentNullException.ThrowIfNull(vars);

        _useDeltaFunction = useDeltaFunction;
        _vars = (SimpleTensor[])vars.Clone();
        _expandAndContract = [];
    }

    public DifferentiateTransformation(SimpleTensor[] vars, ITransformation[] expandAndContract)
        : this(true, vars, expandAndContract)
    {
    }

    public DifferentiateTransformation(bool useDeltaFunction, SimpleTensor[] vars, ITransformation[] expandAndContract)
    {
        ArgumentNullException.ThrowIfNull(vars);
        ArgumentNullException.ThrowIfNull(expandAndContract);

        _useDeltaFunction = useDeltaFunction;
        _vars = (SimpleTensor[])vars.Clone();
        _expandAndContract = (ITransformation[])expandAndContract.Clone();
    }

    [Creator(Vararg = true, HasArgs = true)]
    public DifferentiateTransformation(SimpleTensor[] vars, [Options] DifferentiateOptions options)
    {
        ArgumentNullException.ThrowIfNull(vars);
        ArgumentNullException.ThrowIfNull(options);

        _useDeltaFunction = options.UseDiracDelta;
        _vars = (SimpleTensor[])vars.Clone();
        _expandAndContract = [options.Simplifications];
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        return Differentiate(tensor, _expandAndContract, _useDeltaFunction, _vars);
    }

    public string ToString(OutputFormat outputFormat)
    {
        ArgumentNullException.ThrowIfNull(outputFormat);

        if (_vars.Length == 0)
        {
            return "Differentiate[]";
        }

        var parts = new string[_vars.Length];
        for (int i = 0; i < _vars.Length; ++i)
        {
            parts[i] = _vars[i].ToString(outputFormat);
        }

        return $"Differentiate[{string.Join(",", parts)}]";
    }

    public override string ToString()
    {
        return ToString(ContextCC.DefaultOutputFormat);
    }

    public static Tensor Differentiate(Tensor tensor, SimpleTensor var, int order)
    {
        return Differentiate(tensor, var, order, false);
    }

    public static Tensor Differentiate(Tensor tensor, SimpleTensor var, int order, bool useDeltaFunction)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(var);

        if (var.Indices.Size() != 0 && order > 1)
        {
            throw new ArgumentException("Only scalar variables support higher-order derivatives.", nameof(order));
        }

        Tensor result = tensor;
        for (; order > 0; --order)
        {
            result = Differentiate(result, [], var, useDeltaFunction);
        }

        return result;
    }

    public static Tensor Differentiate(Tensor tensor, params SimpleTensor[] vars)
    {
        return Differentiate(tensor, true, vars);
    }

    public static Tensor Differentiate(Tensor tensor, bool useDeltaFunction, params SimpleTensor[] vars)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(vars);

        if (vars.Length == 0)
        {
            return tensor;
        }

        if (vars.Length == 1)
        {
            return Differentiate(tensor, [], vars[0], useDeltaFunction);
        }

        return Differentiate(tensor, [], useDeltaFunction, vars);
    }

    public static Tensor Differentiate(
        Tensor tensor,
        ITransformation[] expandAndContract,
        bool useDeltaFunction,
        params SimpleTensor[] vars)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        ArgumentNullException.ThrowIfNull(expandAndContract);
        ArgumentNullException.ThrowIfNull(vars);

        if (vars.Length == 0)
        {
            return tensor;
        }

        if (vars.Length == 1)
        {
            return Differentiate(tensor, expandAndContract, vars[0], useDeltaFunction);
        }

        bool needRename = false;
        foreach (SimpleTensor var in vars)
        {
            if (var.Indices.Size() != 0)
            {
                needRename = true;
                break;
            }
        }

        SimpleTensor[] resolvedVars = (SimpleTensor[])vars.Clone();
        if (needRename)
        {
            HashSet<int> allTensorIndices = TensorUtils.GetAllIndicesNamesT(tensor);
            HashSet<int> dummyTensorIndices = new(allTensorIndices);
            foreach (int index in tensor.Indices.GetFree().AllIndices)
            {
                dummyTensorIndices.Remove(index);
            }

            needRename = false;
            foreach (SimpleTensor var in vars)
            {
                if (ContainsIndicesNames(allTensorIndices, TensorUtils.GetAllDummyIndicesT(var).ToArray())
                    || ContainsIndicesNames(dummyTensorIndices, var.Indices))
                {
                    needRename = true;
                    break;
                }
            }

            foreach (SimpleTensor var in vars)
            {
                allTensorIndices.UnionWith(IndicesUtils.GetIndicesNames(var.Indices.GetFree()));
            }

            if (needRename)
            {
                for (int i = 0; i < vars.Length; ++i)
                {
                    if (allTensorIndices.Count == 0 || resolvedVars[i].Indices.Size() == 0)
                    {
                        continue;
                    }

                    if (resolvedVars[i].Indices.Size() != resolvedVars[i].Indices.GetFree().Size())
                    {
                        resolvedVars[i] = (SimpleTensor)ApplyIndexMapping.RenameDummy(
                            resolvedVars[i],
                            allTensorIndices.ToArray());
                    }

                    allTensorIndices.UnionWith(IndicesUtils.GetIndicesNames(resolvedVars[i].Indices));
                }

                tensor = ApplyIndexMapping.RenameDummy(
                    tensor,
                    TensorUtils.GetAllIndicesNamesT(resolvedVars).ToArray(),
                    allTensorIndices);
            }

            tensor = ApplyIndexMapping.RenameIndicesOfFieldsArguments(tensor, allTensorIndices);
        }

        foreach (SimpleTensor var in resolvedVars)
        {
            tensor = DifferentiateInternal(tensor, CreateRule(var, useDeltaFunction), expandAndContract);
        }

        return tensor;
    }

    private static Tensor Differentiate(
        Tensor tensor,
        ITransformation[] expandAndContract,
        SimpleTensor var,
        bool useDeltaFunction)
    {
        if (var.Indices.Size() != 0)
        {
            HashSet<int> allTensorIndices = TensorUtils.GetAllIndicesNamesT(tensor);
            HashSet<int> dummyTensorIndices = new(allTensorIndices);
            foreach (int index in tensor.Indices.GetFree().AllIndices)
            {
                dummyTensorIndices.Remove(index);
            }

            if (ContainsIndicesNames(allTensorIndices, TensorUtils.GetAllDummyIndicesT(var).ToArray())
                || ContainsIndicesNames(dummyTensorIndices, var.Indices))
            {
                allTensorIndices.UnionWith(IndicesUtils.GetIndicesNames(var.Indices));
                var = (SimpleTensor)ApplyIndexMapping.RenameDummy(
                    var,
                    TensorUtils.GetAllIndicesNamesT(tensor).ToArray());
                tensor = ApplyIndexMapping.RenameDummy(
                    tensor,
                    TensorUtils.GetAllIndicesNamesT(var).ToArray(),
                    allTensorIndices);
            }
            else
            {
                allTensorIndices.UnionWith(IndicesUtils.GetIndicesNames(var.Indices));
            }

            tensor = ApplyIndexMapping.RenameIndicesOfFieldsArguments(tensor, allTensorIndices);
        }

        return DifferentiateInternal(tensor, CreateRule(var, useDeltaFunction), expandAndContract);
    }

    private static bool ContainsIndicesNames(ISet<int> set, Indices.Indices indices)
    {
        foreach (int index in indices.AllIndices)
        {
            if (set.Contains(IndicesUtils.GetNameWithType(index)))
            {
                return true;
            }
        }

        return false;
    }

    private static bool ContainsIndicesNames(ISet<int> set, int[] indices)
    {
        foreach (int index in indices)
        {
            if (set.Contains(IndicesUtils.GetNameWithType(index)))
            {
                return true;
            }
        }

        return false;
    }

    private static Tensor DifferentiateWithRenaming(
        Tensor tensor,
        SimpleTensorDifferentiationRule rule,
        ITransformation[] transformations)
    {
        SimpleTensorDifferentiationRule newRule = rule.NewRuleForTensor(tensor);
        tensor = ApplyIndexMapping.RenameDummy(tensor, newRule.GetForbidden());
        return DifferentiateInternal(tensor, newRule, transformations);
    }

    private static Tensor DifferentiateSimpleTensor(
        SimpleTensor tensor,
        SimpleTensorDifferentiationRule rule,
        ITransformation[] transformations)
    {
        Tensor differentiated = rule.DifferentiateSimpleTensor(tensor);
        return ApplyTransformations(differentiated, transformations);
    }

    private static Tensor DifferentiateInternal(
        Tensor tensor,
        SimpleTensorDifferentiationRule rule,
        ITransformation[] transformations)
    {
        if (tensor is Complex)
        {
            return Complex.Zero;
        }

        if (tensor.GetType() == typeof(SimpleTensor))
        {
            return DifferentiateSimpleTensor((SimpleTensor)tensor, rule, transformations);
        }

        if (tensor.GetType() == typeof(TensorField))
        {
            TensorField field = (TensorField)tensor;
            if (rule.Var.Name == field.Name)
            {
                TensorField differentiatedVar = (TensorField)rule.Var;
                if (rule.UseDeltaFunction)
                {
                    ScalarsBackedProductBuilder builder = new();
                    builder.Put(DifferentiateSimpleTensor((SimpleTensor)tensor, rule, transformations));
                    for (int i = 0; i < differentiatedVar.Size; ++i)
                    {
                        builder.Put(CreateDiracDelta(field[i], differentiatedVar[i]));
                    }

                    return builder.Build();
                }

                if (IndexMappings.AnyMappingExists(differentiatedVar, field)
                    || IndexMappings.AnyMappingExists(field, differentiatedVar))
                {
                    return DifferentiateSimpleTensor((SimpleTensor)tensor, rule, transformations);
                }
            }

            SumBuilder result = new(tensor.Size);
            for (int i = tensor.Size - 1; i >= 0; --i)
            {
                Tensor differentiatedArg = DifferentiateInternal(field[i], rule, transformations);
                if (TensorUtils.IsZero(differentiatedArg))
                {
                    continue;
                }

                result.Put(
                    TensorApi.Multiply(
                        differentiatedArg,
                        CreateFieldDerivative(field, field.GetArgIndices(i).GetInverted(), i)));
            }

            return ApplyTransformations(
                EliminateMetricsTransformation.Eliminate(result.Build()),
                transformations);
        }

        if (tensor is Sum sum)
        {
            SumBuilder builder = new();
            foreach (Tensor child in sum)
            {
                builder.Put(ApplyTransformations(
                    DifferentiateInternal(child, rule, transformations),
                    transformations));
            }

            return builder.Build();
        }

        if (tensor is ScalarFunction function)
        {
            Tensor differentiated = TensorApi.Multiply(
                function.Derivative(),
                DifferentiateWithRenaming(function[0], rule, transformations));
            return ApplyTransformations(differentiated, transformations);
        }

        if (tensor is Power power)
        {
            Tensor differentiated = TensorApi.Sum(
                TensorApi.MultiplyAndRenameConflictingDummies(
                    power[1],
                    TensorApi.Pow(power[0], TensorApi.Sum(power[1], Complex.MinusOne)),
                    DifferentiateInternal(power[0], rule, transformations)),
                TensorApi.MultiplyAndRenameConflictingDummies(
                    power,
                    new Log(power[0]),
                    DifferentiateWithRenaming(power[1], rule, transformations)));
            return ApplyTransformations(differentiated, transformations);
        }

        if (tensor is Product)
        {
            SumBuilder result = new();
            for (int i = tensor.Size - 1; i >= 0; --i)
            {
                Tensor differentiated = tensor.Set(i, DifferentiateInternal(tensor[i], rule, transformations));
                if (rule.Var.Indices.Size() != 0)
                {
                    differentiated = EliminateMetricsTransformation.Eliminate(differentiated);
                }

                differentiated = ApplyTransformations(differentiated, transformations);
                result.Put(differentiated);
            }

            return result.Build();
        }

        throw new NotSupportedException($"Unsupported tensor type: {tensor.GetType()}");
    }

    private static Tensor ApplyTransformations(Tensor tensor, ITransformation[] transformations)
    {
        return Transformation.ApplySequentially(tensor, transformations);
    }

    private static SimpleTensorDifferentiationRule CreateRule(SimpleTensor var, bool useDeltaFunction)
    {
        ArgumentNullException.ThrowIfNull(var);

        return var.Indices.Size() == 0
            ? new SymbolicDifferentiationRule(var, useDeltaFunction)
            : new SymmetricDifferentiationRule(var, useDeltaFunction);
    }

    private static Tensor CreateFieldDerivative(TensorField field, Indices.Indices derivativeIndices, int argPosition)
    {
        int[] orders = new int[field.Size];
        orders[argPosition] = 1;

        NameDescriptorForTensorField descriptor = field.GetNameDescriptor().GetDerivative(orders);
        IndicesBuilder indicesBuilder = new();
        indicesBuilder.Append(field.SimpleIndices);
        indicesBuilder.Append(derivativeIndices);

        SimpleIndices indices = IndicesFactory.CreateSimple(descriptor.GetSymmetries(), indicesBuilder.Indices);
        return TensorField.Create(descriptor.Id, indices, field.GetArgIndices(), field.GetArguments());
    }

    private static Tensor CreateDiracDelta(Tensor left, Tensor right)
    {
        return TensorField.Create(
            Context.Get().DiracDeltaName,
            IndicesFactory.EmptySimpleIndices,
            [left, right]);
    }
}

internal abstract class SimpleTensorDifferentiationRule
{
    protected SimpleTensorDifferentiationRule(SimpleTensor var, bool useDeltaFunction)
    {
        Var = var ?? throw new ArgumentNullException(nameof(var));
        UseDeltaFunction = useDeltaFunction;
    }

    public SimpleTensor Var { get; }

    public bool UseDeltaFunction { get; }

    public Tensor DifferentiateSimpleTensor(SimpleTensor simpleTensor)
    {
        ArgumentNullException.ThrowIfNull(simpleTensor);

        if (simpleTensor.Name != Var.Name)
        {
            return Complex.Zero;
        }

        return DifferentiateSimpleTensorWithoutCheck(simpleTensor);
    }

    public abstract SimpleTensorDifferentiationRule NewRuleForTensor(Tensor tensor);

    public abstract Tensor DifferentiateSimpleTensorWithoutCheck(SimpleTensor simpleTensor);

    public abstract int[] GetForbidden();
}

internal sealed class SymbolicDifferentiationRule : SimpleTensorDifferentiationRule
{
    public SymbolicDifferentiationRule(SimpleTensor var, bool useDeltaFunction)
        : base(var, useDeltaFunction)
    {
    }

    public override Tensor DifferentiateSimpleTensorWithoutCheck(SimpleTensor simpleTensor)
    {
        _ = simpleTensor;
        return Complex.One;
    }

    public override SimpleTensorDifferentiationRule NewRuleForTensor(Tensor tensor)
    {
        _ = tensor;
        return this;
    }

    public override int[] GetForbidden()
    {
        return [];
    }
}

internal sealed class SymmetricDifferentiationRule : SimpleTensorDifferentiationRule
{
    private readonly Tensor _derivative;
    private readonly int[] _allFreeFrom;
    private readonly int[] _freeVarIndices;

    public SymmetricDifferentiationRule(SimpleTensor var, bool useDeltaFunction)
        : base(var, useDeltaFunction)
    {
        SimpleIndices varIndices = var.SimpleIndices;
        int[] allFreeVarIndices = new int[varIndices.Size()];
        int[] allFreeArgIndices = new int[varIndices.Size()];
        IndexGenerator indexGenerator = new(varIndices);
        for (int i = 0; i < allFreeArgIndices.Length; ++i)
        {
            byte type = IndicesUtils.GetType(varIndices[i]);
            int rawState = IndicesUtils.GetRawStateInt(varIndices[i]);

            allFreeVarIndices[i] = IndicesUtils.SetRawState(
                rawState == 0 ? IndicesUtils.UpperRawStateInt : 0,
                indexGenerator.Generate(type));
            allFreeArgIndices[i] = IndicesUtils.SetRawState(rawState, indexGenerator.Generate(type));
        }

        Tensor metricProduct = BuildMetricProduct(allFreeArgIndices, allFreeVarIndices);
        SimpleIndices freeVarSymmetries = IndicesFactory.CreateSimple(varIndices.Symmetries, allFreeVarIndices);
        Tensor derivative = new SymmetrizeITransformation(freeVarSymmetries, true).Transform(metricProduct);

        int[] from = Combine(allFreeVarIndices, allFreeArgIndices);
        int[] to = Combine(varIndices.GetInverted().AllIndices.ToArray(), allFreeArgIndices);
        derivative = ApplyIndexMapping.Apply(derivative, new Mapping(from, to), []);
        _derivative = EliminateMetricsTransformation.Eliminate(derivative);

        _freeVarIndices = var.Indices.GetFree().GetInverted().AllIndices.ToArray();
        _allFreeFrom = Combine(allFreeArgIndices, _freeVarIndices);
    }

    private SymmetricDifferentiationRule(
        SimpleTensor var,
        Tensor derivative,
        int[] allFreeFrom,
        int[] freeVarIndices,
        bool useDeltaFunction)
        : base(var, useDeltaFunction)
    {
        _derivative = derivative ?? throw new ArgumentNullException(nameof(derivative));
        _allFreeFrom = (int[])allFreeFrom.Clone();
        _freeVarIndices = (int[])freeVarIndices.Clone();
    }

    public override Tensor DifferentiateSimpleTensorWithoutCheck(SimpleTensor simpleTensor)
    {
        int[] to = Combine(simpleTensor.Indices.AllIndices.ToArray(), _freeVarIndices);
        return ApplyIndexMapping.Apply(_derivative, new Mapping(_allFreeFrom, to), []);
    }

    public override SimpleTensorDifferentiationRule NewRuleForTensor(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        return new SymmetricDifferentiationRule(
            Var,
            ApplyIndexMapping.RenameDummy(_derivative, TensorUtils.GetAllIndicesNamesT(tensor).ToArray()),
            _allFreeFrom,
            _freeVarIndices,
            UseDeltaFunction);
    }

    public override int[] GetForbidden()
    {
        return TensorUtils.GetAllIndicesNamesT(_derivative).ToArray();
    }

    private static Tensor BuildMetricProduct(int[] argIndices, int[] varIndices)
    {
        ScalarsBackedProductBuilder builder = new();
        for (int i = 0; i < argIndices.Length; ++i)
        {
            builder.Put(Context.Get().CreateMetricOrKronecker(argIndices[i], varIndices[i]));
        }

        return builder.Build();
    }

    private static int[] Combine(int[] first, int[] second)
    {
        int[] result = new int[first.Length + second.Length];
        Array.Copy(first, 0, result, 0, first.Length);
        Array.Copy(second, 0, result, first.Length, second.Length);
        return result;
    }
}

public sealed class DifferentiateOptions
{
    [Option(Name = "Simplifications", Index = 0)]
    public ITransformation Simplifications = Transformation.Identity;

    [Option(Name = "DiracDelta", Index = 1)]
    public bool UseDiracDelta = true;

    public DifferentiateOptions()
    {
    }

    public DifferentiateOptions(ITransformation simplifications)
    {
        Simplifications = simplifications ?? throw new ArgumentNullException(nameof(simplifications));
    }
}
