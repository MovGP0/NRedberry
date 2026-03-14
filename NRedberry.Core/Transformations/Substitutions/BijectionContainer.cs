using NRedberry.IndexMapping;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.substitutions.SumBijectionPort.BijectionContainer.
/// </summary>
public sealed class BijectionContainer
{
    public Mapping Mapping { get; }

    public int[] Bijection { get; }

    public BijectionContainer(Mapping mapping, int[] bijection)
    {
        Mapping = mapping ?? throw new ArgumentNullException(nameof(mapping));
        Bijection = bijection ?? throw new ArgumentNullException(nameof(bijection));
    }

    public override string ToString()
    {
        return $"[{string.Join(", ", Bijection)}]\n{Mapping}";
    }
}
