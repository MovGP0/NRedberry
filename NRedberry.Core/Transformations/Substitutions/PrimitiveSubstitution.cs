using NRedberry.Core.IndexMapping;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Transformations.Substitutions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.substitutions.PrimitiveSubstitution.
/// </summary>
public abstract class PrimitiveSubstitution
{
    protected PrimitiveSubstitution(Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }

    protected Tensor From => throw new NotImplementedException();

    protected Tensor To => throw new NotImplementedException();

    protected bool ToIsSymbolic => throw new NotImplementedException();

    protected bool PossiblyAddsDummies => throw new NotImplementedException();

    public Tensor NewTo(Tensor current, SubstitutionIterator iterator)
    {
        throw new NotImplementedException();
    }

    protected Tensor ApplyIndexMappingToTo(Tensor oldFrom, Tensor to, Mapping mapping, SubstitutionIterator iterator)
    {
        throw new NotImplementedException();
    }

    protected abstract Tensor NewToCore(Tensor currentNode, SubstitutionIterator iterator);

    public string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }
}
