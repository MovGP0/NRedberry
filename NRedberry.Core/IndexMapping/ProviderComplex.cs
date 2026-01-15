using NRedberry.Numbers;
using NRedberry.Tensors;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/ProviderComplex.java
 */

internal static class ProviderComplex
{
    public static IIndexMappingProviderFactory Factory { get; } = new ProviderComplexFactory();

    public static IIndexMappingProvider Create(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        if (from.Equals(to))
        {
            Complex complexFrom = (Complex)from;
            if (complexFrom.IsZero())
            {
                return new PlusMinusIndexMappingProvider(provider);
            }

            return new DummyIndexMappingProvider(provider);
        }

        Complex complexTo = (Complex)to;
        if (from.Equals(complexTo.Negate()))
        {
            return new MinusIndexMappingProvider(provider);
        }

        return IndexMappingProviderUtil.EmptyProvider;
    }
}

internal sealed class ProviderComplexFactory : IIndexMappingProviderFactory
{
    public IIndexMappingProvider Create(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        return ProviderComplex.Create(provider, from, to);
    }
}
