using NRedberry.Core.Tensors;
using NRedberry.Core.Utils;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.DiracSimplify0.
/// </summary>
public sealed class DiracSimplify0 : AbstractFeynCalcTransformation
{
    private readonly Dictionary<int, Tensor> _cache = new();

    public DiracSimplify0(DiracOptions options)
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
}
