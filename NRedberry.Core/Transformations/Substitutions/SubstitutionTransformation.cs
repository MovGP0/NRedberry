using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.substitutions.SubstitutionTransformation.
/// </summary>
public sealed class SubstitutionTransformation : TransformationToStringAble
{
    private SubstitutionTransformation(PrimitiveSubstitution[] primitiveSubstitutions, bool applyIfModified)
    {
        throw new NotImplementedException();
    }

    public SubstitutionTransformation(Expression[] expressions, bool applyIfModified)
    {
        throw new NotImplementedException();
    }

    public SubstitutionTransformation(Expression expression)
    {
        throw new NotImplementedException();
    }

    public SubstitutionTransformation(params Expression[] expressions)
    {
        throw new NotImplementedException();
    }

    public SubstitutionTransformation(Tensor from, Tensor to, bool applyIfModified)
    {
        throw new NotImplementedException();
    }

    public SubstitutionTransformation(Tensor[] from, Tensor[] to)
    {
        throw new NotImplementedException();
    }

    public SubstitutionTransformation(Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }

    public SubstitutionTransformation(Tensor[] from, Tensor[] to, bool applyIfModified)
    {
        throw new NotImplementedException();
    }

    public SubstitutionTransformation Transpose()
    {
        throw new NotImplementedException();
    }

    public SubstitutionTransformation AsSimpleSubstitution()
    {
        throw new NotImplementedException();
    }

    private static void CheckConsistence(Tensor[] from, Tensor[] to)
    {
        throw new NotImplementedException();
    }

    private static void CheckConsistence(Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }

    private static PrimitiveSubstitution CreatePrimitiveSubstitution(Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
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
}
