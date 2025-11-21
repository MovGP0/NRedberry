using NRedberry.Concurrent;
using NRedberry.Tensors;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.substitutions.ProductsBijectionsPort.
/// </summary>
public sealed class ProductsBijectionsPort : IOutputPort<int[]>
{
    public ProductsBijectionsPort(ProductContent fromContent, ProductContent targetContent)
    {
        throw new NotImplementedException();
    }

    public int[] Take()
    {
        throw new NotImplementedException();
    }

    private static bool WeakMatch(Tensor first, Tensor second)
    {
        throw new NotImplementedException();
    }

    private static bool AlreadyContains(int[] bijection, int value)
    {
        throw new NotImplementedException();
    }

    private sealed class InnerPort : IOutputPort<int[]>
    {
        public InnerPort(int[] bijection, int[] seeds)
        {
            throw new NotImplementedException();
        }

        public int[] Take()
        {
            throw new NotImplementedException();
        }

        private void Initialize()
        {
            throw new NotImplementedException();
        }
    }

    private sealed class PermutationInfo
    {
        public PermutationInfo(PermutationInfo? previous, long[] fromContractions, long[] targetContractions)
        {
            throw new NotImplementedException();
        }

        public bool Next()
        {
            throw new NotImplementedException();
        }

        public bool NextAndResetRightChain()
        {
            throw new NotImplementedException();
        }
    }

    private sealed class SeedPlanter
    {
        public SeedPlanter()
        {
            throw new NotImplementedException();
        }

        public int[] Next()
        {
            throw new NotImplementedException();
        }
    }
}
