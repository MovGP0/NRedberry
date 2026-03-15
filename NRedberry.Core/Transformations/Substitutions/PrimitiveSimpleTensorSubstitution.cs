using NRedberry.IndexMapping;
using NRedberry.Tensors;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Port of cc.redberry.core.transformations.substitutions.PrimitiveSimpleTensorSubstitution.
/// </summary>
public sealed class PrimitiveSimpleTensorSubstitution : PrimitiveSubstitution
{
    public PrimitiveSimpleTensorSubstitution(Tensor from, Tensor to)
        : base(from, to)
    {
    }

    protected override Tensor NewToCore(Tensor currentNode, SubstitutionIterator iterator)
    {
        ArgumentNullException.ThrowIfNull(currentNode);
        ArgumentNullException.ThrowIfNull(iterator);

        Mapping? mapping = IndexMappings.GetFirst(From, currentNode);
        if (mapping is null)
        {
            return currentNode;
        }

        return ApplyIndexMappingToTo(currentNode, To, mapping, iterator);
    }
}
