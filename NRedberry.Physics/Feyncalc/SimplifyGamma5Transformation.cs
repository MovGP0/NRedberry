using NRedberry.Core.Tensors;
using NRedberry.Core.Utils;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.SimplifyGamma5Transformation.
/// </summary>
public sealed class SimplifyGamma5Transformation : AbstractFeynCalcTransformation
{
    public SimplifyGamma5Transformation(DiracOptions options)
        : base(options, null)
    {
        throw new NotImplementedException();
    }

    public string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    protected override Tensor? TransformLine(ProductOfGammas productOfGammas, IntArrayList modifiedElements)
    {
        throw new NotImplementedException();
    }

    private Tensor SimplifyProduct(IList<Tensor> gammas)
    {
        throw new NotImplementedException();
    }
}
