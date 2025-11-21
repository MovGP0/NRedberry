using NRedberry.Core.Utils;
using NRedberry.Graphs;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.AbstractFeynCalcTransformation.
/// </summary>
public abstract class AbstractFeynCalcTransformation : AbstractTransformationWithGammas
{
    protected AbstractFeynCalcTransformation(DiracOptions options, ITransformation? preprocessor)
        : base(options)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    protected virtual IIndicator<GraphType> GraphFilter()
    {
        throw new NotImplementedException();
    }

    protected bool IsZeroTrace(ProductOfGammas productOfGammas)
    {
        throw new NotImplementedException();
    }

    protected virtual Tensor? TransformLine(ProductOfGammas productOfGammas, List<int> modifiedElements)
    {
        throw new NotImplementedException();
    }

    protected ITransformation? Preprocessor => throw new NotImplementedException();
}
