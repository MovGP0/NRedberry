using NRedberry.Concurrent;
using NRedberry.Tensors;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/ProviderFunctions.java
 */

internal static class ProviderFunctions
{
    public static IIndexMappingProviderFactory OddFactory { get; } = new ProviderFunctionsOddFactory();

    public static IIndexMappingProviderFactory EvenFactory { get; } = new ProviderFunctionsEvenFactory();

    public static IIndexMappingProviderFactory Factory { get; } = new ProviderFunctionsFactory();

    public static IIndexMappingProvider CreateOdd(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        IOutputPort<IIndexMappingBuffer> mappingPort = IndexMappings.CreatePortOfBuffers(from[0], to[0]);
        IIndexMappingBuffer? buffer;
        byte state = 0;

        while ((buffer = mappingPort.Take()) is not null)
        {
            state |= (byte)(buffer.GetSign() ? 0x10 : 0x01);
            if (state == 0x11)
            {
                break;
            }
        }

        return state switch
        {
            0x00 => IndexMappingProviderUtil.EmptyProvider,
            0x01 => new DummyIndexMappingProvider(provider),
            0x10 => new MinusIndexMappingProvider(provider),
            0x11 => new PlusMinusIndexMappingProvider(provider),
            _ => throw new InvalidOperationException("Ups")
        };
    }

    public static IIndexMappingProvider CreateEven(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        if (IndexMappings.CreatePortOfBuffers(from[0], to[0]).Take() is not null)
        {
            return new DummyIndexMappingProvider(provider);
        }

        return IndexMappingProviderUtil.EmptyProvider;
    }

    public static IIndexMappingProvider Create(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        IIndexMappingBuffer? buffer = IndexMappings.CreatePortOfBuffers(from[0], to[0]).Take();
        if (buffer?.GetSign() == false)
        {
            return new DummyIndexMappingProvider(provider);
        }

        return IndexMappingProviderUtil.EmptyProvider;
    }
}

internal sealed class ProviderFunctionsOddFactory : IIndexMappingProviderFactory
{
    public IIndexMappingProvider Create(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        return ProviderFunctions.CreateOdd(provider, from, to);
    }
}

internal sealed class ProviderFunctionsEvenFactory : IIndexMappingProviderFactory
{
    public IIndexMappingProvider Create(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        return ProviderFunctions.CreateEven(provider, from, to);
    }
}

internal sealed class ProviderFunctionsFactory : IIndexMappingProviderFactory
{
    public IIndexMappingProvider Create(IIndexMappingProvider provider, Tensor from, Tensor to)
    {
        return ProviderFunctions.Create(provider, from, to);
    }
}
