using NRedberry.Concurrent;
using NRedberry.IndexMapping;
using NRedberry.Tensors;

namespace NRedberry.Transformations.Substitutions;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.substitutions.SumBijectionPort.
/// </summary>
public sealed class SumBijectionPort : IOutputPort<BijectionContainer>
{
    public SumBijectionPort(Tensor from, Tensor to)
    {
        throw new NotImplementedException();
    }

    public BijectionContainer Take()
    {
        throw new NotImplementedException();
    }

    private static int[] Fill(int[] storage, List<int[]> bijections)
    {
        throw new NotImplementedException();
    }

    private interface IMapper
    {
        int[]? NextMapping(Mapping buffer);

        void Reset();
    }

    private abstract class AbstractMapper : IMapper
    {
        public int[]? NextMapping(Mapping buffer)
        {
            throw new NotImplementedException();
        }

        public virtual void Reset()
        {
            throw new NotImplementedException();
        }

        protected abstract int[]? NextMappingCore(Mapping buffer);
    }

    private abstract class AbstractStretchMapper : AbstractMapper
    {
        protected AbstractStretchMapper(Tensor[] from, Tensor[] to, int fromPointer)
        {
            throw new NotImplementedException();
        }

        protected bool Test(Mapping buffer)
        {
            throw new NotImplementedException();
        }
    }

    private interface IMapperSource : IMapper, IOutputPort<Mapping>
    {
    }

    private sealed class SinglePairSource : AbstractMapper, IMapperSource
    {
        public SinglePairSource(Tensor from, Tensor to, int fromPointer)
        {
            throw new NotImplementedException();
        }

        protected override int[]? NextMappingCore(Mapping buffer)
        {
            throw new NotImplementedException();
        }

        public Mapping Take()
        {
            throw new NotImplementedException();
        }
    }

    private sealed class SinglePairMapper : AbstractMapper
    {
        public SinglePairMapper(Tensor from, Tensor to, int fromPointer)
        {
            throw new NotImplementedException();
        }

        protected override int[]? NextMappingCore(Mapping buffer)
        {
            throw new NotImplementedException();
        }
    }

    private sealed class StretchPairSource : AbstractStretchMapper, IMapperSource
    {
        public StretchPairSource(Tensor[] from, Tensor[] to, int fromPointer)
            : base(from, to, fromPointer)
        {
            throw new NotImplementedException();
        }

        protected override int[]? NextMappingCore(Mapping buffer)
        {
            throw new NotImplementedException();
        }

        public Mapping Take()
        {
            throw new NotImplementedException();
        }
    }

    private sealed class StretchPairMapper : AbstractStretchMapper
    {
        public StretchPairMapper(Tensor[] from, Tensor[] to, int fromPointer)
            : base(from, to, fromPointer)
        {
            throw new NotImplementedException();
        }

        public override void Reset()
        {
            throw new NotImplementedException();
        }

        protected override int[]? NextMappingCore(Mapping buffer)
        {
            throw new NotImplementedException();
        }
    }
}
