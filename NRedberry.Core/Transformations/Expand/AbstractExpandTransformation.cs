using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Tensors.Functions;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Substitutions;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.AbstractExpandTransformation.
/// </summary>
public abstract class AbstractExpandTransformation : TransformationToStringAble
{
    protected readonly ITransformation[] transformations;
    protected readonly TraverseGuide traverseGuide;

    protected static TraverseGuide DefaultExpandTraverseGuide { get; } = new DefaultExpandTraverseGuide();

    protected AbstractExpandTransformation()
    {
        transformations = Array.Empty<ITransformation>();
        traverseGuide = DefaultExpandTraverseGuide;
    }

    protected AbstractExpandTransformation(ITransformation[] transformations)
    {
        this.transformations = transformations ?? throw new ArgumentNullException(nameof(transformations));
        traverseGuide = DefaultExpandTraverseGuide;
    }

    protected AbstractExpandTransformation(ITransformation[] transformations, TraverseGuide traverseGuide)
    {
        this.transformations = transformations ?? throw new ArgumentNullException(nameof(transformations));
        this.traverseGuide = traverseGuide ?? throw new ArgumentNullException(nameof(traverseGuide));
    }

    protected AbstractExpandTransformation(ExpandOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);
        transformations = options.Simplifications is null ? Array.Empty<ITransformation>() : [options.Simplifications];
        traverseGuide = options.TraverseGuide ?? DefaultExpandTraverseGuide;
    }

    public virtual Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        SubstitutionIterator iterator = new(tensor, traverseGuide);
        Tensor? current;
        while ((current = iterator.Next()) is not null)
        {
            if (current is Product product)
            {
                iterator.UnsafeSet(ExpandProduct(product, transformations));
                continue;
            }

            if (!ExpandUtils.IsExpandablePower(current))
            {
                continue;
            }

            var sum = (Sum)current[0];
            int exponent = ((Complex)current[1]).IntValue();
            if (exponent == -1)
            {
                continue;
            }

            bool symbolic = TensorUtils.IsSymbolic(sum);
            bool reciprocal = exponent < 0;
            exponent = Math.Abs(exponent);

            Tensor temp = symbolic
                ? ExpandUtils.ExpandSymbolicPower(sum, exponent, transformations)
                : ExpandUtils.ExpandPower(sum, exponent, iterator.GetForbidden(), transformations);
            if (reciprocal)
            {
                temp = Tensors.Tensors.Reciprocal(temp);
            }

            if (symbolic)
            {
                iterator.UnsafeSet(temp);
            }
            else
            {
                iterator.Set(temp);
            }
        }

        return iterator.Result();
    }

    protected abstract Tensor ExpandProduct(Product product, ITransformation[] transformations);

    public abstract string ToString(OutputFormat outputFormat);

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }
}

internal sealed class DefaultExpandTraverseGuide : TraverseGuide
{
    public TraversePermission GetPermission(Tensor tensor, Tensor parent, int indexInParent)
    {
        if (tensor is ScalarFunction)
        {
            return TraversePermission.DontShow;
        }

        if (tensor is TensorField)
        {
            return TraversePermission.DontShow;
        }

        if (TensorUtils.IsNegativeIntegerPower(tensor))
        {
            return TraversePermission.DontShow;
        }

        return TraversePermission.Enter;
    }
}
