using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;
using NRedberry.Transformations.Substitutions;
using TensorFactory = NRedberry.Tensors.Tensors;

namespace NRedberry.Physics.Oneloopdiv;

/// <summary>
/// Skeleton port of cc.redberry.physics.oneloopdiv.SqrSubs.
/// </summary>
internal sealed class SqrSubs : ITransformation
{
    private readonly ITransformation _transformation;

    public SqrSubs(SimpleTensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);
        if (tensor.Indices.Size() != 1)
        {
            throw new ArgumentException("Expected a single index.", nameof(tensor));
        }

        Tensor inverted = ApplyIndexMapping.InvertIndices(tensor);
        Tensor product = TensorFactory.Multiply(tensor, inverted);
        _transformation = new SubstitutionTransformation(TensorFactory.Expression(product, Complex.One));
    }

    public Tensor Transform(Tensor tensor)
    {
        return _transformation.Transform(tensor);
    }
}
