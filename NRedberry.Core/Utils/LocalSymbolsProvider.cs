using NRedberry.Concurrent;
using NRedberry.Indices;
using NRedberry.Tensors;
using TensorFunctions = NRedberry.Tensors.Tensors;

namespace NRedberry.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.LocalSymbolsProvider.
/// </summary>
public sealed class LocalSymbolsProvider : IOutputPort<SimpleTensor>
{
    private readonly string _prefix;
    private readonly HashSet<int> _forbiddenNames;
    private long _counter;

    public LocalSymbolsProvider(Tensor forbidden, string prefix)
    {
        ArgumentNullException.ThrowIfNull(forbidden);
        ArgumentNullException.ThrowIfNull(prefix);

        _forbiddenNames = TensorUtils.GetSimpleTensorsNames(forbidden);
        _prefix = prefix;
    }

    public SimpleTensor Take()
    {
        SimpleTensor st;
        do
        {
            st = TensorFunctions.SimpleTensor($"{_prefix}{_counter++}", IndicesFactory.EmptySimpleIndices);
        }
        while (_forbiddenNames.Contains(st.Name));

        return st;
    }
}
