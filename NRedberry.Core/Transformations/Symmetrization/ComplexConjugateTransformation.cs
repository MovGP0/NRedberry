using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Port of cc.redberry.core.transformations.ComplexConjugateTransformation.
/// </summary>
public sealed class ComplexConjugateTransformation : TransformationToStringAble
{
    public static ComplexConjugateTransformation Instance { get; } = new();

    private ComplexConjugateTransformation()
    {
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        FromChildToParentIterator iterator = new(tensor);
        Tensor current;
        while ((current = iterator.Next()) is not null)
        {
            if (current is Complex complex)
            {
                iterator.Set(complex.Conjugate());
            }
        }

        return iterator.Result();
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "Conjugate";
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }
}
