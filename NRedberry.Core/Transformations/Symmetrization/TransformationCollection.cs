using System.Collections;
using NRedberry.Tensors;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.TransformationCollection.
/// </summary>
public sealed class TransformationCollection : TransformationToStringAble, IEnumerable<ITransformation>
{
    private readonly ITransformation[] transformations = null!;

    public TransformationCollection(IEnumerable<ITransformation> transformations)
    {
        throw new NotImplementedException();
    }

    public TransformationCollection(params ITransformation[] transformations)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyList<ITransformation> GetTransformations()
    {
        throw new NotImplementedException();
    }

    public string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    public IEnumerator<ITransformation> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
