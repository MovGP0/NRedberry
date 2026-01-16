using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.AbstractExpandNumeratorDenominatorTransformation.
/// </summary>
public abstract class AbstractExpandNumeratorDenominatorTransformation : TransformationToStringAble
{
    protected readonly ITransformation[] transformations = [];

    protected AbstractExpandNumeratorDenominatorTransformation()
    {
        transformations = [];
    }

    protected AbstractExpandNumeratorDenominatorTransformation(ITransformation[] transformations)
    {
        this.transformations = transformations ?? throw new ArgumentNullException(nameof(transformations));
    }

    protected AbstractExpandNumeratorDenominatorTransformation(ExpandOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        transformations = options.Simplifications is null ? [] : [options.Simplifications];
    }

    public virtual Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        if (tensor is Product or Power)
        {
            return ExpandProduct(tensor);
        }

        if (tensor is Sum sum)
        {
            SumBuilder sb = new(sum.Size);
            foreach (Tensor summand in sum)
            {
                sb.Put(Transform(summand));
            }

            return sb.Build();
        }

        return tensor;
    }

    protected abstract Tensor ExpandProduct(Tensor tensor);

    public virtual string ToString(OutputFormat outputFormat)
    {
        return GetType().Name;
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }
}
