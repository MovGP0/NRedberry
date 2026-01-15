using NRedberry.Concurrent;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/IndexMappingProvider.java
 */

public interface IIndexMappingProvider : IOutputPort<IIndexMappingBuffer>
{
    bool Tick();
}

public static class IndexMappingProviderUtil
{
    public static IIndexMappingProvider EmptyProvider { get; } = new EmptyIndexMappingProvider();

    public static IIndexMappingProvider Singleton(IIndexMappingBuffer buffer)
    {
        ArgumentNullException.ThrowIfNull(buffer);
        return new SingletonIndexMappingProvider(buffer);
    }
}

internal sealed class EmptyIndexMappingProvider : IIndexMappingProvider
{
    public bool Tick()
    {
        return false;
    }

    public IIndexMappingBuffer Take()
    {
        return null!;
    }
}

internal sealed class SingletonIndexMappingProvider : IIndexMappingProvider
{
    private IIndexMappingBuffer? _buffer;

    public SingletonIndexMappingProvider(IIndexMappingBuffer buffer)
    {
        _buffer = buffer ?? throw new ArgumentNullException(nameof(buffer));
    }

    public bool Tick()
    {
        return false;
    }

    public IIndexMappingBuffer Take()
    {
        IIndexMappingBuffer? buffer = _buffer;
        _buffer = null;
        return buffer!;
    }
}
