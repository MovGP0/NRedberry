using NRedberry.Tensors;
using NRedberry.Transformations.Substitutions;
using TensorCC = NRedberry.Tensors.CC;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Port of cc.redberry.physics.feyncalc.DiracSimplifyTransformation.
/// </summary>
public sealed class DiracSimplifyTransformation : AbstractTransformationWithGammas
{
    private readonly SimplifyGamma5Transformation _simplifyGamma5;
    private readonly Lazy<DiracSimplify0> _simplify0;
    private readonly Lazy<DiracSimplify1> _simplify1;

    public DiracSimplifyTransformation(DiracOptions options)
        : base(options)
    {
        ArgumentNullException.ThrowIfNull(options);

        _simplifyGamma5 = new SimplifyGamma5Transformation(options);
        _simplify0 = new Lazy<DiracSimplify0>(() => new DiracSimplify0(options));
        _simplify1 = new Lazy<DiracSimplify1>(() => new DiracSimplify1(options));
    }

    public new Tensor Transform(Tensor tensor)
    {
        ArgumentNullException.ThrowIfNull(tensor);

        SubstitutionIterator iterator = new(tensor);
        Tensor original;
        while ((original = iterator.Next()) is not null)
        {
            if (original is not Product)
            {
                continue;
            }

            if (original.Indices.Size(MatrixType) == 0)
            {
                continue;
            }

            if (!ContainsGammaOr5Matrices(original))
            {
                continue;
            }

            Tensor current = _simplifyGamma5.Transform(original);
            current = _simplify0.Value.Transform(current);
            current = _simplify1.Value.Transform(current);
            iterator.SafeSet(current);
        }

        return iterator.Result();
    }

    public override string ToString()
    {
        return ToString(TensorCC.GetDefaultOutputFormat());
    }

    public new string ToString(OutputFormat outputFormat)
    {
        _ = outputFormat;
        return "DiracSimplify";
    }
}
