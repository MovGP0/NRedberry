using System.Collections.Generic;
using System.Linq;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Transformations.Symmetrization;

public sealed class TransformationCollection : Transformation
{
    private readonly Transformation[] _transformations;

    public TransformationCollection(IEnumerable<Transformation> transformations)
        => _transformations = transformations.ToArray();

    public TransformationCollection(params Transformation[] transformations)
        => _transformations = (Transformation[])transformations.Clone();

    public Tensor Transform(Tensor t)
    {
        foreach (Transformation tr in _transformations)
        {
            t = tr.Transform(t);
        }
        return t;
    }

    public List<Transformation> GetTransformations() => new(_transformations);

    public override string ToString() => string.Join<Transformation>("\n", _transformations);
}