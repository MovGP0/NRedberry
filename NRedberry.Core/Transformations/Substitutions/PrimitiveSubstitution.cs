using NRedberry.Indices;
using NRedberry.IndexMapping;
using NRedberry.Tensors;
using TensorCC = NRedberry.Tensors.CC;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Port of cc.redberry.core.transformations.substitutions.PrimitiveSubstitution.
/// </summary>
public abstract class PrimitiveSubstitution
{
    protected Tensor From { get; }

    protected Tensor To { get; }

    protected bool ToIsSymbolic { get; }

    protected bool PossiblyAddsDummies { get; }

    protected PrimitiveSubstitution(Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        From = ApplyIndexMapping.OptimizeDummies(from);
        To = ApplyIndexMapping.OptimizeDummies(to);

        int[] typesCounts = new int[IndexTypeMethods.TypesCount];
        foreach (int index in TensorUtils.GetAllDummyIndicesIncludingScalarFunctionsT(To))
        {
            ++typesCounts[IndicesUtils.GetType(index)];
        }

        foreach (int index in TensorUtils.GetAllDummyIndicesT(From))
        {
            --typesCounts[IndicesUtils.GetType(index)];
        }

        for (int i = 0; i < typesCounts.Length; ++i)
        {
            if (typesCounts[i] > 0)
            {
                PossiblyAddsDummies = true;
                break;
            }
        }

        ToIsSymbolic = TensorUtils.IsSymbolic(To);
    }

    public Tensor NewTo(Tensor current, SubstitutionIterator iterator)
    {
        ArgumentNullException.ThrowIfNull(current);
        ArgumentNullException.ThrowIfNull(iterator);

        if (current.GetType() != From.GetType())
        {
            return current;
        }

        return NewToCore(current, iterator);
    }

    protected Tensor ApplyIndexMappingToTo(Tensor oldFrom, Tensor to, Mapping mapping, SubstitutionIterator iterator)
    {
        ArgumentNullException.ThrowIfNull(oldFrom);
        ArgumentNullException.ThrowIfNull(to);
        ArgumentNullException.ThrowIfNull(mapping);
        ArgumentNullException.ThrowIfNull(iterator);

        if (ToIsSymbolic)
        {
            return mapping.GetSign() ? NRedberry.Tensors.Tensors.Negate(to) : to;
        }

        if (PossiblyAddsDummies)
        {
            return ApplyIndexMapping.Apply(to, mapping, iterator.GetForbidden());
        }

        return ApplyIndexMapping.ApplyIndexMappingAndRenameAllDummies(
            to,
            mapping,
            TensorUtils.GetAllDummyIndicesT(oldFrom).ToArray());
    }

    protected abstract Tensor NewToCore(Tensor currentNode, SubstitutionIterator iterator);

    public string ToString(OutputFormat outputFormat)
    {
        ArgumentNullException.ThrowIfNull(outputFormat);

        string symbol = outputFormat.Is(OutputFormat.WolframMathematica) ? "->" : "=";
        return From.ToString(outputFormat) + symbol + To.ToString(outputFormat);
    }

    public override string ToString()
    {
        return ToString(TensorCC.GetDefaultOutputFormat());
    }
}
