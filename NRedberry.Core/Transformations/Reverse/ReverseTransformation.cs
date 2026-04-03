using System.Text;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Reverse;

public sealed class ReverseTransformation : TransformationToStringAble
{
    private readonly SingleReverse[] _reversers;

    public ReverseTransformation(params IndexType[] types)
    {
        ArgumentNullException.ThrowIfNull(types);

        _reversers = new SingleReverse[types.Length];
        for (int i = 0; i < types.Length; ++i)
        {
            _reversers[i] = new SingleReverse(types[i]);
        }
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        return Transformation.ApplySequentially(tensor, _reversers);
    }

    public string ToString(OutputFormat outputFormat)
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append("Reverse[");
        for (int i = 0; i < _reversers.Length; ++i)
        {
            if (i > 0)
            {
                stringBuilder.Append(',');
            }

            stringBuilder.Append(_reversers[i].Type);
        }

        stringBuilder.Append(']');
        return stringBuilder.ToString();
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }
}
