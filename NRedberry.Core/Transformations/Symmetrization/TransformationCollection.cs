using System.Collections;
using System.Text;
using NRedberry.Tensors;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.TransformationCollection.
/// </summary>
public sealed class TransformationCollection : TransformationToStringAble, IEnumerable<ITransformation>
{
    private readonly ITransformation[] _transformations;

    public TransformationCollection(IEnumerable<ITransformation> transformations)
    {
        ArgumentNullException.ThrowIfNull(transformations);
        _transformations = transformations.ToArray();
    }

    public TransformationCollection(params ITransformation[] transformations)
    {
        ArgumentNullException.ThrowIfNull(transformations);
        _transformations = transformations.Length == 0
            ? Array.Empty<ITransformation>()
            : (ITransformation[])transformations.Clone();
    }

    public Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        Tensor current = tensor;
        foreach (ITransformation transformation in _transformations)
        {
            current = transformation.Transform(current);
        }

        return current;
    }

    public IReadOnlyList<ITransformation> GetTransformations()
    {
        return Array.AsReadOnly(_transformations);
    }

    public string ToString(OutputFormat outputFormat)
    {
        if (_transformations.Length == 0)
        {
            return string.Empty;
        }

        StringBuilder sb = new();
        for (int i = 0; i < _transformations.Length; ++i)
        {
            ITransformation transformation = _transformations[i];
            if (transformation is TransformationToStringAble toStringAble)
            {
                sb.Append(toStringAble.ToString(outputFormat));
            }
            else
            {
                sb.Append(transformation);
            }

            if (i < _transformations.Length - 1)
            {
                sb.Append(" & ");
            }
        }

        return sb.ToString();
    }

    public override string ToString()
    {
        return ToString(CC.GetDefaultOutputFormat());
    }

    public IEnumerator<ITransformation> GetEnumerator()
    {
        return ((IEnumerable<ITransformation>)_transformations).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
