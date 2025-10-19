using NRedberry.Core.Concurrent;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Utils;

/// <summary>
/// Skeleton port of cc.redberry.core.utils.LocalSymbolsProvider.
/// </summary>
public sealed class LocalSymbolsProvider : IOutputPort<SimpleTensor>
{
    private readonly string prefix = string.Empty;
    private readonly HashSet<int> forbiddenNames = [];
    private long counter;

    public LocalSymbolsProvider(Tensor forbidden, string prefix)
    {
        throw new NotImplementedException();
    }

    public SimpleTensor Take()
    {
        throw new NotImplementedException();
    }
}
