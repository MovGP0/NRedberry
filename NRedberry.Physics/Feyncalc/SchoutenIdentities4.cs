using NRedberry.Indices;
using NRedberry.Numbers;
using NRedberry.Tensors;
using NRedberry.Transformations.Symmetrization;

namespace NRedberry.Physics.Feyncalc;

/// <summary>
/// Skeleton port of cc.redberry.physics.feyncalc.SchoutenIdentities4.
/// </summary>
public sealed class SchoutenIdentities4 : ITransformation
{
    private readonly SimpleTensor leviCivita = null!;
    private readonly Tensor[] schouten1 = [];
    private readonly Tensor[] schouten2 = [];
    private readonly Tensor[] schouten3 = [];
    private readonly Tensor[][] allSchouten = [];

    private static readonly string[] SchoutenCombinations1 = [];
    private static readonly string[] SchoutenCombinations2 = [];
    private static readonly string[] SchoutenCombinations3 = [];

    public SchoutenIdentities4(SimpleTensor leviCivita)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    private static Mapping0? BuildMapping(Tensor epsilon, Tensor part)
    {
        throw new NotImplementedException();
    }

    private static bool TestMapping(Mapping0 mapping, Tensor epsilon, Tensor part)
    {
        throw new NotImplementedException();
    }

    private static bool Contains(int[] array, int value)
    {
        throw new NotImplementedException();
    }

    private sealed class Mapping0(Complex factor, IIndexMapping mapping)
    {
        public Complex Factor { get; } = factor;

        public IIndexMapping Mapping { get; } = mapping;
    }
}
