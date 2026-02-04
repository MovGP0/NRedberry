using NRedberry.IndexMapping;
using NRedberry.Tensors;
using NRedberry.Tensors.Iterators;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Physics.Oneloopdiv;

/// <summary>
/// Skeleton port of cc.redberry.physics.oneloopdiv.NaiveSubstitution.
/// </summary>
internal sealed class NaiveSubstitution : ITransformation
{
    private readonly Tensor from;
    private readonly Tensor to;

    public NaiveSubstitution(Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);
        this.from = from;
        this.to = to;
    }

    public Tensor Transform(Tensor tensor)
    {
        TreeTraverseIterator iterator = new(tensor);
        TraverseState? state;
        while ((state = iterator.Next()) is not null)
        {
            if (state != TraverseState.Leaving)
            {
                continue;
            }

            Tensor current = iterator.Current();
            Mapping? mapping = IndexMappings.GetFirst(from, current);
            if (mapping is not null)
            {
                Tensor newFrom = ApplyIndexMapping.ApplyIndexMappingAutomatically(to, mapping);
                iterator.Set(newFrom);
            }
        }

        return iterator.Result();
    }
}
