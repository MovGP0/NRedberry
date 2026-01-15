using NRedberry.Concurrent;
using NRedberry.Tensors;

namespace NRedberry.IndexMapping;

/*
 * Original: ./core/src/main/java/cc/redberry/core/indexmapping/SimpleProductMappingsPort.java
 */

internal sealed class SimpleProductMappingsPort : IOutputPort<IIndexMappingBuffer>
{
    private readonly IIndexMappingProvider[] _providers;
    private bool _inited;

    public SimpleProductMappingsPort(IIndexMappingProvider[] providers)
    {
        ArgumentNullException.ThrowIfNull(providers);
        _providers = providers;
    }

    public SimpleProductMappingsPort(IIndexMappingProvider provider, Tensor[] from, Tensor[] to)
    {
        ArgumentNullException.ThrowIfNull(provider);
        ArgumentNullException.ThrowIfNull(from);
        ArgumentNullException.ThrowIfNull(to);

        _providers = new IIndexMappingProvider[from.Length];
        _providers[0] = IndexMappings.CreatePort(provider, from[0], to[0]);
        for (var i = 1; i < from.Length; ++i)
        {
            _providers[i] = IndexMappings.CreatePort(_providers[i - 1], from[i], to[i]);
        }
    }

    public IIndexMappingBuffer Take()
    {
        if (!_inited)
        {
            for (var i = 0; i < _providers.Length; ++i)
            {
                _providers[i].Tick();
            }

            _inited = true;
        }

        var index = _providers.Length - 1;
        IIndexMappingBuffer buffer = _providers[index].Take();
        if (buffer is not null)
        {
            return buffer;
        }

        OUTER:
        while (true)
        {
            bool r;
            while ((r = !_providers[index--].Tick()) && index >= 0)
            {
            }

            if (index == -1 && r)
            {
                return null!;
            }

            index += 2;
            for (; index < _providers.Length; ++index)
            {
                if (!_providers[index].Tick())
                {
                    index--;
                    goto OUTER;
                }
            }

            index--;
            buffer = _providers[index].Take();
            if (buffer is not null)
            {
                return buffer;
            }
        }
    }
}
