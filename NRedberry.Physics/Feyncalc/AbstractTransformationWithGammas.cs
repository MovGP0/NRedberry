using NRedberry.Parsers.Preprocessor;
using NRedberry.Tensors;
using NRedberry.Transformations.Substitutions;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.AbstractTransformationWithGammas.
/// </summary>
public abstract class AbstractTransformationWithGammas : TransformationToStringAble
{
    protected const string GammaMatrixStringName = "G";
    protected const string Gamma5StringName = "G5";
    protected const string LeviCivitaStringName = "eps";

    protected AbstractTransformationWithGammas(DiracOptions options) => throw new NotImplementedException();

    public override string ToString() => throw new NotImplementedException();

    public string ToString(OutputFormat outputFormat) => throw new NotImplementedException();

    protected ITransformation ExpandAndEliminate => throw new NotImplementedException();
    protected Expression TraceOfOne => throw new NotImplementedException();
    protected Expression DeltaTrace => throw new NotImplementedException();
    protected SubstitutionTransformation DeltaTraces => throw new NotImplementedException();
    protected ChangeIndicesTypesAndTensorNames TokenTransformer => throw new NotImplementedException();
    protected int GammaName => throw new NotImplementedException();
    protected int Gamma5Name => throw new NotImplementedException();
    protected IndexType MetricType => throw new NotImplementedException();
    protected IndexType MatrixType => throw new NotImplementedException();

    public Tensor Transform(Tensor t) => throw new NotImplementedException();
}
