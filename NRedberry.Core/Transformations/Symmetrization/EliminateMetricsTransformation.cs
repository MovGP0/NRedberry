using NRedberry.Indices;
using NRedberry.Tensors;

namespace NRedberry.Transformations.Symmetrization;

/// <summary>
/// Skeleton port of cc.redberry.core.transformations.EliminateMetricsTransformation.
/// </summary>
public sealed class EliminateMetricsTransformation : TransformationToStringAble
{
    public static EliminateMetricsTransformation Instance => throw new NotImplementedException();

    private EliminateMetricsTransformation()
    {
        throw new NotImplementedException();
    }

    public static Tensor Eliminate(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    public Tensor Transform(Tensor tensor)
    {
        throw new NotImplementedException();
    }

    private Tensor Transform(Tensor tensor, IMetricsChain chain)
    {
        throw new NotImplementedException();
    }

    public string ToString(OutputFormat outputFormat)
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        throw new NotImplementedException();
    }

    private interface IMetricsChain
    {
        bool MergeWith(MetricWrapper metric);

        bool Add(MetricWrapper metric);

        SimpleTensor Apply(SimpleTensor tensor);

        IMetricsChain Clone();

        bool Equals(IMetricsChain other);
    }

    private sealed class MetricsChainImpl : IMetricsChain
    {
        public MetricsChainImpl(IMetricsChain parent)
        {
            throw new NotImplementedException();
        }

        private MetricsChainImpl(List<MetricWrapper> container, IMetricsChain parent)
        {
            throw new NotImplementedException();
        }

        public bool MergeWith(MetricWrapper metric)
        {
            throw new NotImplementedException();
        }

        public bool Add(MetricWrapper metric)
        {
            throw new NotImplementedException();
        }

        public SimpleTensor Apply(SimpleTensor tensor)
        {
            throw new NotImplementedException();
        }

        public IMetricsChain Clone()
        {
            throw new NotImplementedException();
        }

        public bool Equals(IMetricsChain other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }

    private sealed class DummyMetricsChain : IMetricsChain
    {
        public static DummyMetricsChain Instance => throw new NotImplementedException();

        private DummyMetricsChain()
        {
            throw new NotImplementedException();
        }

        public bool MergeWith(MetricWrapper metric)
        {
            throw new NotImplementedException();
        }

        public bool Add(MetricWrapper metric)
        {
            throw new NotImplementedException();
        }

        public SimpleTensor Apply(SimpleTensor tensor)
        {
            throw new NotImplementedException();
        }

        public IMetricsChain Clone()
        {
            throw new NotImplementedException();
        }

        public bool Equals(IMetricsChain other)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }

    private sealed class MetricWrapper : IComparable<MetricWrapper>
    {
        public MetricWrapper(Tensor metric)
        {
            throw new NotImplementedException();
        }

        private MetricWrapper(int first, int second, Tensor metric)
        {
            throw new NotImplementedException();
        }

        public int CompareTo(MetricWrapper other)
        {
            throw new NotImplementedException();
        }

        public SimpleTensor Apply(SimpleTensor tensor)
        {
            throw new NotImplementedException();
        }

        public bool Apply(MetricWrapper other)
        {
            throw new NotImplementedException();
        }

        public MetricWrapper Clone()
        {
            throw new NotImplementedException();
        }

        private sealed class SimpleMapping : IIndexMapping
        {
            public SimpleMapping(int from, int to)
            {
                throw new NotImplementedException();
            }

            public int Map(int value)
            {
                throw new NotImplementedException();
            }
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object? obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
