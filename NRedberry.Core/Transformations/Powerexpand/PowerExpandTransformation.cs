using NRedberry.Core.Utils;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Symmetrization;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Transformations.Powerexpand;

public sealed class PowerExpandTransformation : TransformationToStringAble
{
    public static PowerExpandTransformation Instance { get; } = new();

    private readonly IIndicator<Tensor> _indicator;

    private PowerExpandTransformation()
        : this(new TrueIndicator<Tensor>())
    {
    }

    public PowerExpandTransformation(IIndicator<Tensor> indicator)
    {
        _indicator = indicator ?? throw new ArgumentNullException(nameof(indicator));
    }

    public PowerExpandTransformation(params SimpleTensor[] variables)
        : this(PowerExpandUtils.VarsToIndicator(variables))
    {
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        FromChildToParentIterator iterator = new(tensor);
        Tensor? current;

        while ((current = iterator.Next()) is not null)
        {
            if (PowerExpandUtils.PowerExpandApplicable(current, _indicator))
            {
                iterator.Set(TensorFactory.Multiply(PowerExpandUtils.PowerExpandToArray1(current, _indicator)));
            }
        }

        return iterator.Result();
    }

    public string ToString(OutputFormat outputFormat)
    {
        return "PowerExpand";
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }
}
