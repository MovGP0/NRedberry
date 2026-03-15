using System.Text;
using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using TensorApi = NRedberry.Tensors.Tensors;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Port of cc.redberry.core.transformations.substitutions.SubstitutionTransformation.
/// </summary>
public sealed class SubstitutionTransformation : TransformationToStringAble
{
    private readonly PrimitiveSubstitution[] _primitiveSubstitutions;
    private readonly Tensor[] _from;
    private readonly Tensor[] _to;
    private readonly bool[] _possiblyAddsDummies;
    private readonly bool _applyIfModified;

    private SubstitutionTransformation(
        PrimitiveSubstitution[] primitiveSubstitutions,
        Tensor[] from,
        Tensor[] to,
        bool[] possiblyAddsDummies,
        bool applyIfModified)
    {
        ArgumentNullException.ThrowIfNull(primitiveSubstitutions);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);
        ArgumentNullException.ThrowIfNull(possiblyAddsDummies);

        if (primitiveSubstitutions.Length != from.Length
            || from.Length != to.Length
            || to.Length != possiblyAddsDummies.Length)
        {
            throw new ArgumentException("Substitution arrays must have the same length.");
        }

        _primitiveSubstitutions = primitiveSubstitutions;
        _from = from;
        _to = to;
        _possiblyAddsDummies = possiblyAddsDummies;
        _applyIfModified = applyIfModified;
    }

    public SubstitutionTransformation(Expression[] expressions, bool applyIfModified)
    {
        ArgumentNullException.ThrowIfNull(expressions);

        _primitiveSubstitutions = new PrimitiveSubstitution[expressions.Length];
        _from = new Tensor[expressions.Length];
        _to = new Tensor[expressions.Length];
        _possiblyAddsDummies = new bool[expressions.Length];
        _applyIfModified = applyIfModified;

        for (int i = expressions.Length - 1; i >= 0; --i)
        {
            Expression expression = expressions[i] ?? throw new ArgumentNullException(nameof(expressions));
            _primitiveSubstitutions[i] = CreatePrimitiveSubstitution(
                expression[0],
                expression[1],
                out _from[i],
                out _to[i],
                out _possiblyAddsDummies[i]);
        }
    }

    public SubstitutionTransformation(Expression expression)
        : this(
            expression is null ? throw new ArgumentNullException(nameof(expression)) : expression[0],
            expression[1])
    {
    }

    public SubstitutionTransformation(params Expression[] expressions)
        : this(
            expressions,
            expressions.Length == 1 && !TensorUtils.ShareSimpleTensors(expressions[0][0], expressions[0][1]))
    {
    }

    public SubstitutionTransformation(Tensor from, Tensor to, bool applyIfModified)
        : this([from], [to], applyIfModified)
    {
    }

    public SubstitutionTransformation(Tensor[] from, Tensor[] to)
        : this(
            from,
            to,
            from.Length == 1 && !TensorUtils.ShareSimpleTensors(from[0], to[0]))
    {
    }

    public SubstitutionTransformation(Tensor from, Tensor to)
        : this(
            from,
            to,
            !TensorUtils.ShareSimpleTensors(from, to))
    {
    }

    public SubstitutionTransformation(Tensor[] from, Tensor[] to, bool applyIfModified)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        CheckConsistence(from, to);

        _primitiveSubstitutions = new PrimitiveSubstitution[from.Length];
        _from = new Tensor[from.Length];
        _to = new Tensor[to.Length];
        _possiblyAddsDummies = new bool[from.Length];
        _applyIfModified = applyIfModified;

        for (int i = 0; i < from.Length; ++i)
        {
            _primitiveSubstitutions[i] = CreatePrimitiveSubstitution(
                from[i],
                to[i],
                out _from[i],
                out _to[i],
                out _possiblyAddsDummies[i]);
        }
    }

    public SubstitutionTransformation Transpose()
    {
        Tensor[] from = new Tensor[_primitiveSubstitutions.Length];
        Tensor[] to = new Tensor[_primitiveSubstitutions.Length];

        for (int i = _primitiveSubstitutions.Length - 1; i >= 0; --i)
        {
            from[i] = _to[i];
            to[i] = _from[i];
        }

        return new SubstitutionTransformation(from, to, _applyIfModified);
    }

    public SubstitutionTransformation AsSimpleSubstitution()
    {
        PrimitiveSubstitution[] primitiveSubstitutions = new PrimitiveSubstitution[_primitiveSubstitutions.Length];
        for (int i = _primitiveSubstitutions.Length - 1; i >= 0; --i)
        {
            primitiveSubstitutions[i] = new PrimitiveSimpleTensorSubstitution(_from[i], _to[i]);
        }

        return new SubstitutionTransformation(
            primitiveSubstitutions,
            (Tensor[])_from.Clone(),
            (Tensor[])_to.Clone(),
            (bool[])_possiblyAddsDummies.Clone(),
            _applyIfModified);
    }

    private static void CheckConsistence(Tensor[] from, Tensor[] to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        if (from.Length != to.Length)
        {
            throw new ArgumentException("from array and to array have different length.");
        }

        for (int i = from.Length - 1; i >= 0; --i)
        {
            CheckConsistence(from[i], to[i]);
        }
    }

    private static void CheckConsistence(Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        if (!TensorUtils.IsZeroOrIndeterminate(to)
            && !from.Indices.GetFree().EqualsRegardlessOrder(to.Indices.GetFree()))
        {
            throw new ArgumentException(
                $"Tensor from free indices not equal to tensor to free indices: {from.Indices.GetFree()}  {to.Indices.GetFree()}");
        }
    }

    private static PrimitiveSubstitution CreatePrimitiveSubstitution(Tensor from, Tensor to)
    {
        return CreatePrimitiveSubstitution(from, to, out _, out _, out _);
    }

    private static PrimitiveSubstitution CreatePrimitiveSubstitution(
        Tensor from,
        Tensor to,
        out Tensor normalizedFrom,
        out Tensor normalizedTo,
        out bool possiblyAddsDummies)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        normalizedFrom = ApplyIndexMapping.OptimizeDummies(from);
        normalizedTo = ApplyIndexMapping.OptimizeDummies(to);

        Type fromType = normalizedFrom.GetType();
        if (fromType == typeof(SimpleTensor))
        {
            possiblyAddsDummies = CalculatePossiblyAddsDummies(normalizedFrom, normalizedTo);
            return new PrimitiveSimpleTensorSubstitution(normalizedFrom, normalizedTo);
        }

        if (fromType == typeof(TensorField))
        {
            bool argumentIsNotSimple = false;
            foreach (Tensor tensor in normalizedFrom)
            {
                if (tensor is not SimpleTensor)
                {
                    argumentIsNotSimple = true;
                    break;
                }
            }

            possiblyAddsDummies = CalculatePossiblyAddsDummies(normalizedFrom, normalizedTo);
            if (argumentIsNotSimple)
            {
                return new PrimitiveSimpleTensorSubstitution(normalizedFrom, normalizedTo);
            }

            return new PrimitiveTensorFieldSubstitution(normalizedFrom, normalizedTo);
        }

        if (fromType == typeof(Product))
        {
            Product product = (Product)normalizedFrom;
            if (product.Size == 2 && product[0] is Complex factor)
            {
                return CreatePrimitiveSubstitution(
                    product[1],
                    TensorApi.Divide(normalizedTo, factor),
                    out normalizedFrom,
                    out normalizedTo,
                    out possiblyAddsDummies);
            }

            possiblyAddsDummies = CalculatePossiblyAddsDummies(normalizedFrom, normalizedTo);
            return new PrimitiveProductSubstitution(normalizedFrom, normalizedTo);
        }

        if (fromType == typeof(Sum))
        {
            possiblyAddsDummies = CalculatePossiblyAddsDummies(normalizedFrom, normalizedTo);
            return new PrimitiveSumSubstitution(normalizedFrom, normalizedTo);
        }

        possiblyAddsDummies = CalculatePossiblyAddsDummies(normalizedFrom, normalizedTo);
        return new PrimitiveSimpleTensorSubstitution(normalizedFrom, normalizedTo);
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        SubstitutionIterator iterator = new(tensor);
        Tensor? current;
        while ((current = iterator.Next()) is not null)
        {
            if (!_applyIfModified && iterator.IsCurrentModified())
            {
                continue;
            }

            Tensor old = current;
            bool supposeIndicesAreAdded = false;
            for (int i = 0; i < _primitiveSubstitutions.Length; ++i)
            {
                current = _primitiveSubstitutions[i].NewTo(old, iterator);
                if (!ReferenceEquals(current, old))
                {
                    supposeIndicesAreAdded |= _possiblyAddsDummies[i];
                    if (!_applyIfModified)
                    {
                        break;
                    }
                }

                old = current;
            }

            iterator.Set(current, supposeIndicesAreAdded);
        }

        return iterator.Result();
    }

    public string ToString(OutputFormat outputFormat)
    {
        ArgumentNullException.ThrowIfNull(outputFormat);

        StringBuilder builder = new();
        builder.Append('{');
        for (int i = 0; i < _primitiveSubstitutions.Length; ++i)
        {
            if (i > 0)
            {
                builder.Append(',');
            }

            builder.Append(_primitiveSubstitutions[i].ToString(outputFormat));
        }

        builder.Append('}');
        return builder.ToString();
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }

    private static bool CalculatePossiblyAddsDummies(Tensor from, Tensor to)
    {
        int[] typesCounts = new int[IndexTypeMethods.TypesCount];
        foreach (int index in TensorUtils.GetAllDummyIndicesIncludingScalarFunctionsT(to))
        {
            ++typesCounts[IndicesUtils.GetType(index)];
        }

        foreach (int index in TensorUtils.GetAllDummyIndicesT(from))
        {
            --typesCounts[IndicesUtils.GetType(index)];
        }

        for (int i = 0; i < typesCounts.Length; ++i)
        {
            if (typesCounts[i] > 0)
            {
                return true;
            }
        }

        return false;
    }
}
