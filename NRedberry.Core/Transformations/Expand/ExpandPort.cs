using NRedberry.Core.Concurrent;
using NRedberry.Core.Tensors;

namespace NRedberry.Core.Transformations.Expand;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.expand.ExpandPort.
/// </summary>
public static class ExpandPort
{
    public static Tensor ExpandUsingPort(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public static Tensor ExpandUsingPort(Tensor tensor, bool expandSymbolic)
    {
        throw new NotImplementedException();
    }

    public static IOutputPort<Tensor> CreatePort(Tensor tensor, bool expandSymbolic)
    {
        throw new NotImplementedException();
    }

    public sealed class ExpandPairPort : IOutputPort<Tensor>
    {
        public ExpandPairPort(Sum first, Sum second)
        {
            throw new NotImplementedException();
        }

        public ExpandPairPort(Sum first, Sum second, Tensor[] factors)
        {
            throw new NotImplementedException();
        }

        public Tensor Take()
        {
            throw new NotImplementedException();
        }
    }
}
