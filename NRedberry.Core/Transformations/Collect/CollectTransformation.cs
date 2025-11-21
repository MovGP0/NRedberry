using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Transformations.Collect;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.collect.CollectTransformation.
/// </summary>
public sealed class CollectTransformation : ITransformation
{
    public CollectTransformation(SimpleTensor[] patterns, ITransformation[] transformations, bool expandSymbolic)
    {
        throw new NotImplementedException();
    }

    public CollectTransformation(SimpleTensor[] patterns, ITransformation[] transformations)
        : this(patterns, transformations, true)
    {
    }

    public CollectTransformation(SimpleTensor[] patterns)
        : this(patterns, [])
    {
    }

    public CollectTransformation(SimpleTensor[] patterns, CollectOptions options)
        : this(patterns, [options.Simplifications ?? throw new ArgumentNullException(nameof(options))], options.ExpandSymbolic)
    {
    }

    public CollectTransformation(SimpleTensor[] patterns, bool expandSymbolic)
        : this(patterns, [], expandSymbolic)
    {
    }

    public Tensor Transform(Tensor t)
    {
        throw new NotImplementedException();
    }

    public sealed class CollectOptions
    {
        public ITransformation? Simplifications { get; set; }
        public bool ExpandSymbolic { get; set; } = true;
    }
}
